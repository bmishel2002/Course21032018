using System;

namespace PrimeService
{
    public class PrimeService
    {
        public PrimeService(IDependentService dependentService
            )
        {
            _dependentService = dependentService;
        }
        protected IDependentService _dependentService;

        public bool IsPrime(int number)
        {
            if (_dependentService.IsTrue())
            {
                return true;
            }

            if (number < 0 || number == 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
    }
}