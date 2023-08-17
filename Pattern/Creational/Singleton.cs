class Singleton
{
    public enum Tipo
    {
        Standard
    }
    public Singleton(Tipo tipo)
    {
        System.Console.Write("Singleton > ");
        ISingleton b;
        switch (tipo)
        {
            case Tipo.Standard:
                System.Console.WriteLine("tipo standard");
                break;
        }
    }
}


public class SingletonDb : IDatabase{
    private Dictionary<string , string> listaValori = new Dictionary<string, string>();
    public SingletonDb()
    {
        
    }

    public int GetValore(string filtro)
    {
        throw new NotImplementedException();
    }
}



public interface IDatabase
{
    int GetValore(string filtro);
}

interface ISingleton { }