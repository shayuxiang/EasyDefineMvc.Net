using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Configuration
{
    [AttributeUsage(AttributeTargets.All)]
    public class ApiDescription : Attribute
    {
        private string _text;

        public ApiDescription(string txt)
        {
            this._text = txt;
        }

        public override string ToString()
        {
            return this._text;
        }
    }
}
