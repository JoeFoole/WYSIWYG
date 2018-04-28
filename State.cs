using System;
using System.Collections.Generic;

namespace WYSIWYG
{
    public static class State
    {
        public static double WorkmansCompRate { get; private set; } = .028;

        private static double SingleStandardDeduction { get; set; } = 2215;
        private static double MarriedStandardDeduction { get; set; } = 4435;
        private static double AllowanceAmount { get; set; } = 201;


        public static double Unemployment(double gross)
        {
            return gross * Employer.StateUnemploymentRate;
        }

        public static double WorkmansComp(double hours)
        {
            return hours * WorkmansCompRate;
        }

        public static double Witholding(double gross, double federalWH, char status, int allowances)
        {
            double[] rate = new double[] {0, 0.05, 0.07, 0.09 };
            List<double> table = new List<double>();
            List<double> baseWH = new List<double>();

            if (federalWH > 6650) federalWH = 6650;

            federalWH = WitholdingPhaseout(gross, federalWH, status, allowances);
            double baseWage = BaseWage(gross, federalWH, status, allowances);
            allowances = GetWitholdingTable(gross, status, allowances, out table, out baseWH);

            if (baseWage <= 0) return 0;

            if(gross < 50000)
            {
                for (int x = 0; x < table.Count; x++)
                {
                    if (baseWage < table[x])
                    {
                        return Math.Round((baseWH[x] + (baseWage - table[x - 1]) * rate[x]) - allowances * AllowanceAmount, 2);
                    }
                }
            }
            else
            {
                var threshold = 125000;

                if(status == 'S' && allowances < 3)
                {
                    if(baseWage < threshold)
                    {
                        return 540 + ((baseWage - 8700) * .09) - (AllowanceAmount * allowances);
                    }
                    else
                    {
                        return 11007 + ((baseWage - threshold) * .099) - (AllowanceAmount * allowances);
                    }
                }
                else
                {
                    if (baseWage < threshold*2)
                    {
                        return 1080 + ((baseWage - 17400) * .09) - (AllowanceAmount * allowances);
                    }
                    else
                    {
                        return 22014 + ((baseWage - threshold*2) * .099) - (AllowanceAmount * allowances);
                    }
                }
            }            

            return -1;
        }

        private static double BaseWage(double gross, double federalWH, char status, int allowances)
        {
            if (status == 'M' || (status == 'S' && allowances >= 3))
            {
                if ((gross - federalWH - MarriedStandardDeduction) < 0)
                {
                    return 0;
                }

                return gross - federalWH - MarriedStandardDeduction;
            }
            else
            {
                if ((gross - federalWH - SingleStandardDeduction) < 0)
                {
                    return 0;
                }

                return gross - federalWH - SingleStandardDeduction;
            }            
        }

        private static int GetWitholdingTable(double gross, char status, int allowances, out List<double> table, out List<double> baseWH)
        {  
            if (gross <= 50000)
            {
                Under50kTaxTables(status, allowances, out table, out baseWH);
                return allowances;
            }
            else
            {
                Over50kTaxTables(status, allowances, out table, out baseWH);
                
                if ((status == 'S' && gross > 100000) ||(status == 'M' && gross > 200000))
                    { return 0; }
                else
                    { return allowances; }
            }
        }

        private static void Under50kTaxTables(char status, int allowances, out List<double> table, out List<double> baseWH)
        {
            // Set the baseWH and table Lists to the the correct married or single tables with taxable income $50,000 and below.

            baseWH = new List<double>();
            table = new List<double>();

            if (status == 'S' && allowances < 3)
            {
                double[] singleTable = { 0, 3450, 8700, 50000 };
                double[] singleBaseWH = { 0, 201, 373.50, 741 };

                table.AddRange(singleTable);
                baseWH.AddRange(singleBaseWH);
            }
            else if ((status == 'S' && allowances >= 3) || (status == 'M'))
            {
                double[] marriedTable = { 0, 6900, 17400, 50000 };
                double[] marriedBaseWH = { 0, 201, 546, 1281 };

                table.AddRange(marriedTable);
                baseWH.AddRange(marriedBaseWH);
            }
            else
            {
                double[] singleTable = { 0, 0, 0, 0 };
                double[] singleBaseWH = { 0, 0, 0, 0 };

                table.AddRange(singleTable);
                baseWH.AddRange(singleBaseWH);
            }
        }

        private static void Over50kTaxTables(char status, int allowances, out List<double> table, out List<double> baseWH)
        {
            // Set the baseWH and table Lists to the the correct married or single tables with taxable income above $50,000.
            
                baseWH = new List<double>();
                table = new List<double>();
                
                if (status == 'S' && allowances < 3)
                {
                    double[] singleTable = {0, 41135, 125000 };
                    double[] singleBaseWH = {0, 540, 11007 };
                
                    table.AddRange(singleTable);
                    baseWH.AddRange(singleBaseWH);
                }
                else if ((status == 'S' && allowances > 3) || (status == 'M'))
                {
                    double[] marriedTable = {0, 38915, 250000 };
                    double[] marriedBaseWH = { 0, 1080, 22014 };

                    table.AddRange(marriedTable);
                    baseWH.AddRange(marriedBaseWH);
                }
                else
                {
                    double[] singleTable = { 0, 0, 0, 0 };
                    double[] singleBaseWH = { 0, 0, 0, 0 };

                    table.AddRange(singleTable);
                    baseWH.AddRange(singleBaseWH);
                }

        }

        private static double WitholdingPhaseout(double gross, double federalWH, char status, int allowances)
        {
            if ((gross <= 125000 && status == 'S') || (gross <= 250000 && status == 'S' && allowances >= 3) || (gross <= 250000 && status == 'M'))
            {
                return federalWH;
            }

            return 0;
        }
    }
}
