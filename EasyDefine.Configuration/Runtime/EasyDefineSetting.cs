using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration.Runtime
{
    public class EasyDefineSetting
    {
        public string MasterDb { get; set; }

        public List<string> SlaveDb { get; set; }

        public string MongoMasterDb { get; set; }

        public List<string> MongoSlaveDb { get; set; }

        public string SOASolution { get; set; }

        public string DALSolution { get; set; }

        public string DtoSolution { get; set; }
    }
}
