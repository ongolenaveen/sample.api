﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Template.Shared.Interfaces
{
    public interface IDataProviderStartup
    {
        void AddDataProviders(IServiceCollection services, IConfiguration configuration);
    }
}
