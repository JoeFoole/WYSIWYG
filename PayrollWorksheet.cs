using System;
using System.Collections.Generic;
using static WYSIWYG.Employee;

namespace WYSIWYG
{
    public class PayrollWorksheet
    {
        public static void Worksheet()
        {
            Dictionary<int, TimeSlip> employeeList = new Dictionary<int, TimeSlip>()
            {
                {1, new TimeSlip { FirstName = "Kok", LastName = "Tang", FilingStatus = 'M', PersonalExemption = 1, Rate = 1400.00, Hourly = false } },
                {2, new TimeSlip { FirstName = "Mei", LastName = "Hueng", FilingStatus = 'M', PersonalExemption = 1, Rate = 1000.00, Hourly = false } },
                {7, new TimeSlip { FirstName = "Martin", LastName = "Negrete", FilingStatus = 'M', PersonalExemption = 5, Rate = 11.30, Hourly = true } },
                {28, new TimeSlip { FirstName = "Ruby", LastName = "Tang", FilingStatus = 'S', PersonalExemption = 1, Rate = 11.30, Hourly = true } },
                {88, new TimeSlip { FirstName = "Oscar", LastName = "Valencia", FilingStatus = 'M', PersonalExemption = 7, Rate = 11.30, Hourly = true } },
                {222, new TimeSlip { FirstName = "Rumiko", LastName = "Hawes", FilingStatus = 'M', PersonalExemption = 5, Rate = 11.30, Hourly = true } },
                {224, new TimeSlip { FirstName = "Ya Fang", LastName = "Huang", FilingStatus = 'M', PersonalExemption = 1, Rate = 11.30, Hourly = true } },
                {225, new TimeSlip { FirstName = "Christian", LastName = "Hawes", FilingStatus = 'S', PersonalExemption = 1, Rate = 11.30, Hourly = true } },
                {226, new TimeSlip { FirstName = "Celente", LastName = "Hawes", FilingStatus = 'S', PersonalExemption = 1, Rate = 11.30, Hourly = true} }
            };            
                
            string entry;
            Console.Write("{0,8} {1,-9}    ", "Employee", "Name");
            Console.Write("Reg     OT        Gross       SS      Medicare      FWH        SWH       STT        WC         Net");
            Console.Write("\n\n");

            foreach (var employee in employeeList)
            {
                Console.Write("{0,8}, {1,-9}  : ", employee.Value.LastName, employee.Value.FirstName);
                entry = ReadNoReturn.OnEnter();
                double.TryParse(entry, out double hours);
                employee.Value.RegularHours = hours;

                if (hours == 0)
                {   // If there is no hours, Salary is assigned or next employee is display.
                    if (employee.Value.Hourly == true)
                    {
                        Console.Write("\n\n");
                        continue;
                    }
                    else
                    {
                        employee.Value.GrossWage = employee.Value.Rate;
                        employee.Value.RegularWages = employee.Value.Rate;
                        Console.Write(new String(' ', 7));
                    }
                }
                else
                {   // An employee hours were set, checking for overtime hours and setting Gross Wages.
                    Console.Write(new String(' ', 7 - entry.Length));

                    entry = ReadNoReturn.OnEnter();
                    double.TryParse(entry, out double overtime);
                    employee.Value.OverTimeHours = overtime;

                    employee.Value.GetGrossWage();
                }

                // Printing payroll calculations.
                employee.Value.GetSocialSecurity();
                employee.Value.GetMedicare();
                employee.Value.GetFederalWitholding();
                employee.Value.GetStateWithholding();
               
                Console.Write(new String(' ', 9 - entry.Length));
                Console.Write("{0,-9:C2}  ", employee.Value.GrossWage);
                Console.Write("{0,-9:C2}  ", employee.Value.SocialSecurity);
                Console.Write("{0,-9:C2}  ", employee.Value.Medicare);
                Console.Write("{0,-9:C2}  ", employee.Value.FederalWithholding);
                Console.Write("{0,-9:C2}  ", employee.Value.StateWithholding);
                Console.Write("{0,-9:C2}  ", employee.Value.StateTransitTax);
                Console.Write("{0,-9:C2}  ", employee.Value.WorkmansComp);
                Console.Write("{0,-9:C2}  ", employee.Value.NetWage);
                Console.Write("\n\n");
            }
            FileIO.Save(employeeList, "Temp.txt");
            Serializing.Serial();
            WorksheetSummary.Summary(employeeList);
        }
    }
}
