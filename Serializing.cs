using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using static WYSIWYG.Employee;
using System.Runtime.Serialization;

namespace WYSIWYG
{
    class Serializing
    {
        private const string filename = "data.dat";

        public static void Serial(Dictionary<int, TimeSlip> empList)
        {
            // Serialize object.
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("Person.bin", FileMode.Create, FileAccess.Write, FileShare.None);

            foreach (var employeeID in empList.Values)
            {
                formatter.Serialize(stream, employeeID);
            }

            stream.Close();
            Deserial();
        }

        public static void Serial()
        {
            List<string> items = new List<string>
            {
                "Earnings:\n Regular{ totals.RegularWages,46:C2}{ EmployeeTotals.GetYTD(0).RegularWages,20:C2}",
                " Overtime{ totals.OvertimeWages,45:C2}{ EmployeeTotals.GetYTD(0).OvertimeWages,20:C2}",
                "{ underscore,55}{ underscore,20}",
                "\n{ totals.GrossWage,54:C2}{ EmployeeTotals.GetYTD(0).GrossWage,20:C2}\n",

                "Additional expenses:\n FICA expense{ Federal.SocialSecurity(totals.GrossWage) / 2,41:C2}{ EmployeeTotals.GetYTD(0).SocialSecurity,20:C2}",
                " Medicare expense{ Federal.MedicareCalculation(totals.GrossWage) / 2,37:C2}{ EmployeeTotals.GetYTD(0).Medicare,20:C2}",
                " FUTA expense{ totals.FUTA,41:C2}{ EmployeeTotals.GetYTD(0).FUTA,20:C2}",
                " SUTA expense{ totals.SUTA,41:C2}{ EmployeeTotals.GetYTD(0).SUTA,20:C2}",

                "{ underscore,55}{ underscore,20}",

                "   Total additional expenses{_additionalExpenses(totals),26:C2}{ _additionalExpenses(EmployeeTotals.GetYTD(0)),20:C2}\n",
                "Your total payroll expenses{ totals.GrossWage + _additionalExpenses(totals),24:C2}{ EmployeeTotals.GetYTD(0).GrossWage + _additionalExpenses(EmployeeTotals.GetYTD(0)),20:C2}\n",

                "Liabilities - immediate:\n FICA payable{ Federal.SocialSecurity(totals.GrossWage),41:C2}{ Federal.SocialSecurity(EmployeeTotals.GetYTD(0).GrossWage),20:C2}",
                " Medicare payable{Federal.MedicareCalculation(totals.GrossWage),37:C2}{Federal.MedicareCalculation(EmployeeTotals.GetYTD(0).GrossWage),20:C2}",
                " FWT payable{totals.FederalWithholding,42:C2}{EmployeeTotals.GetYTD(0).FederalWithholding,20:C2}",
                " SWT payable{totals.StateWithholding,42:C2}{EmployeeTotals.GetYTD(0).StateWithholding,20:C2}",
                " Cash - payroll{totals.NetWage,39:C2}{EmployeeTotals.GetYTD(0).NetWage,20:C2}",

                "{ underscore,55}{underscore,20}",
                "    Total immediate liabilities{_ImmediateLiiabilities(totals),23:C2}{_ImmediateLiiabilities(EmployeeTotals.GetYTD(0)),20:C2}",

                "Liablities - other:\n W/C - Insurance{State.WorkmansComp(totals.RegularHours + totals.OverTimeHours),38:C2}{State.WorkmansComp(EmployeeTotals.GetYTD(0).RegularHours + EmployeeTotals.GetYTD(0).OverTimeHours),20:C2}",
                " FUTA Liability{totals.FUTA,39:C2}{EmployeeTotals.GetYTD(0).FUTA,20:C2}",
                " SUTA Liability{totals.SUTA,39:C2}{EmployeeTotals.GetYTD(0).SUTA,20:C2}",

                "{underscore,55}{underscore,20}",

                "Total other Liabilities{_OtherLiabilities(totals),31:C2}{_OtherLiabilities(EmployeeTotals.GetYTD(0)),20:C2}\n",
                " Your total payroll liabilities{_ImmediateLiiabilities(totals) + _OtherLiabilities(totals),23:C2}{_ImmediateLiiabilities(EmployeeTotals.GetYTD(0)) + _OtherLiabilities(EmployeeTotals.GetYTD(0)),20:C2}"
            };

            // Serialize object.
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("SummaryText.bin", FileMode.Create, FileAccess.Write, FileShare.None);

            foreach (var item in items)
            {
                formatter.Serialize(stream, item);
            }

            stream.Close();
        }

        static void Deserial()
        {
            //FileStream stream = File.OpenRead(filename);
            //BinaryFormatter format = new BinaryFormatter();

            //TimeSlip objRead = (TimeSlip)format.Deserialize(stream);
            //stream.Close();

            // Deserialize object.
            IFormatter formatter = new BinaryFormatter();

            Stream readStream = new FileStream("Person.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            TimeSlip p2 = (TimeSlip)formatter.Deserialize(readStream);
            readStream.Close();

            // Test equality with new object.
            // Assert.AreEqual(p.Name, p2.Name);
            // Assert.AreEqual(p.Age, p2.Age);

        }
    }
}
