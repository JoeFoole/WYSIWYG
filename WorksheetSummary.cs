using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static WYSIWYG.Employee;

namespace WYSIWYG
{
    class WorksheetSummary
    {
        public static void Summary(Dictionary<int, Employee.TimeSlip> workSheet)
        {
            Employee.TimeSlip totals = new Employee.TimeSlip();

            foreach (var results in workSheet)
            {
                totals.RegularWages += results.Value.RegularWages;      //Salaries are not in totals.
                totals.OvertimeWages += results.Value.OvertimeWages;
                totals.GrossWage += results.Value.GrossWage;
                totals.RegularHours += results.Value.RegularHours;
                totals.OverTimeHours += results.Value.OverTimeHours;
                totals.SocialSecurity += results.Value.SocialSecurity;
                totals.Medicare += results.Value.Medicare;
                totals.FederalWithholding += results.Value.FederalWithholding;
                totals.StateWithholding += results.Value.StateWithholding;
                totals.FUTA += Federal.FUTACalculation(results.Key, results.Value.GrossWage);
                totals.SUTA += State.Unemployment(results.Value.GrossWage);
            }

            EmployeeTotals.Add(totals);

            Console.Write("Earnings:\n Regular{0,46:C2}{1,20:C2}\n Overtime{2,45:C2}{3,20:C2}\n",totals.RegularWages ,EmployeeTotals.GetYTD(0).RegularWages,totals.OvertimeWages,EmployeeTotals.GetYTD(0).OvertimeWages);
            Console.Write("{0,55}{1,20}",new string('-', 12), new string('-', 12));
            Console.Write("\n{0,54:C2}{1,20:C2}\n\n", totals.GrossWage, EmployeeTotals.GetYTD(0).GrossWage);

            Console.Write("Additional expenses:\n FICA expense{0,41:C2}{1,20:C2}\n Medicare expense{2,37:C2}{3,20:C2}\n FUTA expense{4,41:C2}{5,20:C2}\n SUTA expense{6,41:C2}{7,20:C2}\n",
                            Federal.SocialSecurity(totals.GrossWage) / 2, EmployeeTotals.GetYTD(0).SocialSecurity,
                            Federal.MedicareCalculation(totals.GrossWage)/2, EmployeeTotals.GetYTD(0).Medicare,
                            totals.FUTA, EmployeeTotals.GetYTD(0).FUTA,
                            totals.SUTA, EmployeeTotals.GetYTD(0).SUTA);

            Console.Write("{0,55}{1,20}\n", new string('-', 12), new string('-', 12)); 

            Console.Write("   Total additional expenses{0,26:C2}{1,20:C2}\n\n   Your total payroll expenses{2,24:C2}{3,20:C2}\n\n", 
                            _additionalExpenses(totals), _additionalExpenses(EmployeeTotals.GetYTD(0)),
                            totals.GrossWage + _additionalExpenses(totals),  EmployeeTotals.GetYTD(0).GrossWage + _additionalExpenses(EmployeeTotals.GetYTD(0)));

            Console.Write("Liabilities - immediate:\n FICA payable{0,41:C2}{1,20:C2}\n Medicare payable{2,37:C2}{3,20:C2}\n FWT payable{4,42:C2}{5,20:C2}\n SWT payable{6,42:C2}{7,20:C2}\n Cash - payroll{8,39:C2}{9,20:C2}\n",
                            Federal.SocialSecurity(totals.GrossWage), Federal.SocialSecurity(EmployeeTotals.GetYTD(0).GrossWage),
                            Federal.MedicareCalculation(totals.GrossWage), Federal.MedicareCalculation(EmployeeTotals.GetYTD(0).GrossWage),
                            totals.FederalWithholding, EmployeeTotals.GetYTD(0).FederalWithholding,
                            totals.StateWithholding, EmployeeTotals.GetYTD(0).StateWithholding,
                            totals.NetWage, EmployeeTotals.GetYTD(0).NetWage);

            Console.Write("{0,55}{1,20}\n", new string('-', 12), new string('-', 12));
            Console.Write("    Total immediate liabilities{0,23:C2}{1,20:C2}\n\n",_ImmediateLiiabilities(totals), _ImmediateLiiabilities(EmployeeTotals.GetYTD(0)));

            Console.Write("Liablities - other:\n W/C - Insurance{0,38:C2}{1,20:C2}\n FUTA Liability{2,39:C2}{3,20:C2}\n SUTA Liability{4,39:C2}{5,20:C2}\n",
                            State.WorkmansComp(totals.RegularHours + totals.OverTimeHours), State.WorkmansComp(EmployeeTotals.GetYTD(0).RegularHours + EmployeeTotals.GetYTD(0).OverTimeHours),
                            totals.FUTA, EmployeeTotals.GetYTD(0).FUTA,
                            totals.SUTA, EmployeeTotals.GetYTD(0).SUTA);

            Console.Write("{0,55}{1,20}\n", new string('-', 12), new string('-', 12));

            Console.Write("Total other Liabilities{0,31:C2}{1,20:C2}\n\n Your total payroll liabilities{2,23:C2}{3,20:C2}",
                            _OtherLiabilities(totals), _OtherLiabilities(EmployeeTotals.GetYTD(0)),
                            _ImmediateLiiabilities(totals) + _OtherLiabilities(totals), _ImmediateLiiabilities(EmployeeTotals.GetYTD(0)) + _OtherLiabilities(EmployeeTotals.GetYTD(0)));

            Console.ReadLine();
        }

        private static double _additionalExpenses(Employee.TimeSlip x)
        {
            return x.SocialSecurity +
                    x.Medicare +
                    x.FUTA +                                                                //EmployeeTotals.GetYTD(0).FUTA
                    (State.WorkmansComp(x.RegularHours + x.OverTimeHours) / 2) +
                    State.Unemployment(x.GrossWage);

        }

        private static double _ImmediateLiiabilities(Employee.TimeSlip x)
        {
            return Federal.SocialSecurity(x.GrossWage) +
                    Federal.MedicareCalculation(x.GrossWage) + 
                    x.FederalWithholding +
                    x.StateWithholding +
                    x.NetWage;
        }

        private static double _OtherLiabilities(Employee.TimeSlip x)
        {
            return State.WorkmansComp(x.RegularHours + x.OverTimeHours) +
                   x.FUTA +
                   State.Unemployment(x.GrossWage);
        }
    }
}
