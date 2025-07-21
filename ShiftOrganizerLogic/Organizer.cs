using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShiftOrganizerLogic
{
    internal class Organizer
    {
        internal readonly HashSet<DateTime> r_AllShiftsStartTimes = new HashSet<DateTime>();
        internal readonly List<Employee> r_Employees;
        internal readonly Dictionary<DateTime, List<Employee>> r_ShiftTable = new Dictionary<DateTime, List<Employee>>();
        internal readonly List<Shift> r_ShiftQueue = new List<Shift>();
        internal readonly Dictionary<DateTime, int> r_EmptyShifts = new Dictionary<DateTime, int>();
        private const int k_MaxEmployeesPerShift = 1;//change in ctor
        private const int k_MinEmployeesPerShift = 1;

        private void sortAllShiftsPerWorker()
        {
            foreach (Employee employee in r_Employees)
            {
                employee.sortShifts();
            }

            r_Employees.OrderBy(p=>p.TotalWheight);
            while(r_Employees.Count > 0)
            {
                foreach (Employee employee in r_Employees) 
                {
                    r_ShiftQueue.Add(employee.r_Shifts.First());
                    employee.MarkFirstShiftAsProccesed();
                
                }
            }
        }

        private void fillTable()
        {
            foreach (Shift shift in r_ShiftQueue)
            {
                if (!r_ShiftTable.ContainsKey(shift.ShiftStart))
                {
                    r_ShiftTable[shift.ShiftStart] = new List<Employee>();
                }

                if (r_ShiftTable[shift.ShiftStart].Count() < k_MaxEmployeesPerShift)
                {
                    r_ShiftTable[shift.ShiftStart].Add(shift.Employee);
                }
            }
        }

        private bool checkForEmptyShits()
        {
            foreach(DateTime shiftTime in r_AllShiftsStartTimes)
            {
                if(r_ShiftTable.TryGetValue(shiftTime,out List<Employee> shiftEmployees))
                {
                    if (shiftEmployees.Count < k_MinEmployeesPerShift)
                    {
                        r_EmptyShifts[shiftTime] = k_MinEmployeesPerShift - shiftEmployees.Count;
                    }
                }
                else
                {
                    r_EmptyShifts[shiftTime] = k_MinEmployeesPerShift;
                }
            }
            foreach (KeyValuePair<DateTime, int> emptyShift in r_EmptyShifts)
            {
                if (emptyShift.Value > 0)
                {
                    return false; // There are still empty shifts
                }
            }
            return true; // All shifts are filled
        }

        public void GenerateNewShiftsTable()
        {
            sortAllShiftsPerWorker();
            fillTable();
            if (checkForEmptyShits())
            {
                return
            }
            //when all shifts ended in all employees locate empty slots in table
            //if null return ok
            //else return list of empty shifts
        }
    }
}
