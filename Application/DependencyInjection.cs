using Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    /// <summary>
    /// Dependency injection configuration for Application layer
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Add Application layer services to DI container
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // MediatR - CQRS pattern
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                // Add validation behavior to pipeline
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // FluentValidation - Input validation
            services.AddValidatorsFromAssembly(assembly);

            // AutoMapper - Object mapping
            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
