using System;
using System.IO;
using System.Collections.Generic;
using static WYSIWYG.Employee;

namespace WYSIWYG
{
    class FileIO
    {
        static public void Save(Dictionary<int, TimeSlip> iString, string fileName)
        {
            // IEnumerable<decimal> decimalValues = iString.Select(v => Convert.ToDecimal(v));

            // Set a variable to the My Documents path.
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(@fileName))
            {
                foreach (var employeeID in iString)

                    outputFile.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}",   employeeID.Value.GrossWage, 
                                                                                employeeID.Value.SocialSecurity ,
                                                                                employeeID.Value.Medicare ,
                                                                                employeeID.Value.FederalWithholding ,
                                                                                employeeID.Value.StateWithholding ,
                                                                                employeeID.Value.WorkmansComp ,
                                                                                employeeID.Value.NetWage 
                        );                
            }
        }
    }
}
