using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support;
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
            driver.Navigate().GoToUrl("https://pl.escort.club/");
            
            try
            {
                driver.FindElement(By.XPath("//*[@id=\"adult-only-warning\"]/div/div/div/div[1]/div[2]/a[1]")).Click();
                
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
            Console.ReadLine();

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

        


        /// <summary>
        /// Metoda zwraca element klasy Pani utworzony na podstawie danych pod linkiem
        /// </summary>
        /// <param name="driver"></param>
        public static void WczytajDanePani(this EdgeDriver driver, ref Pani pani)
        {
            Console.WriteLine();
            string lokalizacja = driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div[3]/div[1]/div[2]/div[1]/div[1]")).FindElement(By.ClassName("sub-label")).Text;
            string[] split = lokalizacja.Split(",");
            string wojewodztwo = split[0];
            string miasto = split[1];
            string dzielnica = "";
            for (int i = 2; i < split.Length; i++)
            {
                if (i == split.Length - 1)
                {
                    dzielnica += split[i];
                }
                else
                {
                    dzielnica += split[i] + ",";
                }
            }
            pani.Nazwa = driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div[3]/div[1]/div[2]/div[1]/h1")).Text;
            pani.Dzielnica = dzielnica;
            pani.Wojewodztwo = wojewodztwo;
            pani.Miasto = miasto;
            Console.WriteLine(lokalizacja);
            IWebElement info = driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div[3]/div[2]/div/div[3]")).FindElement(By.ClassName("stats-box"));
            foreach (IWebElement e in info.FindElements(By.ClassName("stat-elem")))
            {
                string label = e.FindElement(By.ClassName("sub-label")).Text;
                label = label.Replace(':',' ').Trim();
                string opis = "brak";
                try
                {
                    opis = e.FindElement(By.ClassName("sub-desc")).Text;
                    if (opis.Length < 1)
                    {
                        opis = "brak";
                    }
                }
               
                catch (NoSuchElementException) { }

                Console.WriteLine(opis);
                switch (label)
                {
                    case "Płeć":
                        {
                            pani.Płeć = opis;
                            break;
                        }
                    case "Orientacja":
                        {
                            pani.Orientacja = opis;
                            break;
                        }
                    case "Wiek":
                        {
                            pani.Wiek = int.Parse(opis.Replace("l",""));
                            break;
                        }
                    case "Wzrost":
                        {
                            pani.Wzrost = int.Parse(opis.Replace("cm",""));
                            break;
                        }
                    case "Waga":
                        {
                            pani.Waga = int.Parse(opis.Replace("kg",""));
                            break;
                        }
                    case "Biust":
                        {
                            pani.Biust = opis;
                            break;
                        }
                    case "Oczy":
                        {
                            pani.Oczy = opis;
                            break;
                        }
                    case "Włosy":
                        {
                            pani.Wlosy = opis;
                            break;
                        }
                    case "Wyjazdy":
                        {
                            pani.Wyjazdy = opis;
                            break;
                        }
                    case "Etniczność":
                        {
                            pani.Etniczność = opis;
                            break;
                        }
                    case "Narodowość":
                        {
                            pani.Narodowosz = opis;
                            break;
                        }
                    case "Znak zodiaku":
                        {
                            pani.ZnakZodiaku = opis;
                            break;
                        }

                }
            }

            info = driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div[3]/div[2]/div/div[4]")).FindElement(By.ClassName("stats-box"));
            Dictionary<string, string> cennik = new Dictionary<string, string>();
            foreach (IWebElement e in info.FindElements(By.ClassName("stat-elem")))
            {
                string label = e.FindElement(By.ClassName("sub-label")).Text;
                label = label.Replace(':', ' ').Trim();
                string opis = "brak";
                try
                {
                    opis = e.FindElement(By.ClassName("sub-desc")).Text.Replace("zł", "");
                    cennik.Add(label, opis);
                }
                catch (NoSuchElementException) { }   

                foreach (KeyValuePair<string,string> kv in cennik)
                {
                    string key = kv.Key;
                    switch (key)
                    {
                        case "15 min":
                            {
                                pani.CenaZaKwadrans = int.Parse(kv.Value);
                                break;
                            }
                        case "0,5 godz":
                            {
                                pani.CenaZaPolGodziny = int.Parse(kv.Value);
                                break;
                            }
                        case "Noc":
                            {
                                pani.CenaZaNoc = int.Parse(kv.Value);
                                break;
                            }
                        case "1 godz":
                            {
                                pani.CenaZaGodzine = int.Parse(kv.Value);
                                break;
                            }
                        default:
                            {
                                pani.Cennik += $"[{key} {kv.Value}]";
                                break;
                            }
                    }
                }
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
