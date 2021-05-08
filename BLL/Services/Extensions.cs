using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public static class Extensions
    {
        public static int ToInt(this bool value) => value ? 1 : 0;
    }
}
