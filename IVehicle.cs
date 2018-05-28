using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8145_Assignment4
{
    interface IVehicle
    {
        int YearOfMake { get; set; }
        string MakingCompany { get; set; }
        string Model { get; set; }

        void OilChange();

        void EngineTuneup();

        void TransmissionCleanup();

        void ExtraTask();
    }
}
