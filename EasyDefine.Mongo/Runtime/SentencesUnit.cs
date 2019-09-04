using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Mongo.Runtime
{
    public class SentencesUnit
    {
        public string Id { get; private set; }
        public int BeginPos { get; set; }
        public int EndPos { get; set; }

        public string Command { get; set; }

        public SentencesUnit() {
            Id = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return $@"Begin:{BeginPos},End:{EndPos},Command:{Command}";
        }
    }
}
