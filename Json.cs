using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    internal class Json
    {
       
        public static void JsonLaden(ref List<User> users, ref List<KategorieClass> kategorie, ref List<Eintrag> eintraege)
        {
            if (File.Exists("users.json"))
            {
                var text = File.ReadAllText("users.json");
                var daten = System.Text.Json.JsonSerializer.Deserialize<List<User>>(text);
                if (daten != null) users = daten;
            }
            if (File.Exists("kategorie.json"))
            {
                var text = File.ReadAllText("kategorie.json");
                var daten = System.Text.Json.JsonSerializer.Deserialize<List<KategorieClass>>(text);
                
                if (daten != null) kategorie = daten;
            }
            if (File.Exists("eintraege.json"))
            {
                var text = File.ReadAllText("eintraege.json");
                var daten = System.Text.Json.JsonSerializer.Deserialize<List<Eintrag>>(text);
                if (daten != null) eintraege = daten;
            }
        }

        public static void JsonSpeichern(List<User> users, List<KategorieClass> kategorie, List<Eintrag> eintraege)
        {
            var text = System.Text.Json.JsonSerializer.Serialize(users);
            File.WriteAllText("users.json", text);

            kategorie = KategorieClass.Kategorien;
            text = System.Text.Json.JsonSerializer.Serialize(kategorie);
            File.WriteAllText("kategorie.json", text);
            text = System.Text.Json.JsonSerializer.Serialize(eintraege);
            File.WriteAllText("eintraege.json", text);
        }

    }
}
