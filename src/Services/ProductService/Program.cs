using MessageBroker.Abstracts;
using MessageBroker.Concretes;
using ProductService.Configuration;
using ProductService.Workers;

namespace ProductService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContext<ProductContext>(contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);

            using (var context = new ProductContext())
            {
                context.Database.EnsureCreated(); 
            }

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            builder.Services.AddHostedService<CreateProductWorker>();
            builder.Services.AddHostedService<UpdateProductWorker>();
            builder.Services.AddHostedService<DeleteProductWorker>();
            builder.Services.AddHostedService<ListProductWorker>();

            builder.Services.Configure<MessageBrokerOptions>(
                builder.Configuration.GetSection(nameof(MessageBrokerOptions))
                );

            builder.Services.AddTransient<IRPCServer, RabbitRPCServer>();

            builder.Build().Run();


        }
    }
}