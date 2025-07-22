using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftOrganizerLogic
{
    public class Shift
    {
        private TimeSpan m_ShiftLength = TimeSpan.FromHours(8);
        public DateTime ShiftStart { get; private set; }
        public Employee Employee { get; private set; }
        public int Wheight { get; set; }
        public Shift(DateTime i_time, int i_Wheight,Employee i_employee)
        {
            ShiftStart = i_time;
            Wheight = i_Wheight;
            Employee = i_employee;
        }

        public DateTime NextShiftStart
        {
            get
            {
                return ShiftStart + m_ShiftLength;
            }
        }
        public DateTime PrevShiftStart
        {
            get
            {
                return ShiftStart - m_ShiftLength;
            }
        }


    }
}
