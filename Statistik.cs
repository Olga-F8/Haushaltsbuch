using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace Haushaltsbuch
{
    public class Statistik
    {
        public static double Gesamtausgaben { get; set; }
        public static double MonatlicheGesamtausgaben { get; set; }

        public static double Kategoriesumme { get; set; }
        public static bool ausgabenlimit_check = false;
        public static string kategorie_filter = "";
        public static string monat_filter = "";
        public static string jahr_filter = "";

        public static List<Eintrag> GefilterteEintraege = Eintrag.UserListeladen();
        public static double GesamtausgabenBerechen()
        {
            Gesamtausgaben = 0;

            foreach (Eintrag eintrag in Eintrag.Eintraege)
            {
                if (eintrag.UserId == User.AktuellerUserID)
                {
                    Gesamtausgaben += eintrag.Betrag;
                }
            }
            Gesamtausgaben = Math.Round(Gesamtausgaben, 2);

            return Gesamtausgaben;
        }
        public static double MonatlicheGesamtausgabenBerechen()
        {
            MonatlicheGesamtausgaben = 0;

            foreach (Eintrag eintrag in Eintrag.Eintraege)
            {
                if (eintrag.UserId == User.AktuellerUserID && eintrag.Datum.Month == DateTime.Now.Month && eintrag.Datum.Year == DateTime.Now.Year)
                {
                    MonatlicheGesamtausgaben += eintrag.Betrag;
                }

            }
            MonatlicheGesamtausgaben = Math.Round(MonatlicheGesamtausgaben, 2);
            return MonatlicheGesamtausgaben;
        }

        public static List<Eintrag> FilterEinsetzen(List<Eintrag> gefilterteListe)
        {
            bool logout = false;
            do 
            {
                Console.Clear();
                Console.WriteLine("Filteroptionen:");
                Console.WriteLine("1. Einträge von heute anzeigen");
                Console.WriteLine("2. Nach Monat filtern");
                Console.WriteLine("3. Nach Jahr filtern");
                Console.WriteLine("4. Nach Kategorie filtern");
                Console.WriteLine("5. Filterzurücksetzen");
                Console.WriteLine("6. Als .pdf Datei speichern");
                Console.WriteLine("7. Die Liste ausdrucken");
                Console.WriteLine("8. Zurück");

                if(monat_filter != "" || jahr_filter != "" || kategorie_filter != "")
                BlauText($"\nFilter ist aktiv: {monat_filter} {jahr_filter} {kategorie_filter}");
                GefilterteEintraegeAnzeigen();
                Console.Write("\nWählen Sie eine Option (1-8): ");
                var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
                if (!eingabe_check || eingabe < 1 || eingabe > 8)
                {
                    RedText("Ungültige Eingabe. Bitte eine Zahl zwischen 1 und 8 eingeben.");
                    return gefilterteListe;
                }
                switch (eingabe)
                {
                    case 1:
                        FilternNachTag();
                        FilterEinsetzen(gefilterteListe);
                        break;
                    case 2:
                        Console.Write("Geben Sie den Monat (1-12) ein: ");
                        var monat_check = int.TryParse(Console.ReadLine(), out int monat);
                        if (monat_check && monat > 0 && monat < 13)
                        {
                            FilternNachMonat(monat);
                            gefilterteListe = GefilterteEintraege;
                            Console.Clear();
                            FilterEinsetzen(gefilterteListe);
                        }
                        else
                            RedText("Falsche Eingabe!");
                        break;
                    case 3:
                        Console.Write("Geben Sie das Jahr (z.B. 2025) ein: ");
                        var jahr_only_check = int.TryParse(Console.ReadLine(), out int jahr_only);
                        if (!jahr_only_check)
                        {
                            RedText("Falsche Eingabe!");
                        }
                        FilternNachJahr(jahr_only);
                        gefilterteListe = GefilterteEintraege;
                        Console.Clear();
                        FilterEinsetzen(gefilterteListe);

                        break;
                    case 4:
                        Console.Write("Geben Sie die Kategorie ein: ");
                        string kategorie = Console.ReadLine();
                        if (KategorieClass.Kategorien.All(kat => kat.NAME != kategorie))
                        {
                           RedText("Kategorie existiert nicht!");
                        }
                        FilternNachKategorie(kategorie);
                        
                        gefilterteListe = GefilterteEintraege;
                        Console.Clear();
                        FilterEinsetzen(gefilterteListe);
                        break;
                    case 5:
                        BlauText("Filter zurückgesetzt.");
                        gefilterteListe = FilterZuruecksetzen();
                        FilterEinsetzen(gefilterteListe);

                        return gefilterteListe;
                    case 6:
                        var aktuellerUser = User.users.FirstOrDefault(u => u.ID == User.AktuellerUserID);
                        string dateiname;
                        if (monat_filter == "" && jahr_filter == "" && kategorie_filter == "")
                            dateiname = $"Alle_Einträge_{aktuellerUser.NAME}_{DateTime.Now.ToShortDateString()}.pdf";
                        else
                            dateiname = $"Einträge_mit_Filter_{monat_filter}_{jahr_filter}_{kategorie_filter}_{aktuellerUser.NAME}_{DateTime.Now.ToShortDateString()}.pdf";
                        if (aktuellerUser != null && !File.Exists(dateiname))
                            ExportToPdf(gefilterteListe, dateiname);
                        else Console.WriteLine("Pdf bereits exestiert.");
                            break;
                    case 7:
                        var aktuellerUser1 = User.users.FirstOrDefault(u => u.ID == User.AktuellerUserID);
                        string dateiname1;
                        if (monat_filter == "" && jahr_filter == "" && kategorie_filter == "")
                            dateiname1 = $"Alle_Einträge_{aktuellerUser1.NAME}_{DateTime.Now.ToShortDateString()}.pdf";
                        else
                            dateiname1 = $"Einträge_mit_Filter_{monat_filter}_{jahr_filter}_{kategorie_filter}_{aktuellerUser1.NAME}_{DateTime.Now.ToShortDateString()}.pdf";
                        if (aktuellerUser1 != null && File.Exists(dateiname1))
                            DruckenEintraege(dateiname1);
                        else
                        {
                            ExportToPdf(gefilterteListe, dateiname1);
                            DruckenEintraege(dateiname1);
                        }
                        return gefilterteListe;
                        case 8:
                        logout = true;
                        break;
                    default:
                       RedText("Ungültige Eingabe. Bitte eine Zahl zwischen 1 und 8 eingeben.");
                       break;
                }
                return gefilterteListe;
            }
            while (!logout) ;
        }
        public static void GefilterteEintraegeAnzeigen()
        {
            int count = 0;
            Console.WriteLine("\nBetrag / Kategorie / Datum");
            foreach (Eintrag eintrag in GefilterteEintraege)
            {
                count++;
                Console.WriteLine($"{count} / {eintrag.Betrag} / {eintrag.Kategorie} / {eintrag.Datum.ToShortDateString()}");
            }
        }

        public static List<Eintrag> FilterZuruecksetzen()
        {
            GefilterteEintraege.Clear();
            GefilterteEintraege = Eintrag.UserListeladen();
            monat_filter = "";
            jahr_filter = "";
            kategorie_filter = "";
            return GefilterteEintraege;
        }
        public static void FilternNachTag()
        {
            Gesamtausgaben = 0;
            GefilterteEintraege.RemoveAll(e => e.Datum.Day != DateTime.Now.Day);
            foreach (Eintrag eintrag in GefilterteEintraege)
            {
                Gesamtausgaben += eintrag.Betrag;
            }
            GefilterteEintraegeAnzeigen();
            Gesamtausgaben = Math.Round(Gesamtausgaben, 2);
            Console.WriteLine($"\nSie haben heute {Gesamtausgaben} Euro ausgegeben.");
            Console.ReadKey();
        }
        public static List<Eintrag> FilternNachMonat(int monat)
        {
            Gesamtausgaben = 0;
            GefilterteEintraege.RemoveAll(e => e.Datum.Month != monat);
            monat_filter = new DateTime(2025, monat, 1).ToString("MMMM", System.Globalization.CultureInfo.GetCultureInfo("de-DE"));

            foreach (Eintrag eintrag in GefilterteEintraege)
            {
                Gesamtausgaben += eintrag.Betrag;
            }
            GefilterteEintraegeAnzeigen();
            Gesamtausgaben = Math.Round(Gesamtausgaben, 2);
            Console.WriteLine($"\nSie haben {Gesamtausgaben} Euro ausgegeben.");
            Console.ReadKey();
            return GefilterteEintraege;
        }
        public static List<Eintrag> FilternNachJahr(int jahr)
        {
            Gesamtausgaben = 0;

            GefilterteEintraege.RemoveAll(e => e.Datum.Year != jahr);
            jahr_filter = jahr.ToString();
            foreach (Eintrag eintrag in GefilterteEintraege)
            {
                Gesamtausgaben += eintrag.Betrag;
            }
            GefilterteEintraegeAnzeigen();
            Gesamtausgaben = Math.Round(Gesamtausgaben, 2);
            Console.WriteLine($"\nSie haben {Gesamtausgaben} Euro ausgegeben.");
            Console.ReadKey();

            return GefilterteEintraege;
        }

        public static List<Eintrag> FilternNachKategorie(string kategorie)
        {
            Gesamtausgaben = 0;
            GefilterteEintraege.RemoveAll(e => e.Kategorie != kategorie);
            kategorie_filter = kategorie;
            foreach (Eintrag eintrag in GefilterteEintraege)
            {
                Gesamtausgaben += eintrag.Betrag;
            }
            GefilterteEintraegeAnzeigen();
            Gesamtausgaben = Math.Round(Gesamtausgaben, 2);
            Console.WriteLine($"\nSie haben {Gesamtausgaben} Euro ausgegeben.");
            Console.ReadKey();

            return GefilterteEintraege;
        }
        public static void SummeNachKategorie()
        {
            Kategoriesumme = 0;
            int count = 0;
            Console.WriteLine("\nKategorie / Betrag im Euro");

            foreach (var kat in KategorieClass.Kategorien)
            {
                foreach (Eintrag eintrag in Eintrag.Eintraege)
                {
                    if (eintrag.Kategorie == kat.NAME && eintrag.Datum.Month== DateTime.Now.Month && eintrag.Datum.Year== DateTime.Now.Year && eintrag.UserId == User.AktuellerUserID)
                    {
                        Kategoriesumme += eintrag.Betrag;
                    }
                }
                Kategoriesumme = Math.Round(Kategoriesumme, 2);
                if (Kategoriesumme != 0)
                    Console.WriteLine($"{kat.NAME} / {Kategoriesumme}");
                Kategoriesumme = 0;
            }

        }


        public static void AusgabenLimitAnzeigen()
        {
            MonatlicheGesamtausgabenBerechen();
            Console.WriteLine();
            foreach (User user in User.users)
            {
                if (user.ID == User.AktuellerUserID)
                    if (user.AUSGABENLIMIT == 0)
                    {
                        Console.WriteLine("Es ist kein Ausgabenlimit festgelegt.");
                    }
                    else
                    {
                        Console.WriteLine($"Monatliches Ausgabenlimit: {user.AUSGABENLIMIT}");
                        if (MonatlicheGesamtausgaben > user.AUSGABENLIMIT)
                        {

                            Console.Write($"Ausgaben im {DateTime.Now.ToString("MMMM")} {DateTime.Now.ToString("yyyy")}:  ");
                            RedText(MonatlicheGesamtausgaben.ToString());
                        }
                        else
                        {
                            Console.Write($"Ausgaben im {DateTime.Now.ToString("MMMM")} {DateTime.Now.ToString("yyyy")}:   ");
                            BlauText(MonatlicheGesamtausgaben.ToString());
                        }

                    }
            }
            SummeNachKategorie();
            Console.WriteLine();
            Console.WriteLine("Wollen Sie Ausgabenlimit ändern (j/n)?\n");
            ConsoleKeyInfo eingabe = Console.ReadKey();
            switch (eingabe.KeyChar)
            {
                case 'j':
                    Console.WriteLine();
                    AusgabenLimitAendern();
                    break;
                case 'n':
                    return;
                default:
                    RedText("\nUngültige Eingabe. Bitte 'j' oder 'n' eingeben.");
                    break;
            }
            MonatlicheGesamtausgaben = 0;


        }
        public static void AusgabenLimitAendern()
        {
            Console.WriteLine("\nGeben Sie das monatlichen Ausgabenlimit in Euro ein:");
            ausgabenlimit_check = double.TryParse(Console.ReadLine(), out double ausgabenLimit);
            if (!ausgabenlimit_check)
            {
                Statistik.RedText("Ungültige Eingabe. Bitte eine Zahl eingeben.");
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
                        SoundPlayer player_s = new SoundPlayer("success.wav");
                        player_s.Play();
                        Statistik.BlauText("Ausgabenlimit Gespeichert!");
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
                    if (MonatlicheGesamtausgaben > user.AUSGABENLIMIT)
                    {
                        string warnung = $"Warnung: Ihre Ausgaben {MonatlicheGesamtausgaben} haben das festgelegte monatlichen Limit von {user.AUSGABENLIMIT} überschritten!";
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



        public static void ExportToPdf(List<Eintrag> eintraege, string dateiPfad)
        {
            PdfDocument dokument = new PdfDocument();
            dokument.Info.Title = "Haushaltsbuch Export";
            PdfPage seite = dokument.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(seite);
            XFont font = new XFont("NotoSans", 12);

            double y = 40;
            gfx.DrawString("Haushaltsbuch – Export", new XFont("NotoSans", 16), XBrushes.Black, new XPoint(40, y));
            y += 30;

            gfx.DrawString("Datum | Kategorie | Betrag | Typ", font, XBrushes.Black, new XPoint(40, y));
            y += 20;

            foreach (var e in eintraege)
            {
                gfx.DrawString($"{e.Datum:dd.MM.yyyy} | {e.Kategorie} | {e.Betrag} € | {e.Typ}",
                               font, XBrushes.Black, new XPoint(40, y));
                y += 15;
            }

            dokument.Save(dateiPfad);
            Console.WriteLine($"PDF erfolgreich gespeichert: {Path.GetFullPath(dateiPfad)}");

        }
        public static void DruckenEintraege(string dateiPfad)
        {
           
                Console.WriteLine("PDF wird gedruckt...");
                var args = $"-print-to-default \"{dateiPfad}\"";
                Process.Start(new ProcessStartInfo(dateiPfad, args) { CreateNoWindow = true, UseShellExecute = true });
        }

        //PrintDocument pd = new PrintDocument();
        //pd.PrintPage += PrintPageHandler;
        //pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
        //pd.Print();

        //private static void PrintPageHandler(object sender, PrintPageEventArgs e)
        //{
        //    string text = "Haushaltsbuch – Übersicht\n\n" +
        //                  "01.10.2025 | Lebensmittel | -45,20 €\n" +
        //                  "02.10.2025 | Gehalt       | +2500,00 €\n";

        //    Font font = new Font("Arial", 12);
        //    e.Graphics.DrawString(text, font, Brushes.Black, 50, 50);
        //}
    }
}

