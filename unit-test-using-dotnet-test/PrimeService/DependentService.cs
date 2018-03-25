using System;
using System.Collections.Generic;
using System.Text;

namespace PrimeService
{
    public class DependentService : IDependentService
    {
        public bool IsTrue()
        {
            return true;
        }
    }
}
