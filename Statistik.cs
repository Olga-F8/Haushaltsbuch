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
        public static bool ausgabenlimit_check = false;


        public static double GesamtausgabenBerechen()
        {
            Gesamtausgaben = 0;

            //ÄNDERN:gesamtausgaben für aktuellen monat berechnen
            foreach (Eintrag eintrag in Eintrag.Eintraege)
            {
                if (eintrag.UserId == User.AktuellerUserID)
                {
                    Gesamtausgaben += eintrag.Betrag;
                }
            }
            return Gesamtausgaben;
        }
        public static void GesamtausgabenAnzeigen()
        {
            foreach (User user in User.users)
            {
                if (user.ID == User.AktuellerUserID)
                {

                    Console.WriteLine($"Monatliche Ausgabenlimit: {user.AUSGABENLIMIT} Euro");
                    GesamtausgabenBerechen();
                    if (user.AUSGABENLIMIT == 0)
                        Console.Write($"Gesamtausgaben: {Gesamtausgaben} Euro.");
                    else if (Gesamtausgaben > user.AUSGABENLIMIT)
                    {
                        Console.Write($"Gesamtausgaben: ");
                        RedText($"{Gesamtausgaben} Euro.");
                    }
                    else
                    {
                        Console.Write($"Gesamtausgaben: ");
                        BlauText($"{Gesamtausgaben} Euro.");
                    }
                }

            }

        }



        public static void KategorieAusgabenAnzeigen()
        {
            Kategoriesumme = 0;

            foreach (var kategorie in KategorieClass.Kategorien)
            {
                foreach (Eintrag eintrag in Eintrag.Eintraege)
                {
                    if (eintrag.Kategorie == kategorie.NAME && eintrag.UserId == User.AktuellerUserID)
                    {
                        Kategoriesumme += eintrag.Betrag;
                    }
                }
                if(Kategoriesumme!=0)
                Console.WriteLine($"Ausgaben in der Kategorie {kategorie.NAME}: {Kategoriesumme} Euro.");
                Kategoriesumme = 0;
            }

        }

        public static void AusgabenLimitAnzeigen()
        {

            foreach (User user in User.users)
            {
                if (user.ID == User.AktuellerUserID)
                    if (user.AUSGABENLIMIT == 0)
                    {
                        Console.WriteLine("Es ist kein Ausgabenlimit festgelegt.");
                        
                    }
                    else
                        Console.WriteLine($"Aktuelle Ausgabenlimit: {user.AUSGABENLIMIT}");
            }

            Console.WriteLine("Wollen Sie Ausgabenlimit ändern (j/n)?\n");
            ConsoleKeyInfo eingabe = Console.ReadKey();
            switch (eingabe.KeyChar)
            {
                case 'j':
                    AusgabenLimitAendern();
                    break;
                case 'n':
                    return;
                default:
                    Console.WriteLine("\nUngültige Eingabe. Bitte 'j' oder 'n' eingeben.");
                    break;
            }


        }
        public static void AusgabenLimitAendern()
        {

            Console.WriteLine("\nGeben Sie monatlichen gesammten Ausgabenlimit in Euro ein:");
            ausgabenlimit_check = double.TryParse(Console.ReadLine(), out double ausgabenLimit);
            if (!ausgabenlimit_check)
            {
                Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl eingeben.");
                return;
            }
            else
            {
                foreach (User user in User.users)
                {
                    if (user.ID == User.AktuellerUserID)
                    {
                        user.AUSGABENLIMIT = ausgabenLimit;
                        Json.JsonSpeichern(User.users, KategorieClass.Kategorien, Eintrag.Eintraege);
                        Console.WriteLine("Ausgabenlimit Gespeichert!");
                    }
                }
            }
        }

        public static void AusgabenLimitPruefen()
        {
            foreach (User user in User.users)
            {
                if (user.ID == User.AktuellerUserID)
                {
                    if (Gesamtausgaben > user.AUSGABENLIMIT)
                    {
                        string warnung = $"Warnung: Ihre Ausgaben {Gesamtausgaben} haben das festgelegte Limit von {user.AUSGABENLIMIT} überschritten!";
                        RedText(warnung);
                    }

                }
            }
        }

        public static void RedText(string text)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void BlauText(string text)
        {

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
