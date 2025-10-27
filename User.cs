using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{

    internal class User
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string PASSWORT { get; set; }
        public static int AktuellerUserID { get; set; }
        public double AUSGABENLIMIT { get; set; }

        public static List<User> users = new List<User>();

        public User(int id, string name, string passwort, double AusgabenLimit)
        {
            ID = id;
            NAME = name;
            PASSWORT = passwort;
            AUSGABENLIMIT = AusgabenLimit;
        }

        public static void UserErstellen()
        {
            int id;
            Console.WriteLine("Geben Sie Ihren Namen ein:");
            string name = Console.ReadLine();
            Console.WriteLine("Geben Sie Ihr Passwort ein:");
            string passwort = Console.ReadLine();
            if (users.Count == 0)
                id = 1;
            else
                id = users[users.Count-1].ID + 1;

            User user = new User(id, name, passwort, 0.0);
            users.Add(user);
            Json.JsonSpeichern(users, KategorieClass.Kategorien, Eintrag.Eintraege);
            Statistik.BlauText("Benutzer erstellt: " + user.NAME);
        }

        public static int UserLogin()
        {
            Console.WriteLine("Geben Sie Ihren Namen ein:");
            string name = Console.ReadLine()??"";
            Console.WriteLine("Geben Sie Ihr Passwort ein:");
            string passwort = ReadPassword();
            foreach (User user in users)
            {
                if (user.NAME == name && user.PASSWORT == passwort)
                {
                    SoundPlayer player_s = new SoundPlayer("success.wav");
                    player_s.Play();
                    Statistik.BlauText("Login erfolgreich: " + user.NAME);
                    AktuellerUserID = user.ID;
                    return AktuellerUserID;
                }
            }
            SoundPlayer player_e = new SoundPlayer("error.wav");
            player_e.Play();
            Statistik.RedText("Login fehlgeschlagen");
            return 0;
        }

        private static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(intercept: true); // kein Echo der Taste
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password[0..^1]; // letztes Zeichen löschen
                        Console.Write("\b \b"); // Stern entfernen
                    }
                }
                else if (keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        ////nachdenken ob löschen und anzeigen Methode erforderlich ist
        //public static void UserLoeschen()
        //{
        //    Console.WriteLine("Geben Sie den Namen ein:");
        //    string name = Console.ReadLine();

        //    foreach (User user in users)
        //    {
        //        if (user.NAME == name)
        //        {
        //            users.Remove(user);
        //            Console.WriteLine("Benutzer gelöscht: " + user.NAME);
        //            return;
        //        }
        //    }
        //}

        //public static void UserAnzeigen()
        //{
        //    Console.WriteLine("Benutzer:");
        //    foreach (User user in users)
        //    {
        //        Console.WriteLine("Name: " + user.NAME + " Passwort: " + user.PASSWORT);
        //    }
        //}


    }
}