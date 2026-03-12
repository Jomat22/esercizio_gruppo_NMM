class Program {
    static void Main(string[] args) {
        OrderManager shop = new OrderManager();
        shop.AggiungiOsservatore(new LoggerService());
        shop.AggiungiOsservatore(new EmailService());
        Console.Clear();

        while (true) {
            try {
                Console.WriteLine("Cosa vuoi comprare?");
                Console.Write("Inserisci prodotto (1/TSHIRT, 2/MUG, 3/SKIN) o 'esci': ");
                string input = Console.ReadLine()?.ToUpper();
                if (input == "ESCI") break;

                IProduct ordine = ProductFactory.CreateProduct(input);

                // Menu Decorator
                bool altro = true;
                while (altro) {
                    Console.WriteLine($"\nConfigurazione: {ordine.GetName()} ({ordine.GetPrice():F2}€)");
                    Console.WriteLine("Aggiungi: 1.Stampa F/R(5.50) 2.Incisione(4.00) 3.Regalo(3.00) 4.Estensione_Garanzia(12.00) 0.Fine");
                    string opt = Console.ReadLine();
                    if (opt == "1") ordine = new StampaFronteRetro(ordine);
                    else if (opt == "2") ordine = new Incisione(ordine);
                    else if (opt == "3") ordine = new ConfRegalo(ordine);
                    else if (opt == "4") ordine = new EstensioneGaranzia(ordine);

                    else altro = false;
                }

                // Menu Strategy
                Console.WriteLine("\nScegli Pricing: 1.Standard 2.Promo(-20%) 3.Wholesale(Se l'ordine supera i 50 euro applica il 30%)");
                string strat = Console.ReadLine();
                if (strat == "2") shop.ImpostaStrategia(new PromoPricing());
                else if (strat == "3") shop.ImpostaStrategia(new WholesalePricing());
                else shop.ImpostaStrategia(new StandardPricing());

                shop.EseguiCheckout(ordine);
            }
            catch (Exception ex) {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }
    }
}