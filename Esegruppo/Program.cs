using System;

namespace ModShop
{
    // interfaccia e modelli
    public interface IProdotto
    {
        string GetDescrizione();
        double GetPrezzo();
    }

    // Prodotto base
    public class TShirt : IProdotto
    {
        public string GetDescrizione() { return "T-Shirt Modulare"; }
        public double GetPrezzo() { return 15.00; }
    }
    // SINGLETON DI Nahira
    public class ModShopConfig
    {
        private static ModShopConfig _instance;
        public string Valuta { get; set; } = "EUR";
        public double Iva { get; set; } = 0.22;
        private ModShopConfig() { }
        public static ModShopConfig Instance
        {
            get {
                if (_instance == null) _instance = new ModShopConfig();
                return _instance;
            }
        }
    }
    // 1. STRATEGY PATTERN*****
    public interface IPricingStrategy  //interfaccia base definendo la firma del metodo  calcolaprezzofinale(r.36) 
    { 
        double CalcolaPrezzoFinale(double prezzoBase); 
    }

    public class StandardPricing : IPricingStrategy //strategia predefinita, implementa l'interfaccia e restituisce il prezzo pieno, senza fare modifiche.
    {
        public double CalcolaPrezzoFinale(double prezzoBase) { return prezzoBase; }
    }

    public class PromoPricing : IPricingStrategy
    {
        public double CalcolaPrezzoFinale(double prezzoBase) { return prezzoBase * 0.80; } //Questa è la strategia scontata. Qui il metodo moltiplica il prezzo per 0.80, applicando quindi uno sconto del 20%."
    }
    // 2. FACADE PATTERN****
    public class ModShopFacade //LIFE Questa classe serve a semplificare la vita a chi usa il programma. Nasconde la complessità degli altri pattern.
    {
        private IPricingStrategy _strategiaCorrente; //creo una variabile privata per memorizzare quale strategia di prezzo stiamo usando in questo momento.

        public ModShopFacade() 
        {
            _strategiaCorrente = new StandardPricing(); //default cosi il negozio parte senza sconti
        }

        public void CambiaStrategiaPrezzo(IPricingStrategy nuovaStrategia) //Questo metodo permette di cambiare lo sconto al volo durante l'esecuzione del programma.
        {
            _strategiaCorrente = nuovaStrategia;
        }

        public void AcquistaProdotto(IProdotto prodotto) //metodo principale che va a ricevere un oggetto
        {
            Console.WriteLine("\n--- ELABORAZIONE ORDINE (FACADE) ---");
            
            double prezzoBase = prodotto.GetPrezzo(); //Qui interrogo il prodotto per sapere quanto costa. Se Teo ha aggiunto dei Decorator, questo metodo mi darà il prezzo già sommato di tutti gli extra.
            double prezzoScontato = _strategiaCorrente.CalcolaPrezzoFinale(prezzoBase); //utilizzo la mia ****Strategy****  Passo il prezzo base e lei mi restituisce il prezzo dopo lo sconto
            
            // Dati dal Singleton di Nahira
            double valoreIva = ModShopConfig.Instance.Iva; 
            string valuta = ModShopConfig.Instance.Valuta;

            double totale = prezzoScontato * (1 + valoreIva); //Eseguo il calcolo finale aggiungendo l'IVA al prezzo già scontato.

            Console.WriteLine("Dettaglio: " + prodotto.GetDescrizione());
            Console.WriteLine("Totale Finale: " + Math.Round(totale, 2) + " " + valuta);
            Console.WriteLine("--------------\n");
        }
    }

    // MAIN DI TEST (Simula il lavoro di Teo)
    class Program
    {
        static void Main(string[] args)
        {
            // Creiamo la Facade
            ModShopFacade negozio = new ModShopFacade();

            // Scegliamo un prodotto (Factory/Decorator di Teo)
            IProdotto mioOrdine = new TShirt();

            // Test 1: Prezzo Standard
            negozio.AcquistaProdotto(mioOrdine);

            // Test 2: Cambio Strategy e nuovo acquisto
            negozio.CambiaStrategiaPrezzo(new PromoPricing());
            negozio.AcquistaProdotto(mioOrdine);

            Console.WriteLine("Premi un tasto per uscire...");
            Console.ReadKey();
        }
    }
}