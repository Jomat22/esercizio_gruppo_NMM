using System;
using System.Collections.Generic;
namespace ModShop;
// Singleton
// Garantire che esista una sola istanza della configurazione 
// per evitare discrepanze (es. diverse aliquote IVA in punti diversi del codice).
public class ModShopConfig
{
    private static ModShopConfig _instance;
    // dati globali dell'applicazione
    public string Valuta { get; set; } = "EUR";
    public double Iva { get; set; } = 0.22;
    // il costruttore è privato: impedisce a chiunque di fare 'new ModShopConfig()'
    private ModShopConfig() { }
    // punto di accesso globale all'unica istanza
    public static ModShopConfig Instance
    {
        get
        {
            //initialization: crea l'istanza solo la prima volta che viene chiesta
            if (_instance == null)
            {
                _instance = new ModShopConfig();
            }
            return _instance;
        }
    }
}
// Observer
// notificare automaticamente diversi moduli (Email, Log, Display)
// ogni volta che lo stato di un ordine cambia, senza accoppiare le classi tra loro.
// L'interfaccia che ogni "Osservatore" deve implementare
public interface IOrderObserver
{
    void Notifica(string messaggio);
}
// soggetto  che viene osservato
public abstract class OrderSubject
{
    private List<IOrderObserver> _observers = new List<IOrderObserver>();

    // aggiunge un osservatore alla lista (es. iscrizione alla newsletter/log)
    public void Attacca(IOrderObserver observer)
    {
        _observers.Add(observer);
    }
    // rimuove un osservatore
    public void Stacca(IOrderObserver observer)
    {
        _observers.Remove(observer);
    }
    // avvisa tutti gli osservatori registrati
    protected void NotificaTutti(string messaggio)
    {
        foreach (var observer in _observers)
        {
            observer.Notifica(messaggio);
        }
    }
}
//esempio concreto
public class EmailService : IOrderObserver
{
    public void Notifica(string msg) => 
        Console.WriteLine($"[Email]: Invio email al cliente... Evento: {msg}");
}
public class LoggerService : IOrderObserver
{
    public void Notifica(string msg) => 
        Console.WriteLine($"[Log]: Scrittura su file... {DateTime.Now}: {msg}");
}
// integrazione nel gestore ordini 
public class OrderManager : OrderSubject
{
    public void CreaNuovoOrdine(string prodotto)
    {
        // usa il Singleton per leggere i dati di configurazione
        string valutaAttuale = ModShopConfig.Instance.Valuta;
        Console.WriteLine($"\n--- Elaborazione Ordine: {prodotto} ({valutaAttuale}) ---");
        // azione completata -> Notifica gli osservatori (Observer)
        NotificaTutti($"Creato ordine per {prodotto}");
    }
}
class Program
{
    static void Main()
    {
        //Setup degli Osservatori
        OrderManager manager = new OrderManager();
        manager.Attacca(new EmailService());
        manager.Attacca(new LoggerService());
        //utilizzo del Singleton per cambiare valuta globalmente
        ModShopConfig.Instance.Valuta = "USD";
       // esecuzione
        manager.CreaNuovoOrdine("Smartphone Ultra");
        
        manager.CreaNuovoOrdine("Laptop Pro");
    }
}