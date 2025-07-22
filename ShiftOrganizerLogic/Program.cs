using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftOrganizerLogic
{
    internal class Program
    {
        public static void Main()
        {
            try
            {
                //create shifts, employees and run a test, all output logged to console
                HashSet<DateTime> allShiftsStartTimes = new HashSet<DateTime>();

                for (int i = 0; i < 21; i++)
                {
                    allShiftsStartTimes.Add(DateTime.Now.AddHours(i * 8));
                }
                Random random = new Random();
                Organizer organizer = new Organizer(allShiftsStartTimes);
                Employee employee1 = new Employee("Alice", new List<Shift>(), allShiftsStartTimes);
                Employee employee2 = new Employee("Bob", new List<Shift>(), allShiftsStartTimes);
                Employee employee3 = new Employee("Charlie", new List<Shift>(), allShiftsStartTimes);
                Employee employee4 = new Employee("Diana", new List<Shift>(), allShiftsStartTimes);
                List<Employee> employees = new List<Employee> { employee1, employee2, employee3, employee4 };
                foreach (Employee employee in employees)
                {
                    foreach (DateTime shiftStart in allShiftsStartTimes)
                    {
                        Shift shift = new Shift(shiftStart, random.Next(4), employee);
                        employee.AddShift(shift);
                    }
                }
                organizer.AddEmployees(employees);
                ShiftSortSolve result = organizer.GenerateNewShiftsTable();
                if (result.v_WasSortSuccesful)
                {
                    Console.WriteLine("Shift sorting was successful!");
                    foreach (var shift in result.r_ShiftTable)
                    {
                        Console.WriteLine($"Shift at {shift.Key}:");
                        foreach (var emp in shift.Value)
                        {
                            Console.WriteLine($" - {emp.Name}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Shift sorting failed.");
                    //log the empty shifts
                    foreach (var emptyShift in result.r_EmptyShifts)
                    {
                        Console.WriteLine($"Shift at {emptyShift.Key} is empty, needs {emptyShift.Value} more employees.");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);

            }

        }
    }
}
