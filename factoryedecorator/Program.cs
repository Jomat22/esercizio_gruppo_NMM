using System;

class Program {
    static void Main() {
        ModShopFacade negozio = new ModShopFacade();
        bool continua = true;

        while (continua) {
            Console.Clear();
            Console.WriteLine("ModShop menù");
            Console.WriteLine("Scegli prodotto: [1] TSHIRT | [2] MUG | [3] SKIN | [0] Esci");
            string scelta = Console.ReadLine();

            if (scelta == "0") break;

            string codice = scelta switch { "1" => "TSHIRT", "2" => "MUG", "3" => "SKIN", _ => "" };
            if (codice == "") continue;

            // Personalizzazioni (Decorator)
            bool stampa = dec ("Vuoi Stampa Fronte/Retro?");
            bool regalo = dec ("Vuoi Confezione Regalo?");
            bool garanzia = dec ("Vuoi Estensione Garanzia?");
            bool incisione = dec ("Vuoi Incisione?");

            //continua con gli altri pattern...
