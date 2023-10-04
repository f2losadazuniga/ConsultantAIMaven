using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using LogicaNegocioServicio.Autenticacion;
using LogicaNegocioServicio.Middleware;
using LogicaNegocioServicio.Comunes;
using ApiIntegracionEntregasLogyTech.Controllers;
using ApiConsultantAIMaven.Controllers;

namespace ApiIntegracionEntregasLogyTech
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(ILogger), (object)new Logger(this.Configuration)));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddControllers();

            services.AddScoped<IUserToken, UserToken>();
      

            services.AddHttpClient();
          
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "Api Consultant AI Maven",
                    Version = "V1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name="Authorization",
                    Type= SecuritySchemeType.ApiKey,
                    Scheme="Bearer",
                    BearerFormat="JWT",
                    In=ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                      {
                        {            new OpenApiSecurityScheme
                                    {
                                    Reference=new OpenApiReference
                                        {
                                            Type=ReferenceType.SecurityScheme,
                                            Id="Bearer"
                                        }
                                }, new string[]{ } 
                        }
                       });
            });



            // se adiciona autenticacion 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                    ClockSkew = TimeSpan.Zero
                });
            //services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();
            services.AddIdentity<UserToken, IdentityRole>()
              .AddDefaultTokenProviders();
            services.AddScoped<CuentasController>();
            services.AddScoped<ChatGPTController>();
            services.AddScoped<ChatGPTFineTunesAdministracionController>();
            services.AddScoped<ChatGPTFineTunesApiController>();
            services.AddScoped<DynamicsController>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            if (env.IsDevelopment())            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("V1/swagger.json", "Api Integracion Entregas LogyTech");
            });
            //app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
