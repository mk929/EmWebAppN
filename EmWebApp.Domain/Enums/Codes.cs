using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmWebApp.Domain.Enums
{
    [System.Flags]
    public enum Group
    {
        Users = 1,
        Administrators = 2,
        Managers = 4
        
    }
}
