using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftOrganizerLogic
{
    internal class Employee
    {
        public readonly List<Shift> r_Shifts = new List<Shift>();
        public readonly List<Shift> r_MarkedShifts = new List<Shift>();
        public string Name { get; set; }
        public int TotalWheight
        {
            get
            {
                int count = 0;
                foreach (Shift shift in r_Shifts)
                {
                    count += shift.Wheight;
                }
                return count;
            }
        }

        public void MarkFirstShiftAsProccesed()
        {
            //sort of dequeue 
            Shift ShiftToMark  = r_Shifts.First<Shift>();
            if (ShiftToMark != null)
            {
                r_Shifts.Remove(ShiftToMark);
                r_MarkedShifts.Add(ShiftToMark);
            }
            else
            {
                throw new Exception($"tried to mark already marked Shift for employee {Name}");
            }
        }
        public void AddShift(Shift shift)
        {
            if (!r_Shifts.Contains(shift))
            {
                r_Shifts.Add(shift);
            }
            else
            {
                throw new Exception($"failed to insert shift for employee: {Name}\n\r at start time {shift.ShiftStart}");
            }
        }

        internal void sortShifts()
        {
            r_Shifts.OrderBy(s => s.Wheight);
        }
    }
}
