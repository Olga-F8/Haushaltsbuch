using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    public class KategorieClass
    {
        public string NAME { get; set; }
        public string BESCHREIBUNG { get; set; }

        public static List<KategorieClass> Kategorien = new List<KategorieClass>();
        public KategorieClass(string name)
        {
            NAME = name;
        }

        public static void doppelteKategorienEntfernen()
        {
            Kategorien = Kategorien.Distinct().ToList();
        }
    }
}
