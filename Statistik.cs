using System;
using System.Collections.Generic;
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


        //option einbauen tägliche ausgaben, monatlichen ausgaben, jährliche ausgaben??
        public static void GesamtausgabenAnzeigen()
        {
            Console.WriteLine();
            foreach (User user in User.users)
            {
                if (user.ID == User.AktuellerUserID)
                {

                    Console.WriteLine($"Monatliche Ausgabenlimit: {user.AUSGABENLIMIT} Euro");
                    GesamtausgabenBerechen();
                    if (user.AUSGABENLIMIT == 0)
                        Console.Write($"Monatliche Gesamtausgaben: {Gesamtausgaben} Euro.");
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
            Console.WriteLine("Kategorie \t\t Betrag im Euro.");

            foreach (var kategorie in KategorieClass.Kategorien)
            {
                foreach (Eintrag eintrag in Eintrag.Eintraege)
                {
                    if (eintrag.Kategorie == kategorie.NAME && eintrag.UserId == User.AktuellerUserID)
                    {
                        Kategoriesumme += eintrag.Betrag;
                    }
                }
                if (Kategoriesumme != 0)
                    Console.WriteLine($"{kategorie.NAME} \t\t {Kategoriesumme}");
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
                    Console.WriteLine();
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
                y += 20;
            }

            dokument.Save(dateiPfad);
            Console.WriteLine($"PDF erfolgreich gespeichert: {Path.GetFullPath(dateiPfad)}");
        }

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
