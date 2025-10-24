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
                if (eintrag.UserId == User.AktuellerUserID && eintrag.Datum.Month == DateTime.Now.Month)
                {
                    MonatlicheGesamtausgaben += eintrag.Betrag;
                }

            }
            MonatlicheGesamtausgaben = Math.Round(MonatlicheGesamtausgaben, 2);
            return MonatlicheGesamtausgaben;
        }

        public static List<Eintrag> FilterEisetzen(List<Eintrag> gefilterteListe)
        {
            Console.Clear();
            Console.WriteLine("Filteroptionen:");
            Console.WriteLine("1. Nach Monat filtern");
            Console.WriteLine("2. Nach Jahr filtern");
            Console.WriteLine("3. Nach Kategorie filtern");
            Console.WriteLine("4. Filterzurücksetzen");
            Console.WriteLine("5. Als .pdf Datei speichern");
            Console.WriteLine("6. Die Liste ausdrucken");


            GefilterteEintraegeAnzeigen();
            Console.Write("\nWählen Sie eine Option (1-3): ");
            var eingabe_check = int.TryParse(Console.ReadLine(), out int eingabe);
            if (!eingabe_check || eingabe < 1 || eingabe > 6)
            {
                Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl zwischen 1 und 3 eingeben.");
                return gefilterteListe;
            }
            switch (eingabe)
            {
                case 1:
                    Console.Write("Geben Sie den Monat (1-12) ein: ");
                    var monat_check = int.TryParse(Console.ReadLine(), out int monat);
                    if (monat_check && monat > 0 && monat < 13)
                    {
                        FilternNachMonat(monat);
                        GefilterteEintraegeAnzeigen();
                        gefilterteListe = GefilterteEintraege;
                        FilterEisetzen(gefilterteListe);

                    }
                    else
                        Console.WriteLine("Falsche Eingabe!");

                    break;
                case 2:
                    Console.Write("Geben Sie das Jahr (z.B. 2025) ein: ");
                    var jahr_only_check = int.TryParse(Console.ReadLine(), out int jahr_only);
                    if (!jahr_only_check)
                    {
                        Console.WriteLine("Falsche Eingabe!");
                        return gefilterteListe;
                    }
                    FilternNachJahr(jahr_only);
                    GefilterteEintraegeAnzeigen();
                    gefilterteListe = GefilterteEintraege;
                    FilterEisetzen(gefilterteListe);

                    break;
                case 3:
                    Console.Write("Geben Sie die Kategorie ein: ");
                    string kategorie = Console.ReadLine();
                    if (KategorieClass.Kategorien.All(kat => kat.NAME != kategorie))
                    {
                        Console.WriteLine("Kategorie existiert nicht!");
                        return gefilterteListe;
                    }

                    FilternNachKategorie(kategorie);
                    gefilterteListe = GefilterteEintraege;
                    FilterEisetzen(gefilterteListe);
                    break;
                case 4:
                    gefilterteListe = FilterZuruecksetzen();
                    return gefilterteListe;
                case 5:
                    var aktuellerUser = User.users.FirstOrDefault(u => u.ID == User.AktuellerUserID);
                    string dateiname = $"Einträge_{aktuellerUser.NAME}_{DateTime.Now.ToShortDateString()}.pdf";

                    if (aktuellerUser != null)
                    {
                        Statistik.ExportToPdf(gefilterteListe, dateiname);
                    }
                    break;
                case 6:
                    var aktuellerUser1 = User.users.FirstOrDefault(u => u.ID == User.AktuellerUserID);
                    string dateiname1 = $"Einträge_{aktuellerUser1.NAME}_{DateTime.Now.ToShortDateString()}.pdf";
                    if (aktuellerUser1 != null)
                    {
                        //Statistik.ExportToPdf(gefilterteListe, dateiname);
                        //string test = @"C:\Users\ForerOlga\source\repos\MeineProjekte1\Haushaltsbuch\bin\Debug\net8.0\Haushalt_Helena_20.10.2025.pdf";
                        DruckenEintraege(dateiname1);
                    }
                    break;
                default:
                    Console.WriteLine("Ungültige Eingabe. Bitte eine Zahl zwischen 1 und 4 eingeben.");
                    return gefilterteListe;
            }

            return gefilterteListe;
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
            Console.WriteLine("Filter zurückgesetzt.");
            return GefilterteEintraege;
        }
        public static List<Eintrag> FilternNachMonat(int monat)
        {
            Gesamtausgaben = 0;
            GefilterteEintraege.RemoveAll(e => e.Datum.Month != monat);
            foreach (Eintrag eintrag in GefilterteEintraege)
            {
                Gesamtausgaben += eintrag.Betrag;
            }
            Console.WriteLine($"\nSie haben {Gesamtausgaben} Euro ausgegeben.");
            return GefilterteEintraege;
        }
        public static List<Eintrag> FilternNachJahr(int jahr)
        {
            Gesamtausgaben = 0;

            GefilterteEintraege.RemoveAll(e => e.Datum.Year != jahr);
            foreach (Eintrag eintrag in GefilterteEintraege)
            {
                Gesamtausgaben += eintrag.Betrag;
            }
            Console.WriteLine($"\nSie haben {Gesamtausgaben} Euro ausgegeben.");

            return GefilterteEintraege;
        }

        public static List<Eintrag> FilternNachKategorie(string kategorie)
        {
            Gesamtausgaben = 0;
            GefilterteEintraege.RemoveAll(e => e.Kategorie != kategorie);
            foreach (Eintrag eintrag in GefilterteEintraege)
            {
                Gesamtausgaben += eintrag.Betrag;
            }
            Console.WriteLine($"\nSie haben {Gesamtausgaben} Euro ausgegeben.");

            return GefilterteEintraege;
        }
        //public static void FilternNachKategorie()//(string kategorie)
        //{
        //    Kategoriesumme = 0;
        //    int count = 0;
        //    Console.WriteLine("\nKategorie / Betrag im Euro");

        //    foreach (var kat in KategorieClass.Kategorien)
        //    {
        //        foreach (Eintrag eintrag in Eintrag.Eintraege)
        //        {
        //            if (eintrag.Kategorie == kat.NAME && eintrag.UserId == User.AktuellerUserID)
        //            {
        //                count++;
        //                Kategoriesumme += eintrag.Betrag;
        //            }
        //        }
        //        if (Kategoriesumme != 0)
        //            Console.WriteLine($"{count} / {kat.NAME} / {Kategoriesumme}");
        //        Kategoriesumme = 0;
        //    }

        //}

        //option einbauen tägliche ausgaben, monatlichen ausgaben, jährliche ausgaben??


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
                        Console.WriteLine($"Ausgabenlimit: {user.AUSGABENLIMIT}");
                        if (MonatlicheGesamtausgaben > user.AUSGABENLIMIT)
                        {

                            Console.Write($"Ausgaben im {DateTime.Now.ToString("MMMM")} {DateTime.Now.ToString("yyyy")}:  ");
                            RedText(MonatlicheGesamtausgaben.ToString());
                        }
                        else
                        {
                            Console.Write($"Ausgaben im {DateTime.Now.ToString("MMMM")} {DateTime.Now.ToString("YYYY")}:   ");
                            BlauText(MonatlicheGesamtausgaben.ToString());
                        }
                    }
            }

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
                    Console.WriteLine("\nUngültige Eingabe. Bitte 'j' oder 'n' eingeben.");
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
                        SoundPlayer player_s = new SoundPlayer("success.wav");
                        player_s.Play();
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
                    if (MonatlicheGesamtausgaben > user.AUSGABENLIMIT)
                    {
                        string warnung = $"Warnung: Ihre Ausgaben {Gesamtausgaben} haben das festgelegte monatlichen Limit von {user.AUSGABENLIMIT} überschritten!";
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
            PdfSharp.Pdf.PdfDocument dokument = new PdfSharp.Pdf.PdfDocument();

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
                y += 20;
            }

            dokument.Save(dateiPfad);
            Console.WriteLine($"PDF erfolgreich gespeichert: {Path.GetFullPath(dateiPfad)}");
            
        }
        public static void DruckenEintraege(string dateiPfad)
        {


            if (File.Exists(dateiPfad))
            {            
                Console.WriteLine("PDF wird gedruckt...");
                var args = $"-print-to-default \"{dateiPfad}\"";
                Process.Start(new ProcessStartInfo(dateiPfad, args) { CreateNoWindow = true, UseShellExecute = true });
            }
            else
            {
                ExportToPdf(GefilterteEintraege, dateiPfad);
                DruckenEintraege(dateiPfad);
            }
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


// Stellen Sie sicher, dass Sie das NuGet-Paket "System.Drawing.Common" installiert haben.
// Fügen Sie in Ihrem Projekt (z.B. über NuGet-Paket-Manager oder mit folgendem Befehl im Terminal):
// dotnet add package System.Drawing.Common

// Zusätzlich: Für .NET 6+ auf Nicht-Windows-Systemen müssen Sie eventuell in Ihrer .csproj-Datei Folgendes ergänzen:
// <PropertyGroup>
//   <UseSystemDrawing>true</UseSystemDrawing>
// </PropertyGroup>
//
// Beispiel für die .csproj-Datei:
//
// <Project Sdk="Microsoft.NET.Sdk">
//   <PropertyGroup>
//     <OutputType>Exe</OutputType>
//     <TargetFramework>net6.0</TargetFramework>
//     <UseSystemDrawing>true</UseSystemDrawing>
//   </PropertyGroup>
// </Project>
//
// Damit wird sichergestellt, dass "System.Drawing.Printing.PrintDocument" verfügbar ist.
