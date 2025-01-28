using LHF.Solid.Business.Intefaces;
using LHF.Solid.Business.Services;
using LHF.Solid.Data.Context;
using LHF.Solid.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace LHF.Solid.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Registro das dependências
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Adicionar controladores e Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configuração do ambiente de desenvolvimento
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configuração de middlewares
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Lógica de negócios diretamente na classe Program (violação do SRP)
            var productService = app.Services.GetRequiredService<IProductService>();

            app.Run();
        }
    }
}