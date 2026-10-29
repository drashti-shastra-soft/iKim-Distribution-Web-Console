using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure
{
    public class APIResponseException : Exception
    {
        public APIResponseException(string error) : base(error)
        {

        }
    }
}
