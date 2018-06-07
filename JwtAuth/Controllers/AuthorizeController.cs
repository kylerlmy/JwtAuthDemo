using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtAuth.Models;
using JwtAuth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Controllers {
    [Route ("api/[controller]")]//不使用Route特性（请求地址设置为http://localhost:5000/Authorize/Token）时，返回404
    public class AuthorizeController : Controller {
        //  private JwtSettings _jwtSettings;
        private IConfiguration _config;
        public AuthorizeController (IConfiguration config) { //IOptions<JwtSettings> _jwtSettingsAccesser
            // _jwtSettings = _jwtSettingsAccesser.Value;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Token ([FromBody] LoginViewModel viewModel) {//不使用[FromBody]返回400（bad request）
            if (ModelState.IsValid) {
                if (!(viewModel.User == "kyle" && viewModel.Password == "123456"))
                    return BadRequest ();

                /*标准中注册的声明 (建议但不强制使用)
                iss: jwt签发者
                sub: jwt所面向的用户
                aud: 接收jwt的一方
                exp: jwt的过期时间，这个过期时间必须要大于签发时间
                nbf: 定义在什么时间之前，该jwt都是不可用的.
                iat: jwt的签发时间
                jti: jwt的唯一身份标识，主要用来作为一次性token,从而回避重放攻击。
                */  

                var claims = new Claim[] {
                    new Claim (ClaimTypes.Name, "kyle"), 
                    new Claim (ClaimTypes.Role, "admin"),
                    new Claim("SuperAdminOnly","true")
                };

                var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_config["JwtSettings:SecretKey"])); //_jwtSettings.SecretKey
                var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken (_config["JwtSettings:Issuer"], //_jwtSettings.Issuer
                    _config["JwtSettings:Audience"], claims, DateTime.Now, DateTime.Now.AddMinutes (30), //_jwtSettings.Audience
                    creds
                );

                return Ok (new { token = new JwtSecurityTokenHandler ().WriteToken (token) });
            }

            return BadRequest ();

        }

    }
}