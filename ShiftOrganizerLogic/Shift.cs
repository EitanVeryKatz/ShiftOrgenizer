using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftOrganizerLogic
{
    internal class Shift
    {
        public DateTime ShiftStart { get; private set; }
        public Employee Employee { get; private set; }
        public int Wheight { get; private set; }
        public Shift(DateTime i_time, int i_Wheight,Employee i_employee)
        {
            ShiftStart = i_time;
            Wheight = i_Wheight;
            Employee = i_employee;
        }
    }
}
