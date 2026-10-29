using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Domain
{
    public interface IAuditable
    {
        int CreatedBy { get; set; }
        int? LastModifiedBy { get; set; }
    }
}
