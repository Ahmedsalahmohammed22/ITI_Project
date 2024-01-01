
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestRestApi.Data;
using TestRestApi.Data.Models;
using TestRestApi.Extension;

namespace TestRestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddDbContext<AppDbContext>(op =>
                op.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MyCon"))
            );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGenJwtAuth();
            builder.Services.AddIdentity<AppUser , IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddCustomJwtAuthExtension(builder.Configuration);

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

            app.Run();
        }
    }
}
