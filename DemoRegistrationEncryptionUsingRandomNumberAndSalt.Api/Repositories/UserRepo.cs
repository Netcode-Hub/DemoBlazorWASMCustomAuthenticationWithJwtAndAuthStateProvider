using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLogicLibrary.Models;
using SharedLogicLibrary.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly TokenSettings _tokenSettings;
        private readonly AppDbContext appDbContext;
        public UserRepo(AppDbContext appDbContext, IOptions<TokenSettings> tokenSettings)
        {
            this.appDbContext = appDbContext;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<Response> RegisterUserAsync(RegistrationEntity registrationEntity)
        {
            var user = await appDbContext.Users.Where(u => u.Email!.ToLower().Equals(registrationEntity.Email!.ToLower())).FirstOrDefaultAsync();
            if (user != null)
                return (new Response() { Success = false, Message = "Email alredy exist" });

            var newUser = new RegistrationEntity()
            {
                Name = registrationEntity.Name,
                Email = registrationEntity.Email,
                Password = HashPassword(registrationEntity.Password!)
            };

            appDbContext.Users.Add(newUser);
            await appDbContext.SaveChangesAsync();


            // add user role
            var recentlyUserAdded = await appDbContext.Users.Where(_=>_.Email ==  registrationEntity.Email).FirstOrDefaultAsync();
            var userRole = new UserRole();
            if (registrationEntity.Email!.StartsWith("admin"))
            {
                userRole.UserId = recentlyUserAdded!.Id;
                userRole.Role = "Admin";
            }
            else
            {
                userRole.UserId = recentlyUserAdded!.Id;
                userRole.Role = "User";
            }

            appDbContext.UserRoles.Add(userRole);
            await appDbContext.SaveChangesAsync();

            return (new Response() { Success = true, Message = "Successfully Created" });
        }



        private static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(salt);
            }
            var rfcPassword = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA1);
            byte[] rfcPasswordHash = rfcPassword.GetBytes(20);

            byte[] passwordHash = new byte[36];
            Array.Copy(salt, 0, passwordHash, 0, 16);
            Array.Copy(rfcPasswordHash, 0, passwordHash, 16, 20);
            return Convert.ToBase64String(passwordHash);
        }

        public async Task<UserSession> LoginUserAsync(LoginModel loginModel)
        {
            if (loginModel == null)
                return new UserSession();

            var user = await appDbContext.Users.Where(u => u.Email!.ToLower().Equals(loginModel.Email!.ToLower())).FirstOrDefaultAsync();
            var userRole = await appDbContext.UserRoles.Where(_ => _.UserId == user!.Id).FirstOrDefaultAsync();

            if (user is null || userRole is null)
                return new UserSession();

            bool DoPasswordsMatch = VerifyUserPassword(loginModel.Password!, user.Password!);
            if (!DoPasswordsMatch)
                return new UserSession();

            return GenerateJwtToken(user.Email!, userRole.Role!);
        }


        //Generate string for token
        private UserSession GenerateJwtToken(string email, string role)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey!));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role),
            });

            var securityTokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtTokenHandler.CreateToken(securityTokenDiscriptor);
            var token = jwtTokenHandler.WriteToken(securityToken);

            return new UserSession()
            {
                Role = role,
                Username = email,
                Token = token
            };
        }


        //Decrypt user database password and encrypt user raw password and compare
        private bool VerifyUserPassword(string rawPassword, string databasePassword)
        {
            byte[] dbPasswordHash = Convert.FromBase64String(databasePassword);
            byte[] salt = new byte[16];
            Array.Copy(dbPasswordHash, 0, salt, 0, 16);
            var rfcPassword = new Rfc2898DeriveBytes(rawPassword, salt, 1000, HashAlgorithmName.SHA1);
            byte[] rfcPasswordHash = rfcPassword.GetBytes(20);
            for (int i = 0; i < rfcPasswordHash.Length; i++)
            {
                if (dbPasswordHash[i + 16] != rfcPasswordHash[i])
                    return false;
            }
            return true;
        }
    }
}
