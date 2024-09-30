using DataLayer;
using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Services
{
    public class AuthService : IAuth
    {
        private readonly UserManager<AuthUser> _UserManager;
        private readonly JWTEF _Jwt;

        public AuthService(UserManager<AuthUser> UserManager, IOptions<JWTEF> jwt)
        {
            _UserManager = UserManager;
            _Jwt = jwt.Value;
        }

        private async Task<JwtSecurityToken> CreateSecureToken(AuthUser User)
        {
            IList<Claim> UserClaims = await _UserManager.GetClaimsAsync(User);
            IList<string> roles = await _UserManager.GetRolesAsync(User);
            List<Claim> RoleClaims = new List<Claim>();

            foreach (var role in roles)
                RoleClaims.Add(new Claim("role", role));
            Claim[] Claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, User.UserName!),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, User.Email!),
                 new Claim("uid", User.Id)
            }
            .Union(UserClaims)
            .Union(RoleClaims).ToArray();

            SymmetricSecurityKey SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Jwt.Key));
            SigningCredentials SigningCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken JwtSecurityToken = new JwtSecurityToken(
                _Jwt.Issuer,
                _Jwt.Audience,
                Claims,
                DateTime.Now,
                DateTime.Now.AddDays(_Jwt.DurationInDays),
                SigningCredentials);
            return JwtSecurityToken;
        }

        public async Task<AuthEF> RegisterAsync(RegisterEF Model)
        {
            if (Model is null)
                throw new ArgumentNullException(nameof(Model));
            if (await _UserManager.FindByEmailAsync(Model.Email) != null)
                return new AuthEF { Message = "User with this Email is already exist" };
            if (await _UserManager.FindByNameAsync(Model.UserName) != null)
                return new AuthEF { Message = $"User with this {Model.UserName} is already exist" };

            AuthUser User = new AuthUser()
            {
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                UserName = Model.UserName,
                Email  = Model.Email
            };

            var Result = await _UserManager.CreateAsync(User, Model.Password);

            if (!Result.Succeeded)
            {
                string Error = string.Empty;

                for (int i = 0; i < Result.Errors.Count(); i++)
                {
                    if (i < Result.Errors.Count() - 1)
                        Error += $"{Result.Errors.ToArray()[i].Description}, ";
                    else
                        Error += $"{Result.Errors.ToArray()[i].Description}";  
                    return new AuthEF { Message = Error };
                }
            }
            await _UserManager.AddToRoleAsync(User, "User");
            JwtSecurityToken Token = await CreateSecureToken(User);

            AuthEF Auth = new AuthEF()
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                ExpiredTime = Token.ValidTo,
                IsAuthenticed = true,
                Roles = new List<string> { "User" },
                UserName = User.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(Token)
            };
            return Auth;
        }

        public async Task<AuthEF> LoginAsync(LoginEF Model)
        {
            AuthUser? User = string.IsNullOrWhiteSpace(Model.UserName) ? 
                await _UserManager.FindByEmailAsync(Model.Email) : 
                await _UserManager.FindByNameAsync(Model.UserName);

            AuthEF Auth = new AuthEF();
            if (User == null || !await _UserManager.CheckPasswordAsync(User, Model.Password))
            {
                Auth.Message = "Email or password is incorrect!"; 
                return Auth;
            }

            var Token = await CreateSecureToken(User);
            var Roles = await _UserManager.GetRolesAsync(User);

            Auth.FirstName = User.FirstName;
            Auth.LastName = User.LastName;
            Auth.Email = User.Email!;
            Auth.UserName = User.UserName!;
            Auth.IsAuthenticed = true;
            Auth.Token = new JwtSecurityTokenHandler().WriteToken(Token);
            Auth.ExpiredTime = Token.ValidTo;
            Auth.Roles = Roles.ToList();

            return Auth;
        }
    }
}
