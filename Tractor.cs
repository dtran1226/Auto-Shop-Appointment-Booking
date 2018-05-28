using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8145_Assignment4
{
    class Tractor : Vehicle
    {
        public Tractor()
        {
        }

        public Tractor(int yearOfMake, string makingCompany, string model)
        {
            YearOfMake = yearOfMake;
            MakingCompany = makingCompany;
            Model = model;
        }

        public override void ExtraTask()
        {
            Console.WriteLine("PTO maintenance");
        }

        public override string ToString()
        {
            return string.Format("This is Tractor which is made in {0} by {1} and its model is {2}", YearOfMake, MakingCompany, Model);
        }
    }
}
