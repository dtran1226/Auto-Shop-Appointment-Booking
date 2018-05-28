using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8145_Assignment4
{
    abstract class Vehicle : IVehicle
    {
        private int yearOfMake;
        private string makingCompany;
        private string model;

        public int YearOfMake { get => yearOfMake; set => yearOfMake = value; }
        public string MakingCompany { get => makingCompany; set => makingCompany = value; }
        public string Model { get => model; set => model = value; }

        public virtual void TransmissionCleanup()
        {
            Console.WriteLine("Transmission cleanup");
        }

        public virtual void EngineTuneup()
        {
            Console.WriteLine("Engine tuneup");
        }

        public void OilChange()
        {
            Console.WriteLine("Oil change");
        }

        public abstract void ExtraTask();
    }
}
