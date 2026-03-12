using System;

namespace ModShop
{
   
    // 1. STRATEGY PATTERN (Tua competenza)
       public interface IPricingStrategy 
    { 
        double CalcolaPrezzoFinale(double prezzoBase); 
    }

    // Strategia 1: Prezzo Pieno
    public class StandardPricing : IPricingStrategy
    {
        public double CalcolaPrezzoFinale(double prezzoBase)
        {
            return prezzoBase;
        }
    }

    // Strategia 2: Sconto 20%
    public class PromoPricing : IPricingStrategy
    {
        public double CalcolaPrezzoFinale(double prezzoBase)
        {
            return prezzoBase * 0.80;
        }
    }

    // 2. FACADE PATTERN
   
    public class ModShopFacade
    {
        private IPricingStrategy _strategiaCorrente;

        public ModShopFacade()
        {
            // Impostiamo una strategia di default
            _strategiaCorrente = new StandardPricing();
        }

        public void CambiaStrategiaPrezzo(IPricingStrategy nuovaStrategia)
        {
            _strategiaCorrente = nuovaStrategia;
            Console.WriteLine("[FACADE]: Strategia di prezzo aggiornata.");
        }

        // Metodo principale della Facade: semplifica l'acquisto
        public void AcquistaProdotto(IProdotto prodottoPersonalizzato)
        {
            Console.WriteLine("\n--- ELABORAZIONE ORDINE TRAMITE FACADE ---");
            
            // 1. Recupera il prezzo dal prodotto (che potrebbe essere decorato)
            double prezzoIniziale = prodottoPersonalizzato.GetPrezzo();
            
            // 2. Applica la strategia di prezzo scelta
            double prezzoScontato = _strategiaCorrente.CalcolaPrezzoFinale(prezzoIniziale);
            
            // 3. Recupera l'IVA dal Singleton (creato dal tuo compagno)
            double iva = AppContext.GetIstanza().GetIva();
            double totale = prezzoScontato + (prezzoScontato * iva / 100);

            // 4. Output finale pulito
            Console.WriteLine("Prodotto: " + prodottoPersonalizzato.GetDescrizione());
            Console.WriteLine("Prezzo Finale (Ivato): " + totale + " " + AppContext.GetIstanza().Valuta);
            Console.WriteLine("------------------------------------------\n");
        }
    }
}