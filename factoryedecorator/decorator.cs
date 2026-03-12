// Decoratore Base
public abstract class ProductDecorator : IProduct {
    protected IProduct _product;
    public ProductDecorator(IProduct product) => _product = product;
    public virtual string GetName() => _product.GetName();
    public virtual double GetPrice() => _product.GetPrice();
}

// 1. Stampa Fronte/Retro
public class StampafronteRetro : ProductDecorator {
    public StampafronteRetro(IProduct p) : base(p) { }
    public override string GetName() => base.GetName() + " [Stampa Fronte/Retro]";
    public override double GetPrice() => base.GetPrice() + 5.50;
}

// 2. Confezione Regalo
public class ConfRegalo : ProductDecorator {
    public ConfRegalo(IProduct p) : base(p) { }
    public override string GetName() => base.GetName() + " [Confezione regalo]";
    public override double GetPrice() => base.GetPrice() + 3.00;
}

// 3. Estensione Garanzia 
public class EstensioneGaranzia : ProductDecorator {
    public EstensioneGaranzia(IProduct p) : base(p) { }
    public override string GetName() => base.GetName() + " [Garanzia extra]";
    public override double GetPrice() => base.GetPrice() + 12.00;
}

// 4. Incisione 
public class Incisione : ProductDecorator {
    public Incisione(IProduct p) : base(p) { }
    public override string GetName() => base.GetName() + " [Incisione]";
    public override double GetPrice() => base.GetPrice() + 4.00;
}