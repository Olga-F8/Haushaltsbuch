using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    public class Statistik
    {
        public static double Gesamtausgaben { get; set; }
        public static double Kategoriesumme { get; set; }

        public static void StatistikAnzeigen()
        {
            Gesamtausgaben = 0;
            Kategoriesumme = 0;

            foreach (Eintrag eintrag in Eintrag.Eintraege)
            {
                if (eintrag.UserId == User.AktuellerUserID)
                {
                    Gesamtausgaben += eintrag.Betrag;
                }
            }
            Console.WriteLine($"Gesamtausgaben: {Gesamtausgaben} Euro.");

            foreach (var kategorie in KategorieClass.Kategorien)
            {
                foreach (Eintrag eintrag in Eintrag.Eintraege)
                {
                    if (eintrag.Kategorie == kategorie.NAME && eintrag.UserId == User.AktuellerUserID)
                    {
                        Kategoriesumme += eintrag.Betrag;
                    }
                }
                Console.WriteLine($"Ausgaben in der Kategorie {kategorie.NAME}: {Kategoriesumme} Euro.");
                Kategoriesumme = 0;
            }

        }

    }
}
