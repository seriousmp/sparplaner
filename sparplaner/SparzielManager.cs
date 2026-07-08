using System.Text.Json;

namespace sparplaner
{
    public class SparzielManager
    {
        private const string DateiPfad = "ziele.json";
        public List<Sparziel> Ziele { get; private set; } = new();

        public void Hinzufuegen(Sparziel ziel) => Ziele.Add(ziel);

        public void Speichern()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(Ziele, options);
                File.WriteAllText(DateiPfad, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern: {ex.Message}");
            }
        }

        public void Laden()
        {
            if (!File.Exists(DateiPfad)) return;
            try
            {
                string inhalt = File.ReadAllText(DateiPfad);
                Ziele = JsonSerializer.Deserialize<List<Sparziel>>(inhalt) ?? new();
            }
            catch
            {
                Console.WriteLine("Fehler beim Laden der Daten. Starte mit leerer Liste.");
                Ziele = new();
            }
        }
    }
}
