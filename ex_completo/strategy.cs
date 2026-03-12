public interface IPricingStrategy {
    double CalcolaPrezzo(double prezzoBase);
}

public class StandardPricing : IPricingStrategy {
    public double CalcolaPrezzo(double prezzoBase) => prezzoBase;
}

public class PromoPricing : IPricingStrategy {
    public double CalcolaPrezzo(double prezzoBase) => prezzoBase * 0.80; // sconto del 20%
}

public class WholesalePricing : IPricingStrategy {
    // Sconto del 30% se l'ordine supera i 50 euro
    public double CalcolaPrezzo(double prezzoBase) => 
        prezzoBase > 50 ? prezzoBase * 0.70 : prezzoBase;
}