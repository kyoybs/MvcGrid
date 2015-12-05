using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synvata.ORM
{
    public class SavableException: Exception
    {
        private SavableException() { }

        public SavableException(string message) : base(message) { }
    }
}
