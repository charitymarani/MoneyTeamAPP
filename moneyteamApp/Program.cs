using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using moneyteamApp.Commands;
using moneyteamApp.DataAccess;
using moneyteamApp.Controllers;
using moneyteamApp.Views;
using Serilog;
using moneyteamApp.Validators;
using moneyteamApp.models;

namespace moneyteamApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.File("../../../DataStore/logs.txt")
                .CreateLogger();

            Log.Logger.Information("Starting Apprication....");
            Console.WriteLine("Welcome to MoneyTeam App...");
            
           var host = CreateHostBuilder(args).UseSerilog().Build();

            var svc = ActivatorUtilities.CreateInstance<Main>(host.Services);
           svc.Run();
           Log.CloseAndFlush();
           Environment.Exit(0);
        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>

                {
                    // required to run the application
                    services.AddSingleton<IMain, Main>();
                    services.AddSingleton<ICommandManager, CommandManager>();
                    services.AddSingleton<IStore<Chama>, ChamaStore>();
                    services.AddSingleton<IStore<Group>, GroupStore>();
                    services.AddSingleton<IStore<Role>, RoleStore>();
                    services.AddSingleton<IStore<Location>, LocationStore>();
                    services.AddSingleton<IStore<Person>, MemberStore>();
                    services.AddSingleton<IStore< Notice>, NoticeStore > ();

                    //Add controllers

                    services.AddScoped<IController<Chama>, ChamaController>();
                    services.AddScoped<IController<Group>, GroupController>();
                    services.AddScoped<IController<Role>, RoleController>();
                    services.AddScoped<IController<Location>, LocationController>();
                    services.AddScoped<IController<Person>, MemberController>();
                    services.AddScoped<IController<Notice>, NoticeController>();

                    services.AddScoped<IView, BaseView>();
                    services.AddScoped<IViewManager, ViewManager>();
                    services.AddSingleton<InputValidator>();
                });
                



    }
}

