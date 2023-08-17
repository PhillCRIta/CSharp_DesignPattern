class FatctoryMethod
{
    public enum Tipo { Standard, Asynchronus, StandardWithClass, TrackAndWeakReference, StandardInternal }
    public FatctoryMethod(Tipo tipo)
    {
        System.Console.Write("Factory > ");
        IFactory b;
        switch (tipo)
        {
            case Tipo.Standard:
                //b = new FatctoryMethodStandard();
                System.Console.WriteLine("tipo standard");
                b = FatctoryMethodStandard.SommaConDecimali(5.35M, 5.55M);
                System.Console.WriteLine(b.ToString());
                FatctoryMethodStandard c = FatctoryMethodStandard.SommaConStringhe("3", "2");
                System.Console.WriteLine(c.ToString());
                //all'interno del builder viene comuque usato uno stringbuilder per generare la stringa, è tutto nascosto però all'utilizzatore delaa calsse BuilderStandard
                break;
            case Tipo.Asynchronus:
                /*
                    I costruttori non possono essere asincroni, i metodi si.
                    Rendo privato il costruttore e il metodo asincrono, poi creo un metodo asincrono che al termine restituisce tutto l'oggetto (privato).
                    Faccio un metodo statico pubblico che fa il NEW su se stesso e poi ritorna il risultato del metodo asincrono (l'oggetto, se stesso, instanziato)
                */
                break;
            case Tipo.StandardWithClass:
                /*
                    Tenendo in mente il principio di separazione delle responsabilità, si può separare la Factory dalla sua reale funzionalità.
                    I metodi statici dell'esempio standard vengono spostati in una classe dedicata.
                    L'inconveniente è far restare pubblico il costruttore della classe originale.
                */
                b = FatctoryClass.SommaConStringhe("4", "3");
                System.Console.WriteLine(b.ToString());
                break;
            case Tipo.TrackAndWeakReference:
                /*
                    Usando le Factories posso creare gli oggetti all'ingrosso, e tenere traccia dentro la factory di quanti oggetti sono stati creati.
                    Posso ciclare gli oggetti dentro la lista per apportare modifiche o altro, tutti gli oggetti creati sono centralizzati dentro la lista della factory.
                    Posso Wrappare (creare un involucro) la lista dentro un oggetto WEAKREFERENCE, in modo da non prolungare la durata dell'oggetto costruito perchè altrimenti
                    gli oggetti vivranno per tutto il tempo in cui vive la fabbrica, effetto che potrebbe non essere necessario perchè in qualche modo stai interferendo con il Garbage Collector.
                    WEAK REFERENCE https://www.codeproject.com/Articles/664282/Understanding-weak-references-in-NET
                */
                CreateIngrossoFactory factory = new CreateIngrossoFactory();
                b = factory.CreateSommaClass(7, 6);
                System.Console.WriteLine(b.ToString());
                break;
            case Tipo.StandardInternal:
                //Per rendere il costruttore  della classe FatctoryMethodWithClass privato e mantenere l'accesso alla factory, devo inserire la classe factory dentro la classe FatctoryMethodWithClass, 
                //in modo che la classe factory veda il costruttore privato FatctoryMethodWithClass, come nell'esempio seguente:
                // b = FatctoryMethodWithClass.FatctoryClass.SommaConStringhe("4", "3");
                //In alcuni casi posso anche usare l'accesso Internal per rendere visibile solo all'interno della classe il costruttore, in modo da essere visto solo da dentro l'Assembly
                //Un oggetto statico non muta nessuno stato all'interno, classe immutabile
                //Vedi anche le Factory Property
                //public static Punto Origine => new Punto(x,y); // getter, instanzia tutte le volte che chiamo il getter un nuovo Punto
                //public static Punto Origine = new Punto(x,y);  // restituisce sempre lo stesso punto
                break;
        }
    }
}


internal class FatctoryMethodStandard : IFactory
{
    private int somma;
    public int Somma
    {
        get { return somma; }
        set { somma = value; }
    }
    public static FatctoryMethodStandard SommaConStringhe(string a, string b)
    {
        int A = Convert.ToInt32(a);
        int B = Convert.ToInt32(b);
        return new FatctoryMethodStandard(A, B);
    }
    public static FatctoryMethodStandard SommaConDecimali(decimal a, decimal b)
    {
        int A = Convert.ToInt32(Math.Round(a));
        int B = Convert.ToInt32(Math.Round(b));
        return new FatctoryMethodStandard(A, B);
    }
    public FatctoryMethodStandard(int a, int b)
    {
        this.somma = a + b;
    }
    public override string ToString()
    {
        return "La somma calcolata è: " + somma.ToString();
    }
}

internal class FatctoryMethodWithClass : IFactory
{
    private int somma;
    public int Somma
    {
        get { return somma; }
        set { somma = value; }
    }
    public FatctoryMethodWithClass(int a, int b)
    {
        this.somma = a + b;
    }
    public override string ToString()
    {
        return "La somma calcolata è: " + somma.ToString();
    }
}
internal static class FatctoryClass
{
    public static FatctoryMethodWithClass SommaConStringhe(string a, string b)
    {
        int A = Convert.ToInt32(a);
        int B = Convert.ToInt32(b);
        return new FatctoryMethodWithClass(A, B);
    }
    public static FatctoryMethodWithClass SommaConDecimali(decimal a, decimal b)
    {
        int A = Convert.ToInt32(Math.Round(a));
        int B = Convert.ToInt32(Math.Round(b));
        return new FatctoryMethodWithClass(A, B);
    }
}


internal class CreateIngrossoFactory
{
    private readonly List<IFactory> lista = new List<IFactory>();

    public IFactory CreateSommaClass(int a, int b)
    {
        IFactory classe = new FatctoryMethodWithClass(a, b);
        lista.Add(classe);
        return classe;
    }
}


interface IFactory { }