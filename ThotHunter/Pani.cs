using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThotHunter
{
    /// <summary>
    /// Klasa odpowiedzialna za przechowywyanie 
    /// </summary>
    public class Pani
    {
        public string Link { get; set; }
        public string Nazwa { get; set; }
        public string Wojewodztwo { get; set; }
        public string Miasto { get; set; }
        public string Dzielnica { get; set; }
        public string Płeć { get; set; }
        public string Orientacja { get; set; }
        public int Wiek { get; set; }
        public int Wzrost { get; set; }
        public int Waga { get; set; }
        public string Biust { get; set; }
        public string Oczy { get; set; }
        public string Wlosy { get; set; }
        public string Wyjazdy { get; set; }
        public string Etniczność { get; set; }
        public string Narodowosz { get; set; }
        public string ZnakZodiaku { get; set; }
        public int CenaZaKwadrans { get; set; }
        public int CenaZaPolGodziny { get; set; }
        public int CenaZaGodzine { get; set; }
        public int CenaZaNoc { get; set; }
        public string Cennik { get; set; }


        public Pani (string Link)
        {
            this.Link = Link;
        }
    }
}
