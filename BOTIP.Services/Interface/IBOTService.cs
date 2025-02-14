using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTIP.Services.Interface
{
    public interface IBOTService
    {
        public Dictionary<string, int> Process(string path);
    }
}
