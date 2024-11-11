using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class GR<T>
    {
        public bool Success { get; set; }

        public T? Object { get; set; }

        public string Msg { get; set; }= string.Empty;
    }
}
