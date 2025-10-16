using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{

    internal class User
    {
        public string NAME { get; set; }
        public string PASSWORT { get; set; }

        public static List<User> users = new List<User>();
        public static List<Eintrag> Eintraege = new List<Eintrag>();

        public User(string name, string passwort)
        {
            NAME = name;
            PASSWORT = passwort;
        }

        public static void UserErstellen()
        {
            Console.WriteLine("Geben Sie Ihren Namen ein:");
            string name = Console.ReadLine();
            Console.WriteLine("Geben Sie Ihr Passwort ein:");
            string passwort = Console.ReadLine();
            User user = new User(name, passwort);
            Console.WriteLine("Benutzer erstellt: " + user.NAME);
        }

        public static void UserLogin()
        {
            Console.WriteLine("Geben Sie Ihren Namen ein:");
            string name = Console.ReadLine();
            Console.WriteLine("Geben Sie Ihr Passwort ein:");
            string passwort = Console.ReadLine();
            foreach (User user in users)
            {
                if (user.NAME == name && user.PASSWORT == passwort)
                {
                    Console.WriteLine("Login erfolgreich: " + user.NAME);
                    return;
                }
            }
            Console.WriteLine("Login fehlgeschlagen");
        }


        //nachdenken ob löschen und anzeigen Methode erforderlich ist
        public static void UserLoeschen()
        {
            Console.WriteLine("Geben Sie den Namen ein:");
            string name = Console.ReadLine();
            
            foreach(User user in users)
            {
                if(user.NAME == name)
                {
                    users.Remove(user);
                    Console.WriteLine("Benutzer gelöscht: " + user.NAME);
                    return;
                }
            }
        }

        public static void UserAnzeigen()
        {
            Console.WriteLine("Benutzer:");
            foreach(User user in users)
            {
                Console.WriteLine("Name: " + user.NAME + " Passwort: " + user.PASSWORT);
            }
        }

       
    }
}