using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using OfficeOpenXml.Table;
using System.Threading;
using System.Linq;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;
using static ThotHunter.Escort;

namespace ThotHunter
{
    class Program
    {
        static void Main(string[] args)
        {
            EdgeOptions options = new EdgeOptions();
            options.AcceptInsecureCertificates = true;
            EdgeDriver driver = new EdgeDriver(options);
            driver.OtworzEscort();
            // driver.UstawFiltryWojewodztwa("Dolnośląskie");
            //driver.UstawFiltryMiasta("Wrocław");
            //driver.KliknijSzukaj();

            driver.WyszukajPoMiescie("Warszawa");

            List<Pani> listaPan = new List<Pani>();
            //List<string> listaLinkow = driver.WczytajDostepneLinki();
            List<string> listaLinkow = driver.WczytajLinki();
            foreach (string link in listaLinkow)
            {
                Console.WriteLine(link);
                Pani pani = new Pani(link);
                driver.Navigate().GoToUrl(link);
                Thread.Sleep(500);
                try
                {
                    driver.WczytajDane(ref pani);
                    Console.WriteLine(pani.ToString());
                    Console.WriteLine();
                    listaPan.Add(pani);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Nie udało się wczytać danych Pani");
                }
            }


            driver.Close();
            createExcel(listaPan, "", "asdad");


        }

        public static byte[] createExcel<T>(IEnumerable<T> list, string author, string title)
        {
            using (ExcelPackage package = new ExcelPackage("C:\\proejtk\\wwa.xlsx"))
            {
                //create the excel file and set some properties
                package.Workbook.Properties.Author = author;
                package.Workbook.Properties.Title = title;
                package.Workbook.Properties.Created = DateTime.Now;

                //create a new sheet
                package.Workbook.Worksheets.Add("Sheet 1");
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.Font.Name = "Calibri";

                //put the data in the sheet, starting from column A, row 1
                ws.Cells["A1"].LoadFromCollection(list, true);

                //set some styling on the header row
                var header = ws.Cells[1, 1, 1, ws.Dimension.End.Column];
                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                header.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));

                //loop the header row to capitalize the values
                for (int col = 1; col <= ws.Dimension.End.Column; col++)
                {
                    var cell = ws.Cells[1, col];
                    cell.Value = cell.Value.ToString().ToUpper();
                }

                //loop the properties in list<t> to apply some data formatting based on data type and check for nested lists
                var listObject = list.First();
                var columns_to_delete = new List<int>();
                for (int i = 0; i < listObject.GetType().GetProperties().Count(); i++)
                {
                    var prop = listObject.GetType().GetProperties()[i];
                    var range = ws.Cells[2, i + 1, ws.Dimension.End.Row, i + 1];

                    //check if the property is a List, if yes add it to columns_to_delete
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        columns_to_delete.Add(i + 1);
                    }

                    //set the date format
                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        range.Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    }

                    //set the decimal format
                    if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                    {
                        range.Style.Numberformat.Format = "0.00";
                    }
                }

                //remove all lists from the sheet, starting with the last column
                foreach (var item in columns_to_delete.OrderByDescending(x => x))
                {
                    ws.DeleteColumn(item);
                }

                //auto fit the column width
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //sometimes the column width is slightly too small (maybe because of font type).
                //So add some extra width just to be sure
                for (int col = 1; col <= ws.Dimension.End.Column; col++)
                {
                    ws.Column(col).Width += 3;
                }

                //send the excel back as byte array
                package.Save();
                return package.GetAsByteArray();

            }
        }
    }
}
