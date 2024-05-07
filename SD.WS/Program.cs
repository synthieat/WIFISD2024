
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SD.Application.Authentication;
using SD.Application.Extensions;
using SD.Application.Movies;
using SD.Common.Services;
using SD.Persistence.Extensions;
using SD.Persistence.Repositories.DBContext;
using SD.Rescources;
using SD.Rescources.Attributes;
using System.Reflection;

namespace SD.WS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /* DBContext registrieren */
            var connectionString = builder.Configuration.GetConnectionString("MovieDbContext");
            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));

            /* Bootstrapping von Handler und Repositories */
            builder.Services.RegisterRepositories();
            builder.Services.RegisterApplicationService();

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            /* MediatR registrieren */
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MovieQueryHandler).GetTypeInfo().Assembly));

            /* UserService registrieren */
            builder.Services.AddScoped<IUserService, UserService>();

            /* BasicAuthection handler registrieren */
            builder.Services.AddAuthentication(nameof(BasicAuthenticationHandler))
                            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(nameof(BasicAuthenticationHandler), null);



            builder.Services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Wifie SW-Developer 2023-2024",
                    Version = "v1",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact { Email = "horst.schneider@hotmail.com", Name = "Horst Schneider" }
                });

                g.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authentication header usering basic scheme"
                });

                g.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string []{ }
                    }
                });

            });

            builder.WebHost.UseIISIntegration();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            LocalizedDescriptionAttribute.Setup(new System.Resources.ResourceManager(typeof(BasicRes)));

            app.Run();


        }
    }
}
