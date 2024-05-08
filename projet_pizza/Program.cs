using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace projet_pizza
{
    public class PizzaPersonnalisee : Pizza
    {
        private static int numeroPizza = 1;
        public PizzaPersonnalisee() : base("Personnalisée " + numeroPizza, 0, false, null)
        {
            ingredients = new List<string>();
            while (true)
            {
                if (ingredients.Count != 0)
                {
                    Console.Write("Liste des ingrédients : ");
                    Console.WriteLine(string.Join(", ", ingredients));
                }
                Console.Write($"Entrez un ingrédient pour la pizza personnalisée {numeroPizza} (ENTER pour terminer) : ");
                string ingredient = Console.ReadLine().ToLower();
                if (ingredients.Contains(ingredient.ToLower()))
                    Console.WriteLine("Erreur, la liste contiens déjà cet élément");

                else
                {
                    if (string.IsNullOrWhiteSpace(ingredient))
                        break;
                    this.ingredients.Add(ingredient);
                }
            }
            numeroPizza += 1;
            this.prix = 5f + this.ingredients.Count * 1.5f;
            Console.Clear();
        }
    }
    public class Pizza
    {
        public string nom {  get; init; }
        public float prix { get; init; }
        public bool vegetarienne { get; init; }
        public List<string> ingredients { get; protected set; }

        public Pizza(string nom, float prix, bool vegetarienne, List<string> ingredients)
        {
            this.nom = nom;
            this.prix = prix;
            this.vegetarienne = vegetarienne;
            this.ingredients = ingredients;
        }

        public void Afficher()
        {
            string badgeVegetarienne = this.vegetarienne ? " (V)" : "";
            string nomAffiche = FormatPremiereLettreMajuscule(this.nom);
            var ingredientsAffiche = this.ingredients.Select(x => FormatPremiereLettreMajuscule(x)).ToList();

            Console.WriteLine($"{nomAffiche}{badgeVegetarienne} - {this.prix}€");
            Console.WriteLine(string.Join(", ", ingredientsAffiche));
            Console.WriteLine();
        }

        private string FormatPremiereLettreMajuscule(string chaine)
        {
            if (string.IsNullOrEmpty(chaine) || string.IsNullOrWhiteSpace(chaine))
                return chaine;

            string chaineMinusucle = chaine.ToLower();
            string chaineMajuscule = chaine.ToUpper();

            return chaineMajuscule[0] + chaineMinusucle[1..];
        }

        public bool ContientIngredient(string ingredient)
        {
            return this.ingredients.Where(i => i.ToLower().Contains(ingredient)).ToList().Count > 0;
        }
    }
    public class Program
    {
        public static List<Pizza> GetPizzaFromCode()
        {
            var listeDePizza = new List<Pizza>()
            {
                new Pizza("4 fromages", 11.5f, true, new List<string> {"cantal", "mozzarella", "fromage de chèvre", "gruyère"}),
                new Pizza("indienne", 10.50f, false, new List < string > { "curry", "mozzarella", "poulet", "poivron", "oignon", "coriandre" }),
                new Pizza("mexicaine", 13f, false, new List < string > { "boeuf", "mozzarella", "maïs", "tomates", "oignon", "corlandre" }),
                new Pizza("margherita", 8f, true, new List < string > { "sauce tomate", "mozzarella", "basilic" }),
                new Pizza("calzone", 12f, false, new List < string > { "tomate", "jambon", "persil", "oignons" }),
                new Pizza("complète", 9.5f, false, new List < string > { "jambon", "oeuf", "fromage" }),
            };

            return listeDePizza;
        }
        public static List<Pizza> GetPizzaFromFile(string filename)
        {
            string data;

            try
            {
                data = File.ReadAllText(filename);
            }

            catch 
            {
                Console.WriteLine("An error occured : File Not Found !");
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<List<Pizza>>(data);
            }
            catch
            {
                Console.WriteLine("An error occured : invalid data");
                return null;
            }

        }
        public static void GenerateJsonFile(List<Pizza> pizzas, string filename)
        {
            string pizzaSerialise;

            try
            {
                pizzaSerialise = JsonConvert.SerializeObject(pizzas);
            }
            catch
            {
                Console.WriteLine("An error occured : Serialization Failed");
                return;
            }

            File.WriteAllText(filename, pizzaSerialise);
        }
        public static List<Pizza> GetPizzaFromUrl(string url)
        {
            var webClient = new WebClient();
            string getData = null;

            Console.WriteLine("Téléchargement des données...");
            try
            {
                getData = webClient.DownloadString(url);
            }
            catch(WebException ex)
            {
                if (ex.Response == null)
                {
                    Console.WriteLine("An error occured, please check your internet connexion or your url !");
                    return null;
                }
                var statusCode = (HttpWebResponse)ex.Response;
                
                if (statusCode.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("An error occured, File not found");
                    return null;
                }
            }

            try
            {
                return JsonConvert.DeserializeObject<List<Pizza>>(getData);
            }
            catch
            {
                Console.WriteLine("An error occured : Deserialization Failed");
                return null;
            }
        }
        public static void Main(string[] args)
        {
            string url = "https://codeavecjonathan.com/res/pizzas2.json";

            Console.OutputEncoding = Encoding.UTF8;
            //string path = Path.Combine("Data_pizza", "pizzas.json");

            var pizzas = GetPizzaFromUrl(url);

            if (pizzas !=  null)
            {
                foreach (var pizza in pizzas)
                    pizza.Afficher();
            }

        }
    }
}