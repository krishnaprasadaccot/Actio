using System;
using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.RabbitMq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RawRabbit;

namespace Actio.Common.Services
{
    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost _webHost;

        public ServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }
        public void Run() => _webHost.Run();

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;

            var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
            .UseStartup<TStartup>();

            return new HostBuilder(webHostBuilder.Build());
        }

        public abstract class BuilderBase
        {
            public abstract ServiceHost Build();
        }

        public class HostBuilder : BuilderBase
        {
            private readonly IWebHost _webhost;
            private IBusClient _bus;
            public HostBuilder(IWebHost webHost)
            {
                _webhost = webHost;
            }

            public BusBuilder UseRabbitMq()
            {
                _bus = (IBusClient)_webhost.Services.GetService(typeof(IBusClient));
                return new BusBuilder(_webhost, _bus);
            }
            public override ServiceHost Build()
            {
                return new ServiceHost(_webhost);
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webhost;
            private IBusClient _bus;

            public BusBuilder(IWebHost webhost, IBusClient bus)
            {
                _webhost = webhost;
                _bus = bus;
            }

            public BusBuilder SubscribeToCommand<TCommand>() where TCommand : ICommand
            {
                var handler = (ICommandHandler<TCommand>)_webhost.Services.GetService(typeof(ICommandHandler<TCommand>));
                _bus.WithCommandHandlerAsync(handler);
                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
            {
                var handler = (IEventHandler<IEvent>)_webhost.Services.GetService(typeof(IEventHandler<IEvent>));
                _bus.WithEventHandlerAsync(handler);
                return this;
            }
            public override ServiceHost Build()
            {
                throw new NotImplementedException();
            }
        }
    }
}