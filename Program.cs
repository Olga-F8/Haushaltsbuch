using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using PdfSharp.Fonts;


// git add .
// git commit -m "blabla"
// git push


namespace Haushaltsbuch
{

    public class Program
    {

        public static void Main(string[] args)
        {
            GlobalFontSettings.FontResolver = new EmbeddedFontResolver();
            Json.JsonLaden(ref User.users, ref KategorieClass.Kategorien, ref Eintrag.Eintraege);
            WillkomenMenue();
        }

        public static void WillkomenMenue()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("================================");
                Console.WriteLine("Willkomen zu Haushaltsbuch");
                Console.WriteLine("================================\n");

                Console.WriteLine("1. Anmeldung");
                Console.WriteLine("2. Registrierung\n");
                Console.Write("Bitte 1 oder 2 eingeben: ");

                var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
                if (eingabe_check)
                {
                    switch (eingabe)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("================================");
                            Console.WriteLine("Anmeldung:");
                            Console.WriteLine("================================\n");

                            if (User.UserLogin() == 0)
                                break;
                            else
                            {
                                Hauptmenue();
                            }
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("================================");
                            Console.WriteLine("Registrierung:");
                            Console.WriteLine("================================\n");
                            User.UserErstellen();
                            break;
                        default:
                            Statistik.RedText("Ungültige Eingabe. Bitte 1 oder 2 eingeben.");
                            break;
                    }
                }
                else
                {
                    Statistik.RedText("Ungültige Eingabe. Bitte eine Zahl eingeben.");
                }
                Console.WriteLine("Drücken Sie eine Taste, um fortzufahren...");
                Console.ReadKey();
            }
        }

        public static void Hauptmenue()
        {
            while (true)
            {
                Console.Clear();
                Statistik.BlauText("Willkomen im Hauptmenü, " + User.users.FirstOrDefault(u => u.ID == User.AktuellerUserID)?.NAME + "!\n");
                Console.WriteLine("================================");
                Console.WriteLine("Hauptmenü");
                Console.WriteLine("================================\n");
                Console.WriteLine("1. Eintrag hinzufügen\n" + "2. Eintrag bearbeiten\n" +
                                  "3. Eintrag löschen\n" + "4. Einträge anzeigen\n" +
                                  "5. Filter einsetzen\n" + "6. Ausgabenlimit einstellen\n" +
                                  "7. Logout\n");
                Console.Write("Bitte eine Zahl eingeben: ");
                var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
                if (eingabe_check)
                {
                    switch (eingabe)
                    {
                        case 1: Eintrag.EintragHinzufuegen();
                            break;
                        case 2: Eintrag.EintragBearbeiten();
                            break;
                        case 3: Eintrag.EintragLoeschen();
                            break;
                        case 4: Eintrag.EintraegeAnzeigen();
                            break;
                        case 5: 
                            Statistik.FilterEinsetzen(Statistik.GefilterteEintraege);
                            break;
                        case 6: Statistik.AusgabenLimitAnzeigen();
                            break;
                        case 7: User.AktuellerUserID = 0;
                            return;
                        default:
                            Statistik.RedText("Ungültige Eingabe. Bitte eine Zahl zwischen 1 und 3 eingeben.");
                            break;
                    }
                }
                else
                    Statistik.RedText("Ungültige Eingabe. Bitte eine Zahl eingeben.");
                Console.WriteLine("\nDrücken Sie eine Taste, um fortzufahren...");
                Console.ReadKey();
            }
        }

        /*
        public static void EintraegeVerwaltungMenue()

        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("***Einträge verwalten***\n");
                Console.WriteLine("1. Eintrag hinzufügen");
                Console.WriteLine("2. Eintrag anzeigen");
                Console.WriteLine("3. Eintrag löschen");
                Console.WriteLine("4. Zurück\n");
                Console.Write("Bitte eine Zahl eingeben: ");

                var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
                if (eingabe_check)
                {
                    switch (eingabe)
                    {
                        case 1:
                            Eintrag.EintragHinzufuegen();
                            break;
                        case 2:
                            Eintrag.EintraegeAnzeigen();
                            break;
                        case 3:
                            Eintrag.EintragLoeschen();
                            break;
                        case 4:
                            return;
                        default:
                            Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl zwischen 1 und 3 eingeben.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl eingeben.");
                }
                Console.WriteLine("Drücken Sie eine Taste, um fortzufahren...");
                Console.ReadKey();
            }
        }

        public static void StatistikFilterMenue()

        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("***Statistik, Filter, Ausgabenlimit***\n");
                Console.WriteLine("1. Gesamtausgaben anzeigen");
                Console.WriteLine("2. Ausgaben nach Kategorie anzeigen");
                Console.WriteLine("3. Ausgabenlimit einstellen");
                                Console.WriteLine("4. Ausgabenliste als .pdf spreichern");
Console.WriteLine("5. Zurück\n");
                Console.Write("Bitte eine Zahl eingeben: ");

                var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
                if (eingabe_check)
                {
                    switch (eingabe)
                    {
                        case 1:
                            Statistik.GesamtausgabenAnzeigen();
                            break;
                        case 2:
                            Statistik.KategorieAusgabenAnzeigen();
                            break;
                        case 3:
                            Statistik.AusgabenLimitAnzeigen();
                            break;
                        case 4:
                            Statistik.ExportToPdf(Eintrag.Eintraege, "Haushalt.pdf"); ;
                            break; 
                        case 5:
                            return;
                        default:
                            Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl zwischen 1 und 3 eingeben.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl eingeben.");
                }
                Console.WriteLine("\nDrücken Sie eine Taste, um fortzufahren...");
                Console.ReadKey();
            }
        }
        */

    }
}
