using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Data;
using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext appDbContext;
        private readonly TokenSettings _tokenSettings;
        public UserRepo(AppDbContext appDbContext, IOptions<TokenSettings> tokenSettings)
        {
            this.appDbContext = appDbContext;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<Response> RegisterUserAsync(RegistrationModel registrationModel)
        {
            var user = await appDbContext.Users.Where(u => u.Email.ToLower().Equals(registrationModel.Email.ToLower())).FirstOrDefaultAsync();
            if (user != null)
                return (new Response() { Success = false, Message = "Email alredy exist" });

            var newUser = new RegistrationModel()
            {
                Name = registrationModel.Name,
                Email = registrationModel.Email,
                Password = HashPassword(registrationModel.Password)
            };

            appDbContext.Users.Add(newUser);
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

        public async Task<Response> LoginUserAsync(LoginModel loginModel)
        {
            if (loginModel == null)
                return new Response() { Success = false, Message = "Bad Request made" };

            var user = await appDbContext.Users.Where(u => u.Email.ToLower().Equals(loginModel.Email!.ToLower())).FirstOrDefaultAsync();
            if (user == null)
                return new Response() { Success = false, Message = "Invalid Email/Password" };

            bool DoPasswordsMatch = VerifyUserPassword(loginModel.Password!, user.Password);
            if (!DoPasswordsMatch)
                return new Response() { Success = false, Message = "Invalid Email/Password" };

            var jwtAccessToken = GenerateJwtToken(user.Id, user.Name, user.Email);
            return new Response() { AccessToken = jwtAccessToken, Success = true, Message = "Success" };
        }

        private string GenerateJwtToken(int id, string name, string email)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey!));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim("UserId", id.ToString()));
            claims.Add(new Claim("Fullname", name));
            claims.Add(new Claim("Email", email));

            var securityToken = new JwtSecurityToken
                (
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                expires: DateTime.Now.AddSeconds(20),
                signingCredentials: credentials,
                claims: claims
                );
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

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
