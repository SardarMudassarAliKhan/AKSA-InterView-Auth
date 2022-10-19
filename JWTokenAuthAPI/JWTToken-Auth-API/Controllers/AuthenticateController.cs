using JWTToken_Auth_BAL.Common;
using JWTToken_Auth_DAL.Dto;
using JWTToken_Auth_DAL.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JWTToken_Auth_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        //private readonly ApplicationDbContext context;
        
       
        public AuthenticateController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            this.signInManager = signInManager;
        }

        
        
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword data)
        {
            try
            {
                var user = await userManager.FindByNameAsync(data.UserName);
                IdentityResult result = await userManager.ChangePasswordAsync(user, data.OldPassword,
                    data.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new Response { Status = "Success", Message = "Password Updated successfully!" });

                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "UnSuccessfull! Please check user details and try again." });
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
            
        }
        /*Forget Password API*/
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPassword objEmail)
        {
            try
            {
                if (string.IsNullOrEmpty(objEmail.emailaddress))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Email address missing." });
                }
                var user = await userManager.FindByEmailAsync(objEmail.emailaddress);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Email address not found." });
                }
                string Resettoken = await userManager.GeneratePasswordResetTokenAsync(user);
                Resettoken = HttpUtility.UrlEncode(Resettoken);
                Resettoken = Uri.EscapeDataString(Resettoken);
                string _confirmationLink = _configuration["ConfirmationURL:ResetPasswordURL"] + "?userId=" + user.Id + "&Token=" + Resettoken;
                return Ok(new { Status = "Success", Message = "Token generated successfully.", URL = _confirmationLink });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Error occured while processing." });
            }
        }
        /* RestPassWordConfermation*/
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel reqObj)
        {
            try
            {
                if (reqObj == null || string.IsNullOrEmpty(reqObj.userid) || string.IsNullOrEmpty(reqObj.token))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Param are missing." });
                }
                var user = await userManager.FindByIdAsync(reqObj.userid); //.Result;
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "token verification failed." });
                }
                string codeHtmlVersion = HttpUtility.UrlDecode(reqObj.token);
                var result = await userManager.ResetPasswordAsync(user, codeHtmlVersion, reqObj.newpassword);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "token verification failed" });
                }
                return Ok(new { Status = "Success", Message = "Password changed successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Error occured while processing your request." });
            }
        }



        /*End Point for Creation of Admin Account for Mall App*/

        [HttpPost]
        [Route("AdminRegister")]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Name = model.Name,
                SecurityStamp = Guid.NewGuid().ToString(),
                AccountType = model.AccountType,
                PhoneNo = model.PhoneNo,
                UserName = model.UserName.ToString(),
                Email = model.Email,    
                Password = model.Password,
                UserRole = model.Role
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));


            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        /*End Point for AdminLogin*/

        [HttpPost]
        [Route("AdminLogin")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    api_key = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = user,
                    Role = userRoles,
                    status = "success"
                });
            }
            return Unauthorized();
        }

        /*End Point for Creation of  Account for Mall App*/

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser user = new ApplicationUser()
            {
                Name = model.Name,
                SecurityStamp = Guid.NewGuid().ToString(),
                AccountType = model.AccountType,
                PhoneNo = model.PhoneNo,
                UserName = model.UserName,
                Email = model.Email,
                UserRole = model.Role,
            };
            
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }
            else
            {
                await roleManager.CreateAsync(new IdentityRole(model.Role));
                await userManager.AddToRoleAsync(user, model.Role);
            }
            return Ok(new { Status = "Success", Message = "User created successfully!"});
            #region
            #endregion
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = user,
                    Role = userRoles,
                    status = "success"
                });
            }
            return Unauthorized();
        }


    }
}
