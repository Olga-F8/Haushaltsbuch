using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    internal class Eintrag
    {
        public double Betrag { get; set; }
        public string Kategorie { get; set; }
        public DateTime Datum { get; set; }
        public string Typ { get; set; }

        public Eintrag(double betrag, string kategorie, DateTime datum, string typ)
        {
            Betrag = betrag;
            Kategorie = kategorie;
            Datum = datum;
            Typ = typ;
        }

        public static void EintragHinzufuegen()
        {
            Console.Clear();
            Console.WriteLine("***Eintrag hinzufügen***\n");
            Console.Write("Betrag: ");
            var betrag_check = double.TryParse(Console.ReadLine(), out double betrag);
            if (!betrag_check)
            {
                Console.WriteLine("Ungültiger Betrag. Bitte eine Zahl eingeben.");
                return;
            }
            Console.Write("Kategorie: ");
            string kategorie = Console.ReadLine() ?? "";



            Console.Write("Datum (yyyy-MM-dd): ");
            var datum_check = DateTime.TryParse(Console.ReadLine(), out DateTime datum);
            if (!datum_check)
            {
                Console.WriteLine("Ungültiges Datum. Bitte im Format yyyy-MM-dd eingeben.");
                return;
            }

            Console.Write("Typ (Einnahme/Ausgabe): ");
            string typ = Console.ReadLine() ?? "Ausgabe";
            if (typ != "Einnahme" && typ != "Ausgabe")
            {
                Console.WriteLine("Ungültiger Typ. Bitte 'Einnahme' oder 'Ausgabe' eingeben.");
                return;
            }
            Eintrag neuerEintrag = new Eintrag(betrag, kategorie, datum, typ);
            User.Eintraege.Add(neuerEintrag);
            Console.WriteLine("Eintrag erfolgreich hinzugefügt!");
        }

        public static void EintraegeAnzeigen()
        {
            int count = 0;
            Console.Clear();
            Console.WriteLine("***Alle Einträge***\n");
            if (User.Eintraege.Count == 0)
            {
                Console.WriteLine("Keine Einträge vorhanden.");
                return;
            }
            foreach (var eintrag in User.Eintraege)
            {
                count++;
                Console.WriteLine($"{count}+1. Betrag: {eintrag.Betrag}, Kategorie: {eintrag.Kategorie}, Datum: {eintrag.Datum.ToShortDateString()}, Typ: {eintrag.Typ}");
            }
        }

        public static void EintragLoeschen()
        {
            Console.Clear();
            Console.WriteLine("***Eintrag löschen***\n");
            EintraegeAnzeigen();

            Console.Write("Geben Sie die Nummer des Eintrags ein, den Sie löschen möchten: ");
            var nummer_check = int.TryParse(Console.ReadLine(), out int nummer);
            if (!nummer_check)
            {
                Console.WriteLine("Falsche Eingabe!");
                return;
            }
           var eintrag = User.Eintraege[nummer-1];
            if (eintrag != null)
            {
                User.Eintraege.Remove(eintrag);
                Console.WriteLine("Eintrag erfolgreich gelöscht!");
            }
            else
            {
                Console.WriteLine("Kein Eintrag für das angegebene Datum gefunden.");
            }
        }


    }
}
