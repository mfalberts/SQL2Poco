using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL2POCO
{
    public class POCOConfig
    {
        public string sqlCommand { get; set; }
        public string nameSpace { get; set; }
        public string className { get; set; }
        public string fileName { get; set; }
    }
}
