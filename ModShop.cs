using System;
using System.Collections.Generic;

namespace ModShop
{
    // 1. Singleton per l'AppContext
    // Garantisce che esista una sola istanza di questa classe in tutta l'app.
    // Centralizza configurazioni globali (IVA, Valuta) e lo storico,
    // evitando discrepanze di dati tra diverse parti del programma.
    public class AppContext
    {
        private static AppContext _instance;
        public string Valuta { get; set; } = "EUR";
        public double Iva { get; set; } = 22.0;
        //extra 1: Storico ordini centralizzato per consultazione globale.
        public List<string> StoricoOrdini { get; } = new List<string>();
        private AppContext() { } // Costruttore privato: impedisce 'new AppContext()' esterno
        public static AppContext Instance => _instance ??= new AppContext();

        public void Log(string messaggio)
        {
            Console.WriteLine($"[SISTEMA-LOG]: {DateTime.Now:HH:mm:ss} - {messaggio}");
        }
    }

    //Observer per il sistema di notifiche
    //crea un legame "uno-a-molti" tra un soggetto e molti osservatori.
    //quando lo stato dell'ordine cambia, tutti i servizi iscritti 
    // (UI, Email) vengono aggiornati automaticamente senza essere "legati" tra loro.
    public interface IOrderObserver { void Aggiorna(string msg); }
    public class UIService : IOrderObserver {
        public void Aggiorna(string msg) => Console.WriteLine($"[DISPLAY-UI]: Notifica ricevuta -> {msg}");
    }
    public class EmailMockService : IOrderObserver {
        public void Aggiorna(string msg) => Console.WriteLine($"[EMAIL-MOCK]: Invio email... {msg}");
    }
    // Factory per ProductFactory
    //incapsula la logica di creazione degli oggetti.
    //permette di creare prodotti complessi usando solo una stringa ("TSHIRT"),
    // separando il codice che "usa" il prodotto dal codice che lo "crea".
    public interface IProduct {
        string GetDescrizione();
        double GetPrezzoBase();
    }
    public class Gadget : IProduct {
        private string _nome; private double _prezzo;
        public Gadget(string n, double p) { _nome = n; _prezzo = p; }
        public string GetDescrizione() => _nome;
        public double GetPrezzoBase() => _prezzo;
    }
    public static class ProductFactory {
        public static IProduct CreateProduct(string code) => code.ToUpper() switch {
            "TSHIRT" => new Gadget("T-Shirt ModShop", 15.00),
            "MUG"    => new Gadget("Mug Ceramica", 8.00),
            "SKIN"   => new Gadget("Skin Digitale", 5.00),
            _ => throw new ArgumentException("Codice prodotto non valido!")
        };
    }
     //Decorator
    //"Avvolge" un oggetto esistente aggiungendo nuove funzionalità.
    //permette di aggiungere incisioni o confezioni regalo a QUALSIASI 
    // prodotto senza dover creare classi come "TShirtConIncisioneERegalo".
    public abstract class ProductDecorator : IProduct {
        protected IProduct _product;
        public ProductDecorator(IProduct p) => _product = p;
        public virtual string GetDescrizione() => _product.GetDescrizione();
        public virtual double GetPrezzoBase() => _product.GetPrezzoBase();
    }
    public class StampaFR : ProductDecorator {
        public StampaFR(IProduct p) : base(p) { }
        public override string GetDescrizione() => base.GetDescrizione() + " [+ Stampa F/R]";
        public override double GetPrezzoBase() => base.GetPrezzoBase() + 5.00;
    }
    public class Incisione : ProductDecorator {
        public Incisione(IProduct p) : base(p) { }
        public override string GetDescrizione() => base.GetDescrizione() + " [+ Incisione Laser]";
        public override double GetPrezzoBase() => base.GetPrezzoBase() + 4.50;
    }
    // Strategy per il Calcolo Pricing
    //definisce una famiglia di algoritmi intercambiabili.
    //permette di cambiare la logica dello sconto (Standard, Promo, Wholesale) 
    // a runtime senza usare infiniti "if/else".
    public interface IPricingStrategy { double Calcola(double p); }
    public class StandardPricing : IPricingStrategy { public double Calcola(double p) => p; }
    public class PromoPricing : IPricingStrategy { public double Calcola(double p) => p * 0.85; } // -15%
    public class WholesalePricing : IPricingStrategy { public double Calcola(double p) => p * 0.70; } // -30%

    // Facadeche è considerato come il motore degli ordini
    //fornisce un'interfaccia semplice a un sistema complesso.
    //nasconde al Main il coordinamento tra Factory, Decorator, Strategy e Observer
    //il Main chiama 'EseguiCheckout' e la Facade fa tutto il resto.
    public class ModShopEngine
    {
        private List<IOrderObserver> _observers = new();
        private IPricingStrategy _strategy = new StandardPricing();

        public ModShopEngine() {
            _observers.Add(new UIService());
            _observers.Add(new EmailMockService());
        }
        public void ImpostaStrategia(IPricingStrategy s) {
            _strategy = s;
            NotificaTutti("Strategia di prezzo aggiornata.");
        }
        public void NotificaTutti(string msg) {
            foreach (var obs in _observers) obs.Aggiorna(msg);
            AppContext.Instance.Log(msg);
        }
        public void EseguiCheckout(IProduct p) {
            // applica Strategy
            double prezzoScontato = _strategy.Calcola(p.GetPrezzoBase());
            // applica Singleton (IVA)
            var config = AppContext.Instance;
            double prezzoFinale = prezzoScontato + (prezzoScontato * config.Iva / 100);
            string info = $"{p.GetDescrizione()} | Totale: {prezzoFinale:F2} {config.Valuta}";
            Console.WriteLine($"\n--- RIEPILOGO: {info} ---\n");
            //salva nello storico (Extra)
            config.StoricoOrdini.Add(info);
            NotificaTutti($"Ordine finalizzato correttamente.");
        }
    }
//main
    class Program
    {
        static void Main()
        {
            ModShopEngine motore = new ModShopEngine();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n==== Menu ModShop ====");
                Console.WriteLine("1. Crea Ordine (TSHIRT / MUG / SKIN)");
                Console.WriteLine("2. Visualizza Storico (Extra 1)");
                Console.WriteLine("3. Impostazioni Globali (Singleton)");
                Console.WriteLine("0. Esci");
                Console.Write("Selezione: ");
                string menu = Console.ReadLine();
                switch (menu)
                {
                    case "1":
                        GestisciNuovoOrdine(motore);
                        break;
                    case "2":
                        Console.WriteLine("\n--- STORICO ORDINI ---");
                        AppContext.Instance.StoricoOrdini.ForEach(Console.WriteLine);
                        break;
                    case "3":
                        Console.Write("Inserisci Valuta (es. USD): ");
                        AppContext.Instance.Valuta = Console.ReadLine().ToUpper();
                        break;
                    case "0":
                        running = false;
                        break;
                }
            }
        }
        static void GestisciNuovoOrdine(ModShopEngine motore)
        {
            try {
                //Uso della Factory
                Console.Write("Inserisci codice prodotto: ");
                string code = Console.ReadLine()?.ToUpper();
                IProduct carrello = ProductFactory.CreateProduct(code);
                //Uso del Decorator
                Console.Write("Aggiungere Stampa F/R (+5€)? (s/n): ");
                if (Console.ReadLine()?.ToLower() == "s") carrello = new StampaFR(carrello);
                Console.Write("Aggiungere Incisione Laser (+4.5€)? (s/n): ");
                if (Console.ReadLine()?.ToLower() == "s") carrello = new Incisione(carrello);
                //Uso della Strategy (Extra 2: Coupon)
                Console.Write("Inserisci coupon (PROMO / WHOLESALE / INVIO per Standard): ");
                string coupon = Console.ReadLine()?.ToUpper();
                motore.ImpostaStrategia(coupon switch {
                    "PROMO" => new PromoPricing(),
                    "WHOLESALE" => new WholesalePricing(),
                    _ => new StandardPricing()
                });
                //Uso della Facade per chiudere l'ordine
                motore.EseguiCheckout(carrello);
            }
            catch (Exception ex) {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }
    }
}