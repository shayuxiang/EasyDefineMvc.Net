using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasyDefine.Configuration.Runtime
{
    internal class JsonConfigurationHelper
    {
        public T GetAppSettings<T>(string key) where T : class, new()
        {
            try
            {
                var jsonpath = $@"{AppContext.BaseDirectory}";
                var jsonfile = "appsettings.json";
                #if DEBUG
                jsonfile = "appsettings.Development.json";
                #endif
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(jsonpath)
                    .Add(new JsonConfigurationSource { Path = jsonfile, Optional = false, ReloadOnChange = true })
                    .Build();
                var appconfig = new ServiceCollection()
                    .AddOptions()
                    .Configure<T>(config.GetSection(key))
                    .BuildServiceProvider()
                    .GetService<IOptions<T>>()
                    .Value;
                return appconfig;
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}
