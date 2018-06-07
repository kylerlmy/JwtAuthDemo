using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtAuth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            // var setings=Configuration.GetSection("JwtSettings:Audience");
            // services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));//获取Json文件中的某一个节点的配置，如果直接传递Configuration，拿不到配置
            // var jwtSettings = new JwtSettings();
            // Configuration.Bind("JwtSettings", jwtSettings);

            //认证
            services.AddAuthentication (options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer (o => {
                    o.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"], //jwtSettings.Issuer,
                    ValidAudience = Configuration["JwtSettings:Audience"], //jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Configuration["JwtSettings:SecretKey"]))
                    };
                });

            //授权
            services.AddAuthorization (option => {
                option.AddPolicy ("SuperAdminOnly", policy => policy.RequireClaim ("SuperAdminOnly"));
            });
            services.AddMvc ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseAuthentication ();
            app.UseMvc ();
        }
    }
}