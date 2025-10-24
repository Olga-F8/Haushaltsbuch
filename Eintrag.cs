using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows;

namespace Haushaltsbuch
{
    public class Eintrag
    {
        public int UserId { get; set; }

        public double Betrag { get; set; }
        public string Kategorie { get; set; }
        public DateTime Datum { get; set; }
        public string Typ { get; set; }
        public static List<Eintrag> Eintraege = new List<Eintrag>();
        public static List<Eintrag> UserEintragListe = new List<Eintrag>();
        public static int count = 0;


        public Eintrag(int userid, double betrag, string kategorie, DateTime datum, string typ)
        {
            UserId = userid;
            Betrag = betrag;
            Kategorie = kategorie;
            Datum = datum;
            Typ = typ;
        }

        public static void EintragHinzufuegen()
        {
            SoundPlayer player_e = new SoundPlayer("error.wav");

            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("Eintrag hinzufügen");
            Console.WriteLine("================================");

            Console.Write("Betrag: ");
            var betrag_check = double.TryParse(Console.ReadLine(), out double betrag);
            if (!betrag_check)
            {
                player_e.Play();
                Console.WriteLine("Ungültiger Betrag. Bitte eine Zahl eingeben.");
                return;
            }
            Console.Write("Kategorie: ");
            string kategorie = Console.ReadLine() ?? "";



            Console.Write("Datum (yyyy-MM-dd): ");
            var datum_check = DateTime.TryParse(Console.ReadLine(), out DateTime datum);
            if (!datum_check)
            {
                player_e.Play();
                Console.WriteLine("Ungültiges Datum. Bitte im Format yyyy-MM-dd eingeben.");
                return;
            }

            Console.Write("Typ (Einnahme/Ausgabe): ");
            string typ = Console.ReadLine() ?? "Ausgabe";
            if (typ != "Einnahme" && typ != "Ausgabe")
            {
                player_e.Play();
                Console.WriteLine("Ungültiger Typ. Bitte 'Einnahme' oder 'Ausgabe' eingeben.");
                return;
            }

            Eintrag neuerEintrag = new Eintrag(User.AktuellerUserID, betrag, kategorie, datum, typ);
            Eintraege.Add(neuerEintrag);

            int count = 0;
            foreach (var kat in KategorieClass.Kategorien)
            {
                if (kat.NAME != kategorie)
                {
                    count++;
                }

            }
            Console.WriteLine(count);
            Console.WriteLine(KategorieClass.Kategorien.Count);

            if (count == KategorieClass.Kategorien.Count)
            {
                KategorieClass neueKategorie = new KategorieClass(kategorie);
                KategorieClass.Kategorien.Add(neueKategorie);

            }
            Json.JsonSpeichern(User.users, KategorieClass.Kategorien, Eintraege);

            SoundPlayer player_s = new SoundPlayer("success.wav");
            player_s.Play();
            Console.WriteLine("Eintrag erfolgreich hinzugefügt!\n");
            Statistik.MonatlicheGesamtausgabenBerechen();
            Statistik.AusgabenLimitPruefen();

        }

        public static void EintragBearbeiten()
        {
            SoundPlayer player_e = new SoundPlayer("error.wav");

            string auswahl_bearbeiten = "";
            int count1 = 0;
            count1 = EintraegeAnzeigen();
            Console.WriteLine();
            Console.Write("Geben Sie die Nummer des Eintrags ein, den Sie bearbeiten möchten: ");
            var nummer_check = int.TryParse(Console.ReadLine(), out int nummer);
            if (nummer <= count1)
            {
                if (nummer_check)
                {
                    Console.WriteLine("Was möchsten Sie ändern:");
                    Console.WriteLine("1. Betrag\n2. Kategorie\n3. Datum\n4. Typ");
                    var auswahl_check = int.TryParse(Console.ReadLine(), out int auswahl);
                    if (auswahl_check)
                    {
                        switch (auswahl)
                        {
                            case 1:
                                auswahl_bearbeiten = "Betrag";
                                EintragAendern(nummer, auswahl_bearbeiten);
                                break;
                            case 2:
                                auswahl_bearbeiten = "Kategorie";
                                EintragAendern(nummer, auswahl_bearbeiten);
                                break;
                            case 3:
                                auswahl_bearbeiten = "Datum";
                                EintragAendern(nummer, auswahl_bearbeiten);
                                break;
                            case 4:
                                auswahl_bearbeiten = "Typ";
                                EintragAendern(nummer, auswahl_bearbeiten);
                                break;
                            default:
                                player_e.Play();
                                Console.WriteLine("Falsche Eingabe!");
                                return;

                        }
                    }
                    else
                    {
                        player_e.Play();
                        Console.WriteLine("Falsche Eingabe!");
                        return;
                    }
                }
                else
                {
                    player_e.Play();
                    Console.WriteLine("Falsche Eingabe!");
                    return;
                }
            }
            else
            {
                player_e.Play();
                Console.WriteLine("Falsche Eingabe!");
                return;
            }
        }
        public static void EintragAendern(int nummer, string auswahl_bearbeiten)
        {
            int count1 = 0;
            SoundPlayer player_e = new SoundPlayer("error.wav");
            foreach (var eintrag in Eintraege)
            {
                if (eintrag.UserId == User.AktuellerUserID)
                {
                    count1++;
                    if (count1 == nummer)
                    {
                        if (eintrag != null)
                        {
                            if (auswahl_bearbeiten == "Betrag")
                            {
                                Console.Write("Geben Sie den neuen Betrag ein: ");
                                var betrag_check = double.TryParse(Console.ReadLine(), out double neuer_betrag);
                                if (!betrag_check)
                                {
                                    player_e.Play();
                                    Console.WriteLine("Ungültiger Betrag. Bitte eine Zahl eingeben.");
                                    return;
                                }
                                eintrag.Betrag = neuer_betrag;
                            }
                            else if (auswahl_bearbeiten == "Kategorie")
                            {
                                Console.Write("Geben Sie die neue Kategorie ein: ");
                                string neue_kategorie = Console.ReadLine() ?? "";
                                eintrag.Kategorie = neue_kategorie;
                            }
                            else if (auswahl_bearbeiten == "Datum")
                            {
                                Console.Write("Geben Sie das neue Datum ein (yyyy-MM-dd): ");
                                var datum_check = DateTime.TryParse(Console.ReadLine(), out DateTime neues_datum);
                                if (!datum_check)
                                {
                                    player_e.Play();
                                    Console.WriteLine("Ungültiges Datum. Bitte im Format yyyy-MM-dd eingeben.");
                                    return;
                                }
                                eintrag.Datum = neues_datum;
                            }
                            else if (auswahl_bearbeiten == "Typ")
                            {
                                Console.Write("Geben Sie den neuen Typ ein (Einnahme/Ausgabe): ");
                                string neuer_typ = Console.ReadLine() ?? "Ausgabe";
                                if (neuer_typ != "Einnahme" && neuer_typ != "Ausgabe")
                                {
                                    player_e.Play();
                                    Console.WriteLine("Ungültiger Typ. Bitte 'Einnahme' oder 'Ausgabe' eingeben.");
                                    return;
                                }
                                eintrag.Typ = neuer_typ;
                            }
                            Json.JsonSpeichern(User.users, KategorieClass.Kategorien, Eintrag.Eintraege);
                            Console.WriteLine($"{nummer}. Betrag: {eintrag.Betrag}, Kategorie: {eintrag.Kategorie}, Datum: {eintrag.Datum.ToShortDateString()}, Typ: {eintrag.Typ}");
                            SoundPlayer player_s = new SoundPlayer("success.wav");
                            player_s.Play();
                            Console.WriteLine("Eintrag erfolgreich geändert und gespeichert!");
                            break;
                        }
                        else
                        {
                            player_e.Play();
                            Console.WriteLine("Kein Eintrag für die angegebenen Nummer gefunden.");
                        }
                    }
                }
            }
        }

        public static List<Eintrag> UserListeladen()
        {
            UserEintragListe.Clear(); //von ki empfohlen
            int count = 0;
            
            foreach (Eintrag eintrag in Eintraege)
            {
                if (eintrag.UserId == User.AktuellerUserID)
                {
                    count++;
                    UserEintragListe.Add(eintrag);
                   // Console.WriteLine($"{count}. Betrag: {eintrag.Betrag}, Kategorie: {eintrag.Kategorie}, Datum: {eintrag.Datum.ToShortDateString()}, Typ: {eintrag.Typ}");
                }
            }
            
            //foreach (var eintrag in UserEintragListe)
            //{
            //    Console.WriteLine($"{eintrag.Betrag}, {eintrag.Kategorie}, {eintrag.Datum.ToShortDateString()}, {eintrag.Typ}");
            //}
            return UserEintragListe;
        }
        public static int EintraegeAnzeigen()
        {
            UserEintragListe.Clear(); // von ki empfohlen
            count = 0;
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("Alle Einträge");
            Console.WriteLine("================================");

            if (Eintraege.Count == 0)
            {
                Console.WriteLine("Keine Einträge vorhanden.");
                return 0;
            }
            foreach (var eintrag in Eintraege)
            {
                if (eintrag.UserId == User.AktuellerUserID)
                {
                    count++;
                    UserEintragListe.Add(eintrag);
                    Console.WriteLine($"{count}. Betrag: {eintrag.Betrag}, Kategorie: {eintrag.Kategorie}, Datum: {eintrag.Datum.ToShortDateString()}, Typ: {eintrag.Typ}");
                }
            }
            if (count == 0)
            {
                Console.WriteLine("Keine Einträge vorhanden.");
            }
            return count;
        }

        //prüfen ob richtige eintrag gelöscht wird
        public static void EintragLoeschen()
        {
            int count1 = 0;
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("Eintrag löschen");
            Console.WriteLine("================================");

            EintraegeAnzeigen();

            Console.Write("Geben Sie die Nummer des Eintrags ein, den Sie löschen möchten: ");
            var nummer_check = int.TryParse(Console.ReadLine(), out int nummer);
            if (!nummer_check)
            {
                SoundPlayer player_e = new SoundPlayer("error.wav");
                player_e.Play();
                Console.WriteLine("Falsche Eingabe!");
                return;
            }

            foreach (var eintrag in Eintraege)
            {
                if (eintrag.UserId == User.AktuellerUserID)
                {
                    count1++;
                    if (count1 == nummer)
                    {
                        if (eintrag != null)
                        {
                            Eintrag.Eintraege.Remove(eintrag);
                            Json.JsonSpeichern(User.users, KategorieClass.Kategorien, Eintrag.Eintraege);
                            SoundPlayer player_s = new SoundPlayer("success.wav");
                            player_s.Play();
                            Console.WriteLine("Eintrag erfolgreich gelöscht!");
                            break;
                        }
                        else
                        {
                            SoundPlayer player_e = new SoundPlayer("error.wav");
                            player_e.Play();
                            Console.WriteLine("Kein Eintrag für die angegebenen Nummer gefunden.");
                        }
                    }
                }
            }
            //var eintrag = Eintrag.Eintraege[nummer - 1];
            //Console.WriteLine(eintrag);

        }


    }
}
