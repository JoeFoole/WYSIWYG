using System.Collections.Generic;
using System;

namespace WYSIWYG
{
    class Employee
    {
        [Serializable]
        public class EmployeeInfo
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public char FilingStatus { get; set; }
            public double Rate { get; set; }
            public int PersonalExemption { get; set; }
            public bool Hourly { get; set; }
        }

        public static class EmployeeTotals
        {
            private static Dictionary<int, TimeSlip> yTD = new Dictionary<int, TimeSlip>();
            private static Employee.TimeSlip totals = new Employee.TimeSlip();

            static EmployeeTotals()
            {
                yTD.Add( 1, new TimeSlip { RegularWages = 1600.00, OvertimeWages = 0.00, GrossWage = 1600.00, RegularHours = 0.00, OverTimeHours = 0.00, SocialSecurity = 99.20, Medicare = 23.2, FederalWithholding = 80.88, StateWithholding = 91.68 });
                yTD.Add( 2, new TimeSlip{ RegularWages = 1000.00, OvertimeWages = 0.00, GrossWage = 1000.00, RegularHours = 0.00, OverTimeHours = 0.00, SocialSecurity = 62.00, Medicare = 14.50, FederalWithholding = 39.62, StateWithholding = 55.82 } );
                yTD.Add(7, new TimeSlip { RegularWages = 904.00, OvertimeWages = 0.00, GrossWage = 904.00, RegularHours = 80.00, OverTimeHours = 0.00, SocialSecurity = 56.05, Medicare = 13.11, FederalWithholding = 0.00, StateWithholding = 16.39 });
                yTD.Add( 28, new TimeSlip{ RegularWages = 892.70, OvertimeWages = 0.00, GrossWage = 892.70, RegularHours = 79.00, OverTimeHours = 0.00, SocialSecurity = 55.35, Medicare = 12.94, FederalWithholding = 63.57, StateWithholding = 63.11 });
                yTD.Add( 88, new TimeSlip{ RegularWages = 904.00, OvertimeWages = 169.50, GrossWage = 1073.50, RegularHours = 80.00, OverTimeHours = 10.00, SocialSecurity = 66.56, Medicare = 15.57, FederalWithholding = 0.00, StateWithholding = 16.19 });
                yTD.Add( 222, new TimeSlip{ RegularWages = 0.00, OvertimeWages = 0.00, GrossWage = 0.00, RegularHours = 0.00, OverTimeHours = 0.00, SocialSecurity = 0.00, Medicare = 0.00, FederalWithholding = 0.00, StateWithholding = 0.00 });
                yTD.Add( 224, new TimeSlip{ RegularWages = 0.00, OvertimeWages = 0.00, GrossWage = 0.00, RegularHours = 0.00, OverTimeHours = 0.00, SocialSecurity = 0.00, Medicare = 0.00, FederalWithholding = 0.00, StateWithholding = 0.00 });
                yTD.Add( 225, new TimeSlip{ RegularWages = 0.00, OvertimeWages = 0.00, GrossWage = 0.00, RegularHours = 0.00, OverTimeHours = 0.00, SocialSecurity = 0.00, Medicare = 0.00, FederalWithholding = 0.00, StateWithholding = 0.00 });
                yTD.Add( 226, new TimeSlip{ RegularWages = 0.00, OvertimeWages = 0.00, GrossWage = 0.00, RegularHours = 0.00, OverTimeHours = 0.00, SocialSecurity = 0.00, Medicare = 0.00, FederalWithholding = 0.00, StateWithholding = 0.00 });

                foreach (var results in yTD)
                {
                    totals.RegularWages += results.Value.RegularWages;
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
            }

            public static TimeSlip GetYTD(int key)
            {
                if (yTD.ContainsKey(key))
                {
                    return yTD[key];
                }               

                return totals;
            }

            public static void Add(TimeSlip item)
            {
                totals.RegularWages += item.RegularWages;
                totals.OvertimeWages += item.OvertimeWages;
                totals.GrossWage += item.GrossWage;
                totals.RegularHours += item.RegularHours;
                totals.OverTimeHours += item.OverTimeHours;
                totals.SocialSecurity += item.SocialSecurity;
                totals.Medicare += item.Medicare;
                totals.FederalWithholding += item.FederalWithholding;
                totals.StateWithholding += item.StateWithholding;
                //totals.FUTA += item.FUTA;
                //totals.SUTA += item.SUTA;
            }
        }            

        [Serializable]
        public class TimeSlip : EmployeeInfo
        {
            public double RegularHours { get; set; }
            public double OverTimeHours { get; set; }
            public double RegularWages { get; set; }
            public double OvertimeWages { get; set; }
            public double GrossWage { get; set; }
            public double NetWage => GrossWage - SocialSecurity - Medicare - FederalWithholding - StateWithholding - WorkmansComp;
            public double SocialSecurity { get; set; }
            public double Medicare { get; set; }
            public double WorkmansComp => State.WorkmansComp(RegularHours + OverTimeHours) / 2;
            public double FederalWithholding { get; set; }
            public double StateWithholding { get; set; }
            public double StateTransitTax => GrossWage * 0.001;
            public double SUTA { get; set; }
            public double FUTA { get; set; }

            public void GetGrossWage()
            {
                RegularWages = RegularHours * Rate;
                OvertimeWages = OverTimeHours * Rate * 1.5;
                GrossWage = RegularWages + OvertimeWages;
            }

            public void GetSocialSecurity()
            {
                SocialSecurity = Federal.SocialSecurity(GrossWage) / 2;
            }

            public void GetMedicare()
            {
                Medicare = Federal.MedicareCalculation(GrossWage) / 2;
            }

            public void GetFederalWitholding()
            {
                FederalWithholding =  Federal.Withholding(GrossWage * Employer.PayPeriod, PersonalExemption, FilingStatus) / Employer.PayPeriod;
            }

            public void GetStateWithholding()
            {
                StateWithholding = State.Witholding(GrossWage * Employer.PayPeriod, FederalWithholding, FilingStatus, PersonalExemption) / Employer.PayPeriod;
            }
        }
    }
}
