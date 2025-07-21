using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftOrganizerLogic
{
    public class ShiftSortSolve
    {
        public readonly Dictionary<DateTime, List<Employee>> r_ShiftTable;
        public readonly Dictionary<DateTime, int> r_EmptyShifts;
        public readonly bool v_WasSortSuccesful;

        public ShiftSortSolve(Dictionary<DateTime, List<Employee>> i_Shifts, Dictionary<DateTime, int> i_EmptyShifts, bool i_WasSuccesful)
        {
            r_EmptyShifts = i_EmptyShifts;
            r_ShiftTable = i_Shifts;
            v_WasSortSuccesful = i_WasSuccesful;

        }
    }
}
