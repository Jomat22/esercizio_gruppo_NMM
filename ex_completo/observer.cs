public interface IOrderObserver {
    void Aggiorna(string messaggio);
}

public class LoggerService : IOrderObserver {
    public void Aggiorna(string msg) => 
        Console.WriteLine($"[LOG - {DateTime.Now:T}]: {msg}");
}

public class EmailService : IOrderObserver {
    public void Aggiorna(string msg) => 
        Console.WriteLine($"[EMAIL CLIENTE]: {msg}");
}

// Il Manager che coordina tutto e notifica gli osservatori (+ facade)
public class OrderManager {
    private List<IOrderObserver> _observers = new List<IOrderObserver>();
    private IPricingStrategy _strategy = new StandardPricing();

    public void AggiungiOsservatore(IOrderObserver obs) => _observers.Add(obs);

    public void ImpostaStrategia(IPricingStrategy strategy) {
        _strategy = strategy;
        Notifica("Strategia di prezzo cambiata.");
    }

    public void EseguiCheckout(IProduct prodotto) {
        double prezzoLavorato = prodotto.GetPrice();
        double prezzoScontato = _strategy.CalcolaPrezzo(prezzoLavorato);
        double iva = ModShopConfig.Instance.Iva;
        double totaleConiva = prezzoScontato * (1 + iva);

        Console.WriteLine("\n--- RIEPILOGO ORDINE ---");
        Console.WriteLine($"Articolo: {prodotto.GetName()}");
        Console.WriteLine($"Totale con iva: {totaleConiva:F2} {ModShopConfig.Instance.Valuta}");
        Console.WriteLine("------------------------\n");
        
        Notifica($"Checkout completato: {prodotto.GetName()} a {totaleConiva:F2}");
    }

    private void Notifica(string msg) {
        foreach (var obs in _observers) obs.Aggiorna(msg);
    }
}