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
        internal readonly List<Employee> r_ShiftManegerEmployees;
        internal readonly List<Employee> r_Employees;
        internal readonly Dictionary<DateTime, List<Employee>> r_ShiftTable = new Dictionary<DateTime, List<Employee>>();
        internal readonly List<Shift> r_ShiftQueue = new List<Shift>();
        internal readonly Dictionary<DateTime, int> r_EmptyShifts = new Dictionary<DateTime, int>();
        private readonly int r_MaxEmployeesPerShift;
        private readonly int r_MinEmployeesPerShift;
        public int AllUnmarkedShiftsForAllEmployees
        {
            get
            {
                int total = 0;
                foreach (Employee e in r_Employees)
                {
                    total += e.r_Shifts.Count;
                }
                return total;
            }
        }
        public int AllUnmarkedShiftsForAllSMEmployees
        {
            get
            {
                int total = 0;
                foreach (Employee e in r_ShiftManegerEmployees)
                {
                    total += e.r_Shifts.Count;
                }
                return total;
            }
        }

        private void sortAllShiftsPerWorker()
        {
            foreach (Employee employee in r_Employees)
            {
                employee.sortShifts();
            }
            foreach (Employee employee in r_ShiftManegerEmployees)
            {
                employee.sortShifts();
            }
            // sort employees by total weight of shifts descending but shiftmanagers first always
            r_Employees.Sort((x, y) => y.TotalWheight.CompareTo(x.TotalWheight));

            while (AllUnmarkedShiftsForAllSMEmployees > 0)
            {
                foreach (Employee employee in r_ShiftManegerEmployees)
                {
                    r_ShiftQueue.Add(employee.r_Shifts.First());
                    employee.MarkFirstShiftAsProccesed();
                }
            }
            while (AllUnmarkedShiftsForAllEmployees > 0)
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

                if (r_ShiftTable[shift.ShiftStart].Count() < r_MaxEmployeesPerShift )
                {
                    if (isEmployeeNotInAdjacentShifts(shift.Employee, shift) && isEmployeeNeededForShift(shift)) 
                    {
                        insertShift(shift);
                    }
                }
            }
        }

        private void insertShift(Shift i_Shift)
        {
            r_ShiftTable[i_Shift.ShiftStart].Add(i_Shift.Employee);
            r_EmptyShifts[i_Shift.ShiftStart]--;
        }

        private bool checkForEmptyShits()
        {
            foreach (KeyValuePair<DateTime, int> emptyShift in r_EmptyShifts)
            {
                if (emptyShift.Value > 0)
                {
                    return false; // There are still empty shifts
                }
            }
            return true; // All shifts are filled
        }

        public ShiftSortSolve GenerateNewShiftsTable()
        {
            sortAllShiftsPerWorker();
            fillTable();
            bool wasSuccesful = checkForEmptyShits();

            return new ShiftSortSolve(r_ShiftTable, r_EmptyShifts, wasSuccesful);
        }

        public Organizer(HashSet<DateTime> i_AllShiftStartTimes,int i_MinEmployeesInShift = 1, int i_MaxEmployeesInShift = 1)
        {
            r_AllShiftsStartTimes = i_AllShiftStartTimes;
            r_Employees = new List<Employee>();
            r_ShiftManegerEmployees = new List<Employee>();
            r_MinEmployeesPerShift = i_MinEmployeesInShift;
            r_MaxEmployeesPerShift= i_MaxEmployeesInShift;
            foreach (DateTime shiftTime in r_AllShiftsStartTimes)
            {
                r_ShiftTable[shiftTime] = new List<Employee>();
                r_EmptyShifts[shiftTime] = r_MinEmployeesPerShift;
            }
        }

        public void AddEmployees(List<Employee> i_Employees)
        {
            foreach (Employee employee in i_Employees)
            {
                if (!r_Employees.Contains(employee))
                {
                    if (employee.IsShiftManager)
                    {
                        r_Employees.Add(employee);
                    }
                    else
                    {
                        r_ShiftManegerEmployees.Add(employee);
                    }
                }
                else
                {
                    throw new Exception($"failed to add employee {employee.Name} to organizer");
                }
            }
        }

        private bool isEmployeeNotInAdjacentShifts(Employee i_employee, Shift i_shift)
        {
            bool notInNextShift = !(r_ShiftTable.TryGetValue(i_shift.NextShiftStart, out var employeesForNextShift) && employeesForNextShift.Contains(i_shift.Employee));
            bool notInPrevShift = !(r_ShiftTable.TryGetValue(i_shift.PrevShiftStart, out var employeesForPrevShift) && employeesForPrevShift.Contains(i_shift.Employee));

            return notInNextShift && notInPrevShift;
        }

        private bool isEmployeeNeededForShift(Shift i_shift)
        {
            return i_shift.Wheight>1||r_ShiftTable[i_shift.ShiftStart].Count < r_MinEmployeesPerShift;
        }
    }
}
