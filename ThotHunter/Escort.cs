using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.DevTools.V104.CSS;

namespace ThotHunter
{
    public static class Escort
    {

        /// <summary>
        /// Metoda odpowiedzialna za otwieranie strony głównej escort.pl
        /// </summary>
        /// <param name="driver"></param>
        public static void OtworzEscort(this EdgeDriver driver)
        {
            driver.Navigate().GoToUrl("https://roksa.top/");
            try
            {
                driver.FindElement(By.XPath("//*[@id=\"forAdultsModal\"]/div/div/div/div/div[3]/div[1]/button")).Click();
                
            }
            catch{
            }
            //driver.FindElement(By.XPath("//*[@id='searchForm']/div[1]/div/div/div[2]/div[1]/div/button/span[1]")).Click();
        }

        /// <summary>
        /// Metoda odpowiedzialna za ustawienie fltra "województwo" na podany argument
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="wojewodztwo"></param>
        public static void UstawFiltryWojewodztwa(this EdgeDriver driver, string wojewodztwo)
        {
            bool CzyZnaleziono = false;
            driver.FindElement(By.XPath("//*[@id='searchForm']/div[1]/div/div/div[2]/div[1]/div/button")).Click();
            IWebElement dropdownMenu = driver.FindElement(By.XPath("//*[@id='searchForm']/div[1]/div/div/div[2]/div[1]/div/div"));
            foreach (IWebElement li in dropdownMenu.FindElements(By.TagName("li")))
            {
                if (li.Text == wojewodztwo)
                {
                    li.Click();
                    CzyZnaleziono = true;
                    Thread.Sleep(500);
                    break;
                }
            }
            if (CzyZnaleziono == false)
            {
                throw new BladFiltra($"Nie udało się znaleźć województwa {wojewodztwo} wśród dostępnych filtrów");
            }
        }


        /// <summary>
        /// Metoda odpowiedzialna za ustawianie filtra "miasto" na podany argument
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="miasto"></param>
        public static void UstawFiltryMiasta(this EdgeDriver driver, string miasto)
        {
            bool CzyZnaleziono = false;
            driver.FindElement(By.XPath("//*[@id='searchForm']/div[1]/div/div/div[2]/div[2]/div/button")).Click();
            IWebElement dropdownMenu = driver.FindElement(By.XPath("//*[@id='searchForm']/div[1]/div/div/div[2]/div[2]/div/div/ul"));
            foreach (IWebElement li in dropdownMenu.FindElements(By.TagName("li")))
            {
                Console.WriteLine(li.Text);
                if (li.Text.Trim() == miasto)
                {

                    li.Click();
                    CzyZnaleziono = true;
                    Thread.Sleep(500);
                    break;
                }
            }
            if (CzyZnaleziono == false)
            {
                throw new BladFiltra($"Nie udało się znaleźć miasta {miasto} wśród dostępnych filtrów");
            }
        }

        /// <summary>
        /// Metoda odpowiedzialna za ustawienie filtrów na wybrane miasto
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="miasto"></param>
        public static void WyszukajPoMiescie(this EdgeDriver driver, string miasto)
        {
            driver.FindElement(By.XPath("//*[@id=\"form_control_input_city\"]")).SendKeys(miasto);
            driver.FindElement(By.XPath("/html/body/nav/div[3]/div/form/div/div[5]/button")).Click();
            Thread.Sleep(500);

        }

        /// <summary>
        /// Metoda klikanie przycisku szukaj
        /// </summary>
        /// <param name="driver"></param>
        public static void KliknijSzukaj(this EdgeDriver driver)
        {
            driver.FindElement(By.XPath("//*[@id='searchForm']/div[1]/div/div/div[2]/div[4]/button")).Click();
        }


        /// <summary>
        /// Metoda zwraca listę linków wszystkich wyświetlonych ogłoszeń
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="przerwijNa"></param>
        /// <returns></returns>
        public static List<string> WczytajDostepneLinki(this EdgeDriver driver, int przerwijNa = 1000000)
        {
            // Ile stron jest dostępnych
            var element = driver.FindElement(By.XPath("/html/body/div[1]/section[4]/div/div[4]/div/ul"));

            List<int> ileStron = new List<int>();
            foreach (var li in element.FindElements(By.TagName("a")))
            {
                try
                {
                    Console.WriteLine(li.GetAttribute("title"));
                }
                catch (Exception e){
                    Console.WriteLine(e);
                    Console.ReadLine();
                    continue;
                }
            }

            List<string> linki = new List<string>();
            IWebElement content = driver.FindElement(By.XPath("/html/body/div[1]/section[4]/div/div[3]"));
            var lista = content.FindElements(By.ClassName("item-col"));
            int b = 0;
            foreach (IWebElement kafelek in lista)
            {
                linki.Add(kafelek.FindElements(By.TagName("a"))[1].GetAttribute("href"));
                b++;
                if (b == przerwijNa)
                {
                    break;
                }
            }
            return linki;
        }

        public static List<string> WczytajLinki(this EdgeDriver driver, int przerwijNa = 1000000)
        {
            List<string> linki = new List<string>();
            var kafelki=driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div/div[2]/div")).FindElements(By.ClassName("single-announcement"));
            foreach (IWebElement kafelek in kafelki)
            {
                string link = kafelek.FindElement(By.TagName("div")).FindElement(By.TagName("a")).GetAttribute("href");
                linki.Add(link);
            }
            return linki;
        }
        
        public static void WczytajDane(this EdgeDriver driver, ref Pani pani)
        {
            var infoList = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div[2]/div/ul")).FindElements(By.TagName("li"));

            foreach (var info in infoList)
            {
                string colName = info.FindElement(By.TagName("span")).FindElement(By.TagName("span")).Text;
                switch (colName)
                {
                    case "Płeć":
                        {
                            pani.Płeć = info.FindElement(By.ClassName("fw-600")).Text;
                            break;

                        }
                    case "Wiek":
                        {
                            pani.Wiek = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    case "Biust":
                        {
                            pani.Biust = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    case "Orientacja":
                        {
                            pani.Orientacja = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    case "Wyjazdy":
                        {
                            pani.Wyjazdy = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    default:
                        Console.WriteLine("brak");
                        break;
                }
            }

            infoList = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div[3]/div/ul")).FindElements(By.TagName("li"));

            foreach (var info in infoList)
            {
                string colName = "cokolwiek";
                try
                {
                    colName = info.FindElement(By.TagName("span")).FindElement(By.TagName("span")).Text;
                }
                catch { 
                }
                switch (colName)
                {
                    case "Wzrost":
                        {
                            pani.Wzrost = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    case "Włosy":
                        {
                            pani.Wlosy = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    case "Oczy":
                        {
                            pani.Oczy = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    case "Waga":
                        {
                            pani.Waga = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    case "Języki":
                        {
                            pani.Jezyki = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    case "Tatuaże":
                        {
                            pani.Tatuaze = info.FindElement(By.ClassName("fw-600")).Text;
                            break;
                        }
                    default:
                        Console.WriteLine($"brak - {colName}");
                        break;
                }
            }

            var listaUslug = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[2]")).FindElements(By.ClassName("badge"));
            foreach (var usluga in listaUslug)
            {
                pani.Uslugi += usluga.Text + "||";
            }
        }
    }

    public class BladFiltra : Exception
    {
        public BladFiltra() { }

        public BladFiltra(string message)
            : base(message) { }

        public BladFiltra(string message, Exception inner)
            : base(message, inner) { }
    }
}
