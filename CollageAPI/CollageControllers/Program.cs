using BuisnessLayer.Services;
using DataLayer;
using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CollageControllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ResourcesDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("Connstr"));
            });

            // Mapping the the JWT Json object with JWT class
            builder.Services.Configure<JWTEF>(builder.Configuration.GetSection("JWT"));

            // Add identity to our services
            builder.Services.AddIdentity<AuthUser, IdentityRole>()
                .AddEntityFrameworkStores<ResourcesDbContext>();

            // Add The Authentication
            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(O =>
                {
                    O.RequireHttpsMetadata = false;
                    O.SaveToken = false;
                    O.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
                    };
                });

            // Dependency injection for classes 
            builder.Services.AddScoped<ICollageRepo, CollageServices>();
            builder.Services.AddScoped<ISpecialtiesRepo, SpecialtiesService>();
            builder.Services.AddScoped<ISemesterRepo, SemestersService>();
            builder.Services.AddScoped<ISubjectRepo, SubjectsService>();
            builder.Services.AddScoped<IReferencesRepo, ReferencesService>();
            builder.Services.AddScoped<IAuth, AuthService>();

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
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
