using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

class Prototype
{
    public enum Tipo
    {
        Standard, CopyConstructor,
        CopyInterface,
        ProptotypeInheritance,
        PrototypeWithSerialization
    }
    public Prototype(Tipo tipo)
    {
        System.Console.Write("Prototype > ");
        IPrototype b;
        switch (tipo)
        {
            case Tipo.Standard:
                System.Console.WriteLine("tipo standard");
                break;
            case Tipo.CopyConstructor:
                System.Console.WriteLine("tipo copy constructor");
                /*
					Copy COnstructor è un termine che deriva dal C++, specifica un oggetto e copia tutti i suoi dati.
				*/
                b = new Persona { Cognome = "Rossi", Nome = "Mario" };
                Persona c = ((Persona)b);
                System.Console.WriteLine(b.GetHashCode());
                System.Console.WriteLine(c.GetHashCode());
                Persona d = new Persona(c);
                //questo nuovo oggetto ha un HashCode diverso dai precedenti
                System.Console.WriteLine(d.GetHashCode());
                break;
            case Tipo.CopyInterface:
                System.Console.WriteLine("tipo copy interface");
                /*
					Uso un'interfaccia che definisce un metodo generico di Deep Copy.
					Questo approcio è molto cerimonioso perchè se ci sono molte classi e membri, bisogna a mano predisporre tutti i metodi di clonazione.
				*/
                b = new PersonaInterface { Cognome = "Rossi", Nome = "Mario" };
                PersonaInterface q = ((PersonaInterface)b);
                System.Console.WriteLine(b.GetHashCode());
                System.Console.WriteLine(q.GetHashCode());
                PersonaInterface w = q.CopiaProfonda();
                System.Console.WriteLine(w.GetHashCode());
                break;
            case Tipo.ProptotypeInheritance:
                System.Console.WriteLine("tipo prototype inheritance");
                /*
					La teoria di base sta nel fornire alle varie classi un metodo per fare una copia di se stesse dentro un qualche destinazione.
					Viene usata un'interfaccia con un metodo suo specifico interno, e un metodo pubblico da implementare in ogni classe che partecipa alla clonazione.
				*/
                Trattore x = new Trattore();
                x.Targa = "XYZ238X";
                x.Cilindrata = 1200;
                x.marca = Motore.ListaMarche.Lamborghini;
                x.Copertoni = new Copertoni { Larghezza = 225, Spalla = 5 };
                x.Modello = "1.6 TFSI";
                System.Console.WriteLine(x.ToString());
                System.Console.WriteLine(x.GetHashCode());
                //posso ESTRARRE la singola sotto classe in un oggetto singolo specificando il tipo nella funzione DeepCopy
                Motore motore = x.DeepCopy<Motore>();//qui viene usato il metodo DeepCopy dell'interfaccia
                Trattore nuovoTrattore = x.DeepCopy<Trattore>();//qui viene usato il metodo DeepCopy dell'interfaccia
                                                                //Copertoni copertone = x.DeepCopy<Copertoni>(); //NON POSSO ESTRARRE COPERTONI, PERCHE' E' UNA PROPRIETA', NON UNA CLASSE PARENTE
                System.Console.WriteLine(motore.GetHashCode());
                System.Console.WriteLine(nuovoTrattore.GetHashCode());
                Trattore y = x.DeepCopy(); // il metodo di estensione DeppCopy consente di fare la copia profonda della classe di partenza
                System.Console.WriteLine(y.GetHashCode());
                y.Copertoni = new Copertoni { Larghezza = 335, Spalla = 8 };
                System.Console.WriteLine(y.ToString());
                break;
            case Tipo.PrototypeWithSerialization:
                System.Console.WriteLine("tipo prototype with serialization");
                /*
					Nella casistica reale, una copia profonda viene fatta serializzando un oggetto (completo del suo albero), per poi deserializzarlo dentro un nuovo oggetto.
					Se uso il BinaryFormatter devo usere [SERIALIZABLE] come attributo, invece se uso XMLSerializer non serve l'attributo.
					L'XMLSerializer necessità di un costruttore senza parametri per ogni oggetto che stai serializzando.
					Uso un metodo di esentione per fare la copia.
				*/
                Trattore g = new Trattore();
                g.Targa = "XYZ238X";
                g.Cilindrata = 1200;
                g.marca = Motore.ListaMarche.Lamborghini;
                g.Copertoni = new Copertoni { Larghezza = 225, Spalla = 5 };
                g.Modello = "1.6 TFSI";
                System.Console.WriteLine(g.ToString());
                System.Console.WriteLine(g.GetHashCode());
                Trattore r = g.CopiaProfondaXml();
                r.Cilindrata = 5500;
                System.Console.WriteLine(r.ToString());
                System.Console.WriteLine(r.GetHashCode());
                break;
        }
    }
}


class Persona : IPrototype
{
    private string nome;
    public string Nome
    {
        get { return nome; }
        set { nome = value; }
    }
    private string cognome;
    public string Cognome
    {
        get { return cognome; }
        set { cognome = value; }
    }
    public Persona()
    {

    }
    //CONTSTRUCTOR COPY
    public Persona(Persona nuova)
    {
        this.cognome = nuova.cognome;
        this.nome = nuova.nome;
    }
}

class PersonaInterface : IPrototype, IPrototype<PersonaInterface>
{
    private string nome;
    public string Nome
    {
        get { return nome; }
        set { nome = value; }
    }
    private string cognome;
    public string Cognome
    {
        get { return cognome; }
        set { cognome = value; }
    }
    public PersonaInterface()
    {

    }
    public PersonaInterface(string nome, string cognome)
    {
        this.nome = nome;
        this.cognome = cognome;
    }
    public PersonaInterface(PersonaInterface nuova)
    {
        this.cognome = nuova.cognome;
        this.nome = nuova.nome;
    }
    public PersonaInterface CopiaProfonda()
    {
        return new PersonaInterface(Nome, Cognome);
    }
}

public interface ICopiaProfonda<T> where T : new()
{
    void CopyTo(T target);
    //per usare questo metodo ogni oggetto deve essere Castato su ICopiaProfonda
    public T DeepCopy()
    {
        T t = new T();
        CopyTo(t);
        return t;
    }
}
public class Motore : ICopiaProfonda<Motore>
{
    private string modello;
    public string Modello
    {
        get { return modello; }
        set { modello = value; }
    }
    private int cilindrata;
    public int Cilindrata
    {
        get { return cilindrata; }
        set { cilindrata = value; }
    }
    public enum ListaMarche { Lamborghini, Landini };
    public ListaMarche marca { get; set; }
    /* 
	dall'esterno chiamo questo metodo
	public T DeepCopy()
	{
		T t = new T();
		CopyTo(t);
		return t;
	}
	CopyTO viene chiamato all'interno dell'interfaccia nel metodo DeepCopy
	*/
    public void CopyTo(Motore target)
    {
        target.Modello = Modello;
        target.Cilindrata = target.Cilindrata;
    }
    public Motore() { }
    public override string ToString() { return $"Trattore: {modello} - Marca: {marca} - Cilindrata: {cilindrata}"; }
}
public class Trattore : Motore, IPrototype, ICopiaProfonda<Trattore>
{
    private string targa;
    public string Targa
    {
        get { return targa; }
        set { targa = value; }
    }
    public Copertoni Copertoni;
    public void CopyTo(Trattore target)
    {
        base.CopyTo(target);
        target.Targa = Targa;
        //questo metodo DeepCopy chiama l'extension method, perchè Copertoni non è un oggetto tipizzato ICopiaProfonda
        //per farlo funzionare potrei chiamarlo in questo modo ((ICopiaProfonda<Copertoni>) copertoni).DeepCopy();
        //per evitare di fare il cast creiamo un metodo di estensione
        //usiamo il metodo di estensione come una specie di cast particolare, passiamo l'oggetto Copertoni e lo castiamo a ICopiaProfonda, da li chiamiamo DeepCopy dell'interfaccia
        target.Copertoni = Copertoni.DeepCopy();
    }
    public Trattore() { }
    public override string ToString() { return $"{base.ToString()}, targa: {targa}, Copertoni: {Copertoni.ToString()}"; }
}
public class Copertoni : ICopiaProfonda<Copertoni>
{
    private int larghezza;
    public int Larghezza
    {
        get { return larghezza; }
        set { larghezza = value; }
    }
    private int spalla;
    public int Spalla
    {
        get { return spalla; }
        set { spalla = value; }
    }
    public override string ToString() { return $"Dimensione copertone: {larghezza} - {spalla}"; }
    public void CopyTo(Copertoni target)
    {
        target.larghezza = larghezza;
        target.spalla = spalla;
    }
    public Copertoni() { }
}

public static class DeepCopyExtensions
{
    public static T DeepCopy<T>(this ICopiaProfonda<T> item)
      where T : new()
    {
        return item.DeepCopy();
    }
    public static T DeepCopy<T>(this T motore)
      where T : Motore, new() // va indicata la classe base da cui tutto eredita
    {
        return ((ICopiaProfonda<T>)motore).DeepCopy();
    }
    //COPIA CON SERIALIZZATORE IL BINARY FORMATTER E' OBSOLETO, VA USATO L'XML FORMATTER
    // public static T CopiaProfonda<T>(this T self)
    // {
    //     MemoryStream stream = new MemoryStream();
    //     BinaryFormatter formatter = new BinaryFormatter();
    //     formatter.Serialize(stream, self);
    //     stream.Seek(0, SeekOrigin.Begin);
    //     object copyObj = formatter.Deserialize(stream);
    //     stream.Close();
    //     return (T)copyObj;
    // }
	public static T CopiaProfondaXml<T>(this T x)
    {
      using (var ms = new MemoryStream())
      {
        XmlSerializer s = new XmlSerializer(typeof(T));
        s.Serialize(ms, x);
        ms.Position = 0;
        return (T) s.Deserialize(ms);
      }
    }
}
interface IPrototype<T>
{
    T CopiaProfonda();
}

interface IPrototype { }