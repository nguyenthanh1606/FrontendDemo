using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class DuplicatesDictionary<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            var element = new KeyValuePair<TKey, TValue>(key, value);
            this.Add(element);
        }
    }


    public class SysParaViewModel
    {
        public string ParaName { get; set; }
        public string ParaValue { get; set; }
        public string Description { get; set; }
    }
}