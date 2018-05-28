using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8145_Assignment4
{
    class Appointment : IComparable
    {
        private string time;
        private Vehicle vehicle;
        private string task;

        public Appointment()
        {
        }

        public Appointment(string time, Vehicle vehicle, string task)
        {
            this.time = time;
            this.vehicle = vehicle;
            this.task = task;
        }

        public string Time { get => time; set => time = value; }
        public string Task { get => task; set => task = value; }
        internal Vehicle Vehicle { get => vehicle; set => vehicle = value; }

        public int CompareTo(object obj)
        {
            Appointment appointment = (Appointment)obj;
            return (Time).CompareTo(appointment.Time);
        }

        public override string ToString()
        {
            return string.Format("Time: {0}, {1}, Task: {2}\n\n", Time, Vehicle, Task);
        }
    }
}
