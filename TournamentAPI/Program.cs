using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TournamentAPI.Api.Extensions;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;
using TournamentAPI.Data.Repositories;

namespace TournamentAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TournamentAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentAPIContext") ?? throw new InvalidOperationException("Connection string 'TournamentAPIContext' not found.")));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(TournamentMappings));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                await app.SeedDataAsync();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
