
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApi.Data;
using WepApi.Helper;
using WepApi.InterFaces;
using WepApi.Repository;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<ICustomerInterface, CustomerRepository>();
            builder.Services.AddScoped<IOrdersInterface, OrderRepository>();
            builder.Services.AddScoped<IProductInterface, ProductRepository>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddDbContext<DataContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



            var app = builder.Build();

            app.UseCors("AllowAll");
            app.UseRouting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
