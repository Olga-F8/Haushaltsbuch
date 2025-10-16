using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    internal class Kategorie
    {
        public string NAME { get; set; }
        public string BESCHREIBUNG { get; set; }
        public Kategorie(string name, string beschreibung)
        {
            NAME = name;
            BESCHREIBUNG = beschreibung;
        }
    }
}
