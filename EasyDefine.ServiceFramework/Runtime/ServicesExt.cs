using EasyDefine.ServiceFramework.Publish;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyDefine.ServiceFramework.Runtime
{
    public static class ServicesExt
    {
        public static void AddEasyDefineSOA(this IServiceCollection services, Assembly assDAL)
        {
            InjectSOAAll.Instances.Register(assDAL, services);
        }
    }
}
