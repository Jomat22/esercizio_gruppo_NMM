using System;
using System.Collections.Generic;

// Interfaccia base per tutti i prodotti
public interface IProduct {
    string GetName();
    double GetPrice();
}

// SINGLETON: Gestisce IVA e Valuta in modo centralizzato
public sealed class ModShopConfig {
    private static readonly ModShopConfig _instance = new ModShopConfig();
    
    public string Valuta { get; set; } = "EUR";
    public double Iva { get; set; } = 0.22; 

    private ModShopConfig() { }

    public static ModShopConfig Instance => _instance;
}