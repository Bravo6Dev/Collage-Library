
using BuisnessLayer;
using DataLayer;
using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.EntityFrameworkCore;

namespace CollageControllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ResourcesDbContext>(Options =>
            Options.UseSqlServer(builder.Configuration.GetConnectionString("Connstr")));

            builder.Services.AddScoped<ICollageRepo, CollageServices>();
            builder.Services.AddScoped<ISpecialtiesRepo, SpecialtiesService>();
            builder.Services.AddScoped<ISemestersRepo, SemestersService>();

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
