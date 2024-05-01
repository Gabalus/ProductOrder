using MessageBroker.Abstracts;
using MessageBroker.Concretes;
using OrderService.Configuration;
using OrderService.Workers;


namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContext<OrderContext>(contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);

            using (var context = new OrderContext())
            {
                context.Database.EnsureCreated();
            }

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            builder.Services.AddHostedService<CreateOrderWorker>();
            builder.Services.AddHostedService<UpdateOrderWorker>();
            builder.Services.AddHostedService<DeleteOrderWorker>();
            builder.Services.AddHostedService<ListOrderWorker>();

            builder.Services.Configure<MessageBrokerOptions>(
                builder.Configuration.GetSection(nameof(MessageBrokerOptions))
                );

            builder.Services.AddTransient<IRPCServer, RabbitRPCServer>();

            builder.Build().Run();
        }
    }
}