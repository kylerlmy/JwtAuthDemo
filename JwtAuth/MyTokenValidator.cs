using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth {
    public class MyTokenValidator : ISecurityTokenValidator {
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken (string securityToken) {
            return true;
        }

        public ClaimsPrincipal ValidateToken (string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken) {
            validatedToken = null;

            var identity = new ClaimsIdentity (JwtBearerDefaults.AuthenticationScheme);

            if (securityToken == "adcdefg") {
                identity.AddClaim (new Claim ("name", "kyle"));
                identity.AddClaim (new Claim ("SuperAdminOnly", "true"));
                identity.AddClaim (new Claim (ClaimsIdentity.DefaultRoleClaimType, "user"));
            }

            var claimsPrincipal = new ClaimsPrincipal (identity);

            return claimsPrincipal;
        }
    }
}