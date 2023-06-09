
using _01_EFCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _01_EFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //.NET web api validation devre d��� etmek i�in
            //builder.Services.Configure<ApiBehaviorOptions>(options =>
            //    options.SuppressModelStateInvalidFilter = true
            //);

            builder.Services.AddDbContext<ToDoDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"))
            );

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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