using System;
using System.Collections.Generic;

namespace WYSIWYG
{
    public static class Federal
    {       
        public static double SocialSecurity(double gross)
        {
            // TODO: This is on the first 127,200.
            return Math.Round( gross * FederalRates.SocialSecurity, 2);  
        }

        public static double MedicareCalculation(double gross)
        {
            return Math.Round( gross * FederalRates.Medicare,2);
        }

        public static double FUTACalculation(int key, double gross)
        {
            //TODO This needs to be on the first $7,000.
            if (Employee.EmployeeTotals.GetYTD(key).GrossWage > FederalRates.FUTAlimit) gross = 0;
            if (Employee.EmployeeTotals.GetYTD(key).GrossWage + gross > FederalRates.FUTAlimit) gross = Employee.EmployeeTotals.GetYTD(key).GrossWage + gross - FederalRates.FUTAlimit;
            return Math.Round(gross * FederalRates.Unemployment,2);
        }       

        public static double Withholding(double gross, int personalExemption, char status)
        {
            TaxTables(status, out double[] taxRate, out List<double> baseWH, out List<double> table);

            gross -= personalExemption * FederalRates.WithholdingAllowance;

            if (gross < table[0]) return 0;

            for (int x = 1; x < 7; x++)
            {
                if (gross < table[x])
                {
                    return Math.Round(baseWH[x] + ((gross - table[x - 1]) * taxRate[x]), 2);
                }

            }

            return Math.Round(baseWH[7] + ((gross - table[6]) * taxRate[7]), 2);
        }

        private static void TaxTables(char status, out double[] taxRate, out List<double> baseWH, out List<double> table)
        {
            taxRate = new double[] { 0, 0.1, 0.12, 0.22, 0.24, 0.32, 0.35, 0.37 };
            baseWH = new List<double>();
            table = new List<double>();
            if (status == 'S')
            {
                double[] singleTable = { 3700, 13225, 42400, 86200, 161200, 203700, 503700 };
                double[] singleBaseWH = { 0, 0, 952.50, 4453.50, 14089.50, 32089.50, 45689.50, 150689.50 };

                table.AddRange(singleTable);
                baseWH.AddRange(singleBaseWH);
            }
            else
            {
                double[] marriedTable = { 11550, 30600, 88950, 176550, 326550, 411550, 611550 };
                double[] marriedBaseWH = { 0, 0, 1905, 8907, 28179, 64179, 91379, 161379 };

                table.AddRange(marriedTable);
                baseWH.AddRange(marriedBaseWH);
            }
        }
    }
    public static class  FederalRates
        {
            public static double SocialSecurity { get; } = 0.124;
            public static double Medicare { get; } = 0.029;
            public static double Unemployment { get; } = 0.006;
            public static double WithholdingAllowance { get; } = 4150;
            public static double FUTAlimit { get; } = 7000;
        }
}
