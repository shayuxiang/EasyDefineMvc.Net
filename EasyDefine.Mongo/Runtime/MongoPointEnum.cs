using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Mongo.Runtime
{
    /// <summary>
    /// MongoDB的主从库指向
    /// </summary>
    public enum MongoPointEnum
    {
        Master = 0,
        Slave = 1
    }
}
