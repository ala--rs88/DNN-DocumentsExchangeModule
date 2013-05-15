using System;
using System.Globalization;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace IgorKarpov.DocumentsExchangeModule.Components.Entities
{
    public class Schedule
    {
        public int Id
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public DateTime LastModificationDate
        {
            get;
            set;
        }

        public String Mon
        {
            get;
            set;
        }

        public String Tue
        {
            get;
            set;
        }

        public String Wed
        {
            get;
            set;
        }

        public String Thu
        {
            get;
            set;
        }

        public String Fri
        {
            get;
            set;
        }

        public String Sat
        {
            get;
            set;
        }

        public String Sun
        {
            get;
            set;
        }

        public void SaveAs(String fileName)
        {
            using (StreamWriter writer = System.IO.File.CreateText(fileName))
            {
                writer.WriteLine("Mon:");
                writer.Write("    ");
                writer.WriteLine(Mon);

                writer.WriteLine("Tue:");
                writer.Write("    ");
                writer.WriteLine(Tue);

                writer.WriteLine("Wed:");
                writer.Write("    ");
                writer.WriteLine(Wed);

                writer.WriteLine("Thu:");
                writer.Write("    ");
                writer.WriteLine(Thu);

                writer.WriteLine("Fri:");
                writer.Write("    ");
                writer.WriteLine(Fri);

                writer.WriteLine("Sat:");
                writer.Write("    ");
                writer.WriteLine(Sat);

                writer.WriteLine("Sun:");
                writer.Write("    ");
                writer.WriteLine(Sun);

                writer.WriteLine();
                writer.WriteLine("Last modified: {0}", LastModificationDate.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}