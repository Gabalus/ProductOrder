
using MessageBroker.Abstracts;
using MessageBroker.Concretes;
using Microsoft.OpenApi.Models;
using OrderSystemAPI.Configuration;
using System.Reflection;

namespace OrderSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<MessageBrokerOptions>(
               builder.Configuration.GetSection(nameof(MessageBrokerOptions))
               );

            builder.Services.AddControllers();
        
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "OrderSystem API",
                    Version = "v1"
                });


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            }); ;
       

        builder.Services.AddTransient<IRPCClient, RabbitRPCClient>();

            var app = builder.Build();
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