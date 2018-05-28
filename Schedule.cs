using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8145_Assignment4
{
    class Schedule
    {
        private List<Appointment> appointments = null;

        private List<Appointment> Appointments { get => appointments; set => appointments = value; }

        public Schedule()
        {
            appointments = new List<Appointment>();
        }

        public Schedule(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }

        public Appointment this[int i]
        {
            get { return appointments[i]; }
            set { appointments[i] = value; }
        }

        public void Add(Appointment appointment)
        {
            appointments.Add(appointment);
        }

        public void Remove(Appointment appointment)
        {
            appointments.Remove(appointment);
        }

        public int Count()
        {
            return appointments.Count();
        }

        public void Sort()
        {
            appointments.Sort();
        }

        public void PrintOut()
        {
            for (int i = 0; i < Count(); i++)
            {
                Console.WriteLine(this[i].ToString());
            }
        }
    }
}
