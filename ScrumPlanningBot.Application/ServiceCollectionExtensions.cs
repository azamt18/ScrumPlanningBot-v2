﻿using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ScrumPlanningBot.Application.Commands;

namespace ScrumPlanningBot.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBotCommands(this IServiceCollection services, Assembly? assembly = null)
        {
            var callingAssembly = assembly ?? Assembly.GetExecutingAssembly();
            var typesToRegister = callingAssembly
                .GetTypes()
                .Where(x => typeof(IBotCommand).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            foreach (var typeToRegister in typesToRegister)
            {
                services.AddTransient(typeof(IBotCommand), typeToRegister);
            }

            return services;
        }
    }
}