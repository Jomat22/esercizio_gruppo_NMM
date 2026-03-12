public class ConcreteProduct : IProduct {
    private string _name;
    private double _price;
    public ConcreteProduct(string name, double price) { 
        _name = name; 
        _price = price; 
    }
    public string GetName() => _name;
    public double GetPrice() => _price;
}

// FACTORY: Crea prodotti concreti partendo da una stringa
public static class ProductFactory {
    public static IProduct CreateProduct(string code) {
        return code.ToUpper() switch {
            "TSHIRT" => new ConcreteProduct("T-Shirt", 11.00),
            "1" => new ConcreteProduct("T-Shirt", 11.00),
            "MUG" => new ConcreteProduct("Mug", 7.50),
            "2" => new ConcreteProduct("Mug", 7.50),
            "SKIN" => new ConcreteProduct("Skin", 3.00),
            "3" => new ConcreteProduct("Skin", 3.00),
            _ => throw new ArgumentException($"Codice '{code}' non trovato in catalogo.")
        };
    }
}