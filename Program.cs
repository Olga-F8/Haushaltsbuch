using System.Security.Cryptography.X509Certificates;

namespace Haushaltsbuch
{      

    public class Program
    {              

       public static void Main(string[] args)
        {
            Json.JsonLaden(ref User.users, ref KategorieClass.Kategorien, ref Eintrag.Eintraege);
            WillkomenMenue();

        }

        public static void WillkomenMenue()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("***Willkomen zu Haushaltsbuch***\n");
                Console.WriteLine("1. Anmeldung");
                Console.WriteLine("2. Registrierung\n");
                Console.Write("Bitte 1 oder 2 eingeben: ");

                var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
                if (eingabe_check)
                {
                    switch (eingabe)
                    {
                        case 1:
                            if (User.UserLogin() == 0)
                                break;
                            else
                                Hauptmenue();                            
                            break;
                        case 2:
                            User.UserErstellen();
                            break;
                        default:
                            Console.WriteLine("Ungültige Eingabe. Bitte 1 oder 2 eingeben.");
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
        public static void Hauptmenue()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("***Hauptmenü***\n");
                Console.WriteLine("1. Einträge verwalten: hinzufügen, bearbeiten, löschen");
                Console.WriteLine("2. Statistik, Filter");
                Console.WriteLine("3. Logout\n");
                Console.Write("Bitte eine Zahl eingeben: ");
                var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
                if (eingabe_check)
                {
                    switch (eingabe)
                    {
                        case 1:
                            EintraegeVerwaltungMenue();
                            break;
                        case 2:
                            StatistikFilterMenue();
                            break;
                        case 3:
                            return;
                            //Console.WriteLine("Bis zum nächten mal!");
                            //Environment.Exit(0);
                            break;
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
                Console.WriteLine("***Statistik, Filter***\n");
                Console.WriteLine("1. Statistik anzeigen");
                Console.WriteLine("2. Filter einsetzen");
                Console.WriteLine("3. Zurück\n");
                Console.Write("Bitte eine Zahl eingeben: ");

                var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
                if (eingabe_check)
                {
                    switch (eingabe)
                    {
                        case 1:
                            Statistik.StatistikAnzeigen();
                            break;
                        case 2:
                            //Filter
                            break;
                        case 3:
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

       
    }
}
