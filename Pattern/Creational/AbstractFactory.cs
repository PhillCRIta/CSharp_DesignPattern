class AbstractFactory
{
    public enum Tipo { Standard, WithReflection }
    public AbstractFactory(Tipo tipo)
    {
        System.Console.Write("Abstract Factory > ");
        IAbstracFactory b;
        switch (tipo)
        {
            case Tipo.Standard:
                System.Console.WriteLine("tipo standard");
                b = new MachcinaPreparaCibo();
                /*  La variabile [b] diventa la Abstract Factory, che permette di creare N macchine di preparazione del cibo.
                    Queste factories restano trasparenti all'utente, e vengono attivate dinamicamente nell'assembly.
                    Usando Activator.GetInstance instanzio degli oggetti dinamicaemente in memoria in modo trasparente.
                    L'utente utilizzatore non conosce all'interno cosa succede.
                    Quando mi serve un cibo, non faccio altro che usare la factory [b], chiamando confenziona cibo che restituisce un NUOVO - NEW cibo che ho richiesto.
                    Aggiungo anche quante porzioni deve preparare come parametro.
                    Poi il nuovo oggetto creato dalla factory ha i suoi metodi e vive di vita propria.
                    Quello che viene instanziato dalla factory è trasparente rispetto alla sua reale implementazione, ogni metodo di ogni oggetto ha una sua implementazione dedicata.
                */
                ICiboPreparato cibo = ((MachcinaPreparaCibo)b).ConfezionaCibo(MachcinaPreparaCibo.CiboDisponibile.Pizza, 5);
                cibo.Mangiare(3);
                cibo.Mangiare(3);
                b = null;
                cibo.Mangiare(2);
                ICiboPreparato cibo2 = ((MachcinaPreparaCibo)b).ConfezionaCibo(MachcinaPreparaCibo.CiboDisponibile.Pizza, 5);
                break;
            case Tipo.WithReflection:
                /*
                    La versione standard presenta una limitazione al SOLID design, quello OPEN-CLOSE.
                    Se devo aggiungere una nuova tipologia di cibo, devo aprire la classe base e aggiungere all'Enumerazione.
                    Posso optare o usando un Container di Dependency Injection oppure usare una reflection sulle tipologie di Cibo, in modo da automatizzre l'Enumerazione.
                */
                 System.Console.WriteLine("tipo con Reflection");
                var c = new MachcinaPreparaCiboReflection();
                ICiboPreparato pizza = c.CreaCibo();
                pizza.Mangiare(2);
                break;
        }
    }
}

public interface ICiboPreparato
{
    void Mangiare(int porzioni);
}
internal class Pizza : ICiboPreparato
{
    public void Mangiare(int porzioni)
    {
        System.Console.WriteLine("Mangio " + porzioni + " fette di pizza");
    }
}
internal class Gelato : ICiboPreparato
{
    public void Mangiare(int porzioni)
    {
        System.Console.WriteLine("Mangio " + porzioni + " palle di gelato");
    }
}
public interface ICiboPreparatoFactory
{
    ICiboPreparato Prepara(int porzioni);
}
internal class GelatoFactory : ICiboPreparatoFactory
{
    public ICiboPreparato Prepara(int porzioni)
    {
        Console.WriteLine($"Preparo {porzioni} palle di Gelato");
        return new Gelato();
    }
}
internal class PizzaFactory : ICiboPreparatoFactory
{
    public ICiboPreparato Prepara(int porzioni)
    {
        Console.WriteLine($"Preparo {porzioni} fette di Pizza");
        return new Pizza();
    }
}
public class MachcinaPreparaCibo : IAbstracFactory
{
    public enum CiboDisponibile
    {
        Pizza, Gelato
    }
    private Dictionary<CiboDisponibile, ICiboPreparatoFactory> factories = new Dictionary<CiboDisponibile, ICiboPreparatoFactory>();
    public MachcinaPreparaCibo()
    {
        // var myType = typeof(MachcinaPreparaCibo);
        // var n = myType.Namespace;
        foreach (CiboDisponibile cibo in Enum.GetValues(typeof(CiboDisponibile)))
        {
            var factory = (ICiboPreparatoFactory)Activator.CreateInstance(
                    //Type.GetType("PhillCRIta.github.io." + Enum.GetName(typeof(CiboDisponibile), cibo) + "Factory")
                    Type.GetType(Enum.GetName(typeof(CiboDisponibile), cibo) + "Factory")
                    );
            factories.Add(cibo, factory);
        }
    }
    public ICiboPreparato ConfezionaCibo(CiboDisponibile cibo, int porzioni)
    {
        return factories[cibo].Prepara(porzioni);
    }
}

public class MachcinaPreparaCiboReflection
{
    private List<Tuple<string, ICiboPreparatoFactory>> factories = new List<Tuple<string, ICiboPreparatoFactory>>();
    public MachcinaPreparaCiboReflection()
    {
        foreach (var t in typeof(MachcinaPreparaCiboReflection).Assembly.GetTypes())
        {
            if (typeof(ICiboPreparatoFactory).IsAssignableFrom(t) && !t.IsInterface)
            {
                factories.Add(Tuple.Create(t.Name.Replace("Factory", string.Empty),
                (ICiboPreparatoFactory)Activator.CreateInstance(t)));
            }
        }
    }
    public ICiboPreparato CreaCibo()
    {
        System.Console.WriteLine("Seleziona il cibo:");
        for (var index = 0; index < factories.Count; index++)
        {
            var tuple = factories[index];
            System.Console.WriteLine($"{index}: {tuple.Item1}");
        }
        while (true)
        {
            string s, r;
            System.Console.Write("Specifica il cibo:");
            s = Console.ReadLine().ToString();
            System.Console.Write("Specifica le porzioni:");
            r = Console.ReadLine().ToString();
            return factories[Convert.ToInt32(s)].Item2.Prepara(Convert.ToInt32(r));
        }
    }
}

interface IAbstracFactory { }