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
        public string Płeć { get; set; }
        public string Orientacja { get; set; }
        public string Wiek { get; set; }
        public string Wzrost { get; set; }
        public string Waga { get; set; }
        public string Biust { get; set; }
        public string Oczy { get; set; }
        public string Wlosy { get; set; }
        public string Wyjazdy { get; set; }
        public string Uslugi { get; set; }
        public string Jezyki { get; set; }
        public string Tatuaze { get; set; }


        public Pani (string Link)
        {
            this.Link = Link;
        }

        public override string ToString()
        {
            return Link + "||" + Płeć + "||" + Orientacja + "||" + Wiek + "||" + Wzrost + "||" + Waga + "||" + Biust + "||" + Oczy + "||" + Wlosy + "||" + Wyjazdy + "||" + Uslugi;
        }
    }
}
