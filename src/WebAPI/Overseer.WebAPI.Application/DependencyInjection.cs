using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Overseer.WebAPI.Application.Common.Behaviours;

namespace Overseer.WebAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        services.AddMediatR(static cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(IdempotentCommandBehaviour<,>));

            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(IdempotentCommandBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}
