using System.Text;

class Builder
{
    public enum Tipo { Standard, Fluent, StepByStep, Faceted }
    public Builder(Tipo tipo)
    {
        /*
            OBIETTIVO: creare un generatore di xml indentanto usando nella costruzione poche istruzione.
            Si potrebbe chiamare n volte per ogni nodo un append dall'esterno, invece demandiamo tutto all'interno della classe
            Versione classica di un builder usato esternamente:
            // if you want to build a simple HTML paragraph using StringBuilder
            var hello = "hello";
            var sb = new StringBuilder();
            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("</p>");
            WriteLine(sb);
        */
        System.Console.Write("Builder > ");
        IBuilder b;
        switch (tipo)
        {
            case Tipo.Standard:
                b = new BuilderStandard();
                //all'interno del builder viene comuque usato uno stringbuilder per generare la stringa, è tutto nascosto però all'utilizzatore delaa calsse BuilderStandard
                break;
            case Tipo.Fluent:
                b = new BuilderFluent();
                // il builder fluent usa un approcio FLUENT INTERFACE, consente di fare più chiamate continuative, restituendo sempre un riferimento all'oggetto che si sta lavorando
                break;
            case Tipo.StepByStep:
                // il builder fluent usa un approcio FLUENT INTERFACE, consente di fare più chiamate continuative, restituendo sempre un riferimento all'oggetto che si sta lavorando
                //FUNZIONA var fattura = FatturaBuilder.Create().Livello1(Livello1.FatturaElettronicaHeader).Build();
                break;
            case Tipo.Faceted:
                b = new FatturaBuilder();
                Fattura fat = ((FatturaBuilder)b).FatturaElettronicaHeader
                                                    .DatiTrasmissione
                                                        .IdTrasmittente
                                                            .IdPaese("")
                                                            .IdCodice("")
                                                        .ProgressivoInvio("")
                                                        .PecDestinatario("");
                                                            
                                                    
                                                            
                                                        


                break;
        }
    }
}/*
idee di sviluppo controllo centralizzato dei valori obbligatori
controllo di almeno un valore
gestione degli array per multi componenti
*/
internal class BuilderStandard : IBuilder
{
    public BuilderStandard()
    {
        System.Console.WriteLine("tipo standard");
        XmlBuilder xBuilder = new XmlBuilder("RADICE");
        xBuilder.AddChild("NODO1", "valore1");
        xBuilder.AddChild("NODO2", "valore2");//.AddChild("SottoNODO2", "valoresottonodo2");
        Console.WriteLine(xBuilder);
    }
}

internal class BuilderFluent : IBuilder
{
    public BuilderFluent()
    {
        System.Console.WriteLine("tipo fluent");
        XmlBuilder xBuilder = new XmlBuilder("RADICE");
        xBuilder.AddChildFluent("NODO1", "valore1").AddChildFluent("NODO2", "valore2");
        Console.WriteLine(xBuilder);
    }
}
interface IBuilder { }

/*ENTITA' DI ELEMENTO CHE CONTIENE LA LOGICA A LIVELLO BASE*/
class XmlElement
{
    public string NomeNodo;
    public string Valore;
    public List<XmlElement> Elementi = new List<XmlElement>();
    private const int spaziIndentazione = 2;
    public XmlElement()
    {

    }
    //ad ogni oggetto che costruisco assegno un nome del nodo e un valore
    public XmlElement(string nomeNodo, string valore)
    {
        NomeNodo = nomeNodo;
        Valore = valore;
    }
    private string ToStringOverride(int indentazione)
    {
        StringBuilder sb = new StringBuilder();
        string spazi = new string(' ', spaziIndentazione * indentazione);
        //sb.Append($"{spazi}<{NomeNodo}>");
        sb.Append($"<{NomeNodo}>");
        if (string.IsNullOrWhiteSpace(Valore) == false)
        {
            //sb.Append(new String(' ', spaziIndentazione * (indentazione + 1)));
            sb.Append(Valore + "*");
            //sb.Append(System.Environment.NewLine);
        }
        foreach (var e in Elementi)
        {
            // sb.Append(e.ToStringOverride(indentazione + 1));
            sb.Append(e.ToStringOverride(0));
        }
        //sb.Append($"{spazi}</{NomeNodo}>\n");
        sb.Append($"{spazi}</{NomeNodo}>");
        return sb.ToString();
    }
    public override string ToString()
    {
        return this.ToStringOverride(0); //indentazione di partenza
    }
}
class XmlBuilder

{
    XmlElement radiceXml = new XmlElement();
    private readonly string radice;
    public XmlBuilder(string nomeRadice)
    {
        this.radice = nomeRadice;
        radiceXml.NomeNodo = nomeRadice;
    }
    public XmlBuilder AddChildFluent(string childName, string childText)
    {
        var e = new XmlElement(childName, childText);
        radiceXml.Elementi.Add(e);
        return this;
    }
    public void AddChild(string childName, string childText)
    {
        var e = new XmlElement(childName, childText);
        radiceXml.Elementi.Add(e);
    }
    public override string ToString()
    {
        //chiamo la funzione interna all'oggetto base
        return radiceXml.ToString();
    }
    public void Clear()
    {
        radiceXml = new XmlElement { NomeNodo = radice };
    }
}

//////
/////Composizione step by step della fattura elettronica
//esemplificazion
//LIVELLO 1 BLU     FatturaEletronicaHeader                 
//LIVELLO 2 VIOLA   DatiTrasmissione - CedentePrestatore    
//LIVELLO 3 ROSA    CodiceDestinatario - PECDestinatario (facoltativo) - DatiAnagrafici
//LIVELLO 4 SENAPE  IdFiscaleIva - Anagrafica - DataIscrizioneAlbo (facoltativo)
//LIVELLO 6 GIALLO  IdPaese - IdCodice
#region "STEP BY STEP"
/*
public enum Livello1
{
    FatturaElettronicaHeader, FatturaElettronicaBody
};

public interface IBuildFattura
{
    public Fattura Build();
}
public interface ISpecLev1
{
    public IBuildFattura Livello1(Livello1 lev1);
}
public class Fattura
{
    public Livello1 Lev1;
}
public class FatturaBuilder : IBuilder
{
    public static ISpecLev1 Create()
    {
        return new Impl();
    }

    private class Impl :
      ISpecLev1,
      IBuildFattura
    {
        private Fattura fattura = new Fattura();

        // public ISpecifyWheelSize OfType(CarType type)
        // {
        //     car.Type = type;
        //     return this;
        // }

        // public IBuildCar WithWheels(int size)
        // {
        //     switch (car.Type)
        //     {
        //         case CarType.Crossover when size < 17 || size > 20:
        //         case CarType.Sedan when size < 15 || size > 17:
        //             throw new ArgumentException($"Wrong size of wheel for {car.Type}.");
        //     }
        //     car.WheelSize = size;
        //     return this;
        // }

        public Fattura Build()
        {
            return fattura;
        }

        public IBuildFattura Livello1(Livello1 _lev1)
        {
            fattura.Lev1 = _lev1;
            return this;
        }
    }
}
*/
#endregion

#region  "FACETED BUILDER"
public class Fattura
{
    public string ProgressivoInvio { get; internal set; }
    public string FormatoTrasmissione { get; internal set; }
    public string IdPaese { get; internal set; }
    public string IdCodice { get; internal set; }
    public string RiferimentoAmministrativo { get; internal set; }
    public string PecDestinatario { get; internal set; }
}
public class FatturaBuilder : IBuilder
{
    // the object we're going to build
    protected Fattura fattura = new Fattura(); // this is a reference!

    public FatturaElettronicaHeaderBuilder FatturaElettronicaHeader => new FatturaElettronicaHeaderBuilder(fattura);
    public FatturaElettronicaBodyBuilder FatturaElettronicaBody => new FatturaElettronicaBodyBuilder(fattura);

    public static implicit operator Fattura(FatturaBuilder pb)
    {
        return pb.fattura;
    }
}
public class FatturaElettronicaHeaderBuilder : FatturaBuilder
{
    public FatturaElettronicaHeaderBuilder(Fattura fattura)
    {
        this.fattura = fattura;
    }
    public DatiTrasmissioneBuilder DatiTrasmissione => new DatiTrasmissioneBuilder(fattura);
    public CedentePrestatoreBuilder CedentePrestatore => new CedentePrestatoreBuilder(fattura);
}
public class FatturaElettronicaBodyBuilder : FatturaBuilder
{
    public FatturaElettronicaBodyBuilder(Fattura person)
    {
        this.fattura = person;
    }
    public FatturaElettronicaBodyBuilder Test1(string progressivoInvio)
    {
        fattura.ProgressivoInvio = progressivoInvio;
        return this;
    }
    
}

public class DatiTrasmissioneBuilder : FatturaBuilder
{
    public DatiTrasmissioneBuilder(Fattura fattura)
    {
        this.fattura = fattura;
    }
    public DatiTrasmissioneBuilder ProgressivoInvio(string progressivoInvio)
    {
        fattura.ProgressivoInvio = progressivoInvio;
        return this;
    }
    public DatiTrasmissioneBuilder FormatoTrasmissione(string formatoTrasmissione)
    {
        fattura.FormatoTrasmissione = formatoTrasmissione;
        return this;
    }
    public FatturaElettronicaHeaderBuilder PecDestinatario(string value)
    {
        fattura.PecDestinatario = value;
        return this.FatturaElettronicaHeader;
    }
    public IdTrasmissioneBuilder IdTrasmittente => new IdTrasmissioneBuilder(fattura);
}

public class IdTrasmissioneBuilder : FatturaBuilder
{
    public IdTrasmissioneBuilder(Fattura fattura)
    {
        this.fattura = fattura;
        // this.levelup = _levelup;
    }
    public IdTrasmissioneBuilder IdPaese(string val)
    {
        fattura.IdPaese = val;
        return this;
    }
    public DatiTrasmissioneBuilder IdCodice(string val)
    {
        fattura.IdCodice = val;
        return this.FatturaElettronicaHeader.DatiTrasmissione;
    }
}

public class CedentePrestatoreBuilder : FatturaBuilder
{
    public CedentePrestatoreBuilder(Fattura fattura)
    {
        this.fattura = fattura;
    }
    public CedentePrestatoreBuilder RiferimentoAmministrativo(string val)
    {
        fattura.RiferimentoAmministrativo = val;
        return this;
    }
}

#endregion