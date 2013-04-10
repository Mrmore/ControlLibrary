using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RenrenCoreWrapper.Helper
{
    public class BatchRunBinder
    {
        public string Method { get; set; }
        public IDictionary<string, string> Pairs { get { return _Pairs; } }
        public Type RespType { get; set; }

        public void AddPair(string param, string value)
        {
            if (!_Pairs.ContainsKey(param))
            {
                this._Pairs.Add(param, value);
            }
        }

        private IDictionary<string, string> _Pairs = new Dictionary<string, string>();
    }
}
