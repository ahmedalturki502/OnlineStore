using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Mapping;


namespace OnlineStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));
        
        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<MappingProfile>();
        
        return services;
    }
}
