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
        string nom;
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
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var ingredients = new List<string>();

            var listeDePizza = new List<Pizza>()
            {
                new Pizza("4 fromages", 11.5f, true, new List<string> {"cantal", "mozzarella", "fromage de chèvre", "gruyère"}),
                new Pizza("indienne", 10.50f, false, new List < string > { "curry", "mozzarella", "poulet", "poivron", "oignon", "coriandre" }),
                new Pizza("mexicaine", 13f, false, new List < string > { "boeuf", "mozzarella", "maïs", "tomates", "oignon", "corlandre" }),
                new Pizza("margherita", 8f, true, new List < string > { "sauce tomate", "mozzarella", "basilic" }),
                new Pizza("calzone", 12f, false, new List < string > { "tomate", "jambon", "persil", "oignons" }),
                new Pizza("complète", 9.5f, false, new List < string > { "jambon", "oeuf", "fromage" }),
                new PizzaPersonnalisee(),
                new PizzaPersonnalisee(),
            };

            foreach (var pizza in listeDePizza)
                pizza.Afficher();
        }
    }
}