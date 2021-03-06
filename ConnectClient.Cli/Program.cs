﻿using Autofac;
using CommandLine;
using ConnectClient.ActiveDirectory;
using ConnectClient.Core.Settings;
using ConnectClient.Core.Sync;
using ConnectClient.Rest;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConnectClient.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var parser = new Parser(config => config.HelpWriter = Console.Out);
            var result = parser.ParseArguments<Options>(args);

            await result.MapResult(async options => await Run(options), _ => Task.FromResult(1));

            Console.WriteLine("Press any key to close this window...");
            Console.ReadLine();
        }

        static async Task Run(Options options)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SettingsManager>().AsSelf().SingleInstance();

            builder.RegisterType<Client>().As<IClient>().SingleInstance();
            builder.RegisterType<LdapUserProvider>().As<ILdapUserProvider>().SingleInstance();

            builder.RegisterType<SyncEngine>().As<ISyncEngine>().SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            var container = builder.Build();
            var syncEngine = container.Resolve<ISyncEngine>();

            await syncEngine.SyncAsync(options.FullSync);
        }
    }
}
