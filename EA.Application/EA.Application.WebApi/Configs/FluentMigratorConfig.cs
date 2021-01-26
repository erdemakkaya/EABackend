using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace EA.Application.WebApi.Configs
{
    public static class FluentMigratorConfig
    {
        public static void Register()
        {
            var runner = ServiceProvider.GetRequiredService<IMigrationRunner>();
        }
    }
}
