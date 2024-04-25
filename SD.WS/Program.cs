
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SD.Application.Extensions;
using SD.Application.Movies;
using SD.Persistence.Extensions;
using SD.Persistence.Repositories.DBContext;
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
            
            builder.Services.AddSwaggerGen();

            builder.WebHost.UseIISIntegration();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
