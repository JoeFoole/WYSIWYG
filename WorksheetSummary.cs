using System;
using System.IO;
using System.Collections.Generic;
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

            Dictionary<string, Tuple<double, double>> dTotals = new Dictionary<string, Tuple<double, double>>()
            {   {"RegularWages", Tuple.Create(totals.RegularWages, EmployeeTotals.GetYTD(0).GrossWage) },
                {"OvertimeWages", Tuple.Create(totals.OvertimeWages, EmployeeTotals.GetYTD(0).OvertimeWages) },
                {"FICAex", Tuple.Create( Federal.SocialSecurity(totals.GrossWage) / 2, EmployeeTotals.GetYTD(0).SocialSecurity) },
                {"FICApay", Tuple.Create( Federal.SocialSecurity(totals.GrossWage), Federal.SocialSecurity(EmployeeTotals.GetYTD(0).GrossWage)) },
                {"Mediex", Tuple.Create( Federal.MedicareCalculation(totals.GrossWage) / 2, EmployeeTotals.GetYTD(0).Medicare) },
                {"Medipay", Tuple.Create(Federal.MedicareCalculation(totals.GrossWage), Federal.MedicareCalculation(EmployeeTotals.GetYTD(0).GrossWage)) },
                {"SUTA", Tuple.Create( totals.SUTA, EmployeeTotals.GetYTD(0).SUTA) },
                {"FWT", Tuple.Create( totals.FederalWithholding,  EmployeeTotals.GetYTD(0).FederalWithholding) },
                {"SWT", Tuple.Create( totals.StateWithholding, EmployeeTotals.GetYTD(0).StateWithholding) },
                {"WCex", Tuple.Create( State.WorkmansComp(totals.RegularHours + totals.OverTimeHours)/2, State.WorkmansComp(EmployeeTotals.GetYTD(0).RegularHours + EmployeeTotals.GetYTD(0).OverTimeHours)/2) },
                {"WCpay", Tuple.Create( State.WorkmansComp(totals.RegularHours + totals.OverTimeHours), State.WorkmansComp(EmployeeTotals.GetYTD(0).RegularHours + EmployeeTotals.GetYTD(0).OverTimeHours)) },
                {"Addex", Tuple.Create( _additionalExpenses(totals), _additionalExpenses(EmployeeTotals.GetYTD(0))) },
                {"Immex", Tuple.Create( _ImmediateLiiabilities(totals), _ImmediateLiiabilities(EmployeeTotals.GetYTD(0))) },
                {"Otherex", Tuple.Create( _OtherLiabilities(totals), _OtherLiabilities(EmployeeTotals.GetYTD(0))) },
                {"TotalEx", Tuple.Create( _additionalExpenses(totals) + totals.RegularWages + totals.OvertimeWages, _additionalExpenses(EmployeeTotals.GetYTD(0)) + EmployeeTotals.GetYTD(0).GrossWage + EmployeeTotals.GetYTD(0).OvertimeWages) },
                {"TotalLiab", Tuple.Create( _OtherLiabilities(totals) + _ImmediateLiiabilities(totals), _OtherLiabilities(EmployeeTotals.GetYTD(0)) + _ImmediateLiiabilities(EmployeeTotals.GetYTD(0))) },
            };

            Output(totals, 0);

            Console.Write("Do you want the worksheet printed? ");
            if (Console.ReadLine() == "y")
            {
                Output(totals, 1);
                PrintingExample.WorksheetPrint();
            }
        }

        private static void Output(Employee.TimeSlip totals, int printer)
        {
            if (printer == 0)
            {
                StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
                sw.AutoFlush = true;
                Console.SetOut(sw);
            }
            else if(printer == 1)
            {
                StreamWriter sw = new StreamWriter(@".\Summary.txt");
                sw.AutoFlush = true;
                Console.SetOut(sw);
            }

            string underscore = new string('-', 12);

            Console.Out.WriteLine($"Earnings:\n Regular{ totals.RegularWages , 46:C2}{ EmployeeTotals.GetYTD(0).RegularWages , 20:C2}");
            Console.Out.WriteLine($" Overtime{ totals.OvertimeWages , 45:C2}{ EmployeeTotals.GetYTD(0).OvertimeWages ,20:C2}" );
            Console.Out.WriteLine($"{ underscore, 55}{ underscore, 20}");
            Console.Out.WriteLine($"\n{ totals.GrossWage, 54:C2}{ EmployeeTotals.GetYTD(0).GrossWage, 20:C2}" );

            Console.Out.WriteLine($"Additional expenses:\n FICA expense{ Federal.SocialSecurity(totals.GrossWage) / 2, 41:C2}{ EmployeeTotals.GetYTD(0).SocialSecurity, 20:C2}");
            Console.Out.WriteLine($" Medicare expense{ Federal.MedicareCalculation(totals.GrossWage) / 2, 37:C2}{ EmployeeTotals.GetYTD(0).Medicare,20:C2}" );
            Console.Out.WriteLine($" FUTA expense{ totals.FUTA, 41:C2}{ EmployeeTotals.GetYTD(0).FUTA, 20:C2}");
            Console.Out.WriteLine($" SUTA expense{ totals.SUTA, 41:C2}{ EmployeeTotals.GetYTD(0).SUTA, 20:C2}");

            Console.Out.WriteLine($"{ underscore, 55}{ underscore, 20}");

            Console.Out.WriteLine($"   Total additional expenses{_additionalExpenses(totals), 26:C2}{ _additionalExpenses(EmployeeTotals.GetYTD(0)), 20:C2}\n");
            Console.Out.WriteLine($"Your total payroll expenses{ totals.GrossWage + _additionalExpenses(totals), 27:C2}{ EmployeeTotals.GetYTD(0).GrossWage + _additionalExpenses(EmployeeTotals.GetYTD(0)),20:C2}\n");

            Console.Out.WriteLine($"Liabilities - immediate:\n FICA payable{ Federal.SocialSecurity(totals.GrossWage),41:C2}{ Federal.SocialSecurity(EmployeeTotals.GetYTD(0).GrossWage), 20:C2}\n ");
            Console.Out.WriteLine($"Medicare payable{Federal.MedicareCalculation(totals.GrossWage),37:C2}{Federal.MedicareCalculation(EmployeeTotals.GetYTD(0).GrossWage),20:C2}");
            Console.Out.WriteLine($" FWT payable{totals.FederalWithholding,42:C2}{EmployeeTotals.GetYTD(0).FederalWithholding,20:C2}");
            Console.Out.WriteLine($" SWT payable{totals.StateWithholding,42:C2}{EmployeeTotals.GetYTD(0).StateWithholding,20:C2}");
            Console.Out.WriteLine($" Cash - payroll{totals.NetWage,39:C2}{EmployeeTotals.GetYTD(0).NetWage,20:C2}");

            Console.Out.WriteLine($"{ underscore,55}{underscore,20}");
            Console.Out.WriteLine($"    Total immediate liabilities{_ImmediateLiiabilities(totals),23:C2}{_ImmediateLiiabilities(EmployeeTotals.GetYTD(0)),20:C2}\n");

            Console.Out.WriteLine($"Liablities - other:\n W/C - Insurance{State.WorkmansComp(totals.RegularHours + totals.OverTimeHours),38:C2}{State.WorkmansComp(EmployeeTotals.GetYTD(0).RegularHours + EmployeeTotals.GetYTD(0).OverTimeHours),20:C2}");
            Console.Out.WriteLine($" FUTA Liability{totals.FUTA,39:C2}{EmployeeTotals.GetYTD(0).FUTA,20:C2}");
            Console.Out.WriteLine($" SUTA Liability{totals.SUTA,39:C2}{EmployeeTotals.GetYTD(0).SUTA,20:C2}");

            Console.Out.WriteLine($"{underscore,55}{underscore,20}");

            Console.Out.WriteLine($"Total other Liabilities{_OtherLiabilities(totals),31:C2}{_OtherLiabilities(EmployeeTotals.GetYTD(0)),20:C2}\n");
            Console.Out.WriteLine($" Your total payroll liabilities{_ImmediateLiiabilities(totals) + _OtherLiabilities(totals),23:C2}{_ImmediateLiiabilities(EmployeeTotals.GetYTD(0)) + _OtherLiabilities(EmployeeTotals.GetYTD(0)),20:C2}");

            if(printer==1)Console.Out.Close();

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

        public static void Temp()
        {
            // Get all files in the current directory.
            string[] files = Directory.GetFiles(".");
            Array.Sort(files);

            // Display the files to the current output source to the console.
            Console.WriteLine("First display of filenames to the console:");
            Array.ForEach(files, s => Console.Out.WriteLine(s));
            Console.Out.WriteLine();

            // Redirect output to a file named Files.txt and write file list.
            StreamWriter sw = new StreamWriter(@".\Files.txt");
            sw.AutoFlush = true;
            Console.SetOut(sw);
            Console.Out.WriteLine("Display filenames to a file:");
            Array.ForEach(files, s => Console.Out.WriteLine(s));
            Console.Out.WriteLine();

            // Close previous output stream and redirect output to standard output.
           
            sw = new StreamWriter(Console.OpenStandardOutput());
            sw.AutoFlush = true;
            Console.SetOut(sw);

            // Display the files to the current output source to the console.
            Console.Out.WriteLine("Second display of filenames to the console:");
            Array.ForEach(files, s => Console.Out.WriteLine(s));
        }


    }
}

