using EasyDefine.Dapper.Publish;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyDefine.Dapper.Core
{
    public static class ServicesDALExt
    {
        public static void AddEasyDefineDAL(this IServiceCollection services, Assembly assDAL)
        {
            InjectDALAll.Instances.Register(assDAL, services);
        }
    }
}
