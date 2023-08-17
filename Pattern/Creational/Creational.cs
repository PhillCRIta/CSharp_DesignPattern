

class Creational : IPattern
{
    public enum TipoPatter { Builder, FactoryMethod, AbstracFactory, Prototype, Singleton };
    public Creational(TipoPatter TipoPatter)
    {
        System.Console.WriteLine("CREATIONAL PATTER");
        switch (TipoPatter)
        {
            case TipoPatter.Builder:
                /*
                    SEMPLIFICA LA COSTRUZIONE DI UN OGGETTO
                    Quando la costruzione di un oggetto pezzo per pezzo diventa molto complicata, fornisci una funzione che faccia la costruzione al posto tuo.
                    Es: costruttore con 10 argomenti non è buono, creazione di un oggetto molto cerimonioso
                    Il builder è un componente separato
                    E' una comodità per creare oggetti, costruire costruttori
                    Lo StringBuilder in c# implementa il builder
                    Posso farlo anche con modo fluente, un costruttore che restituisce se stesso, in modo da concatenare più costruttori in cascata
                */
                //Builder builder = new Builder(Builder.Tipo.Standard);
                // Builder builder = new Builder(Builder.Tipo.Fluent);
                Builder builder = new Builder(Builder.Tipo.Faceted);
                /*
                    E' un oggetto separato per costruire un nuovo oggetto, il costruttore diventa un componente autonomo, richiamato come funzione statica o getter statico.
                    Per fare un Builder fluente usare return this.
                    Facade usao per costruire diversi Builders che lavorano in tandem attraverso la classe base passata da un componente all'altro.
                */
                break;
            case TipoPatter.FactoryMethod:
                /*
                    DEMANDA LA COSTRUZIONE DI MOLTI OGGETTI A UN OGGETTO SEPARATO
                    Viene usato quando la logice per costruire un oggetto diventa troppo complicata, quando il costruttore non descrive in modo ottimale come costruire un oggetto:
                    - il nome del costruttore deve corrispondere al nome del tipo contenuto
                    - non è possibile fare l'overload con gli stessi argomenti così facendo inizi a usare dei parametri opzionali, difficilmente manutenibili
                    La creazione dell'oggetto può essere demandata al Factory Method, è esternalizzata:
                    - usando una funzione separata statica (Factory Method)
                    - è possibile usare la classe esterna per creare l'oggetto (Abstract Factory)
                    - in casi di grossi progetti è possibile avere gerarchie di Factory usando l'Abstrac Factory
                    Costruzione di oggetti all'ingrosso.
                    Modifiche all'ingrosso di oggetti creati con al Factory.
                    Usare sempre i costruttori non è una buona cosa, possono creare ambiguità, a esempio con stessi argomenti, o con lo stesso tipo di ritorno.
                    Posso usare le Factories, uso un metodo statico all'interno della classe che voglio instanziare che a sua volta chiama il costruttore e instanzia la classe.
                    Posso avere più nomi diversi di costruttore che chiamano quello "originale" della classe.
                    Posso fare gerarchie di fabbriche per creare più oggetti correlati
                */
                FatctoryMethod factory = new FatctoryMethod(FatctoryMethod.Tipo.TrackAndWeakReference);
                break;
            case TipoPatter.AbstracFactory:
                /*
                    DEMANDA LA COSTRUZIONE DI MOLTI OGGETTI A UN OGGETTO SEPARATO, SENZA CONOSCERE LA RELATIVA PARTE CONCRETA
                    Viene usato per DISTRUBIRE oggetti astratti al posto dei relativi concreti.
                    L'estrazione non ritorna i tipi che sto creando, invece restiuisce interfacce o classi astratte.
                */
                AbstractFactory absFactory = new AbstractFactory(AbstractFactory.Tipo.WithReflection);
                break;
            case TipoPatter.Prototype:
                /*
                    Il patter Prototype viene usato per prototipizzare oggetti / fare copie di oggetti.
                    Oggetti complicati in ingegneria del software vengono rimodellati di volta in volta, non vengono creati sempre da zero.
                    Per fare delle varianti a un oggetto è necessario fare una copia/clone di tale oggetto e personalizzarlo,
                    va fatta una copia profondo (Deep copy) / clonazione.
                    Il prototype è un oggetto completamente o parzialmente inizializzato su cui fai una copia/clone e poi lo usi per i tuoi scopi.
                    L'interfaccia C# IClonable non fa la copia profonda.
                    Esistono due tipologie di clonazione: Depp copy, Shallow copy. Copia profonda, copia superficiale.
                    Copia profonda > copia fisica dei valori in un nuovo riferimento
                    Copia superficiale > copia del riferimento
                    Una volta fatta la copia dell'oggetto puoi personalizzarla e usarla nel codice.
                */
                // Prototype prototype = new Prototype(Prototype.Tipo.CopyConstructor);
                // Prototype prototype = new Prototype(Prototype.Tipo.CopyInterface);
                // Prototype prototype = new Prototype(Prototype.Tipo.ProptotypeInheritance);
                Prototype prototype = new Prototype(Prototype.Tipo.PrototypeWithSerialization);
                break;
            case TipoPatter.Singleton:
            /*
                Usato per fornire un unico punto di accesso a una classe nel nostro sistema (per esempio, non ha senso avere più istanzhe che eseguono l'accesso al database).
                Un Factory dovrebbe avere una sola istanza, è inutile creare più volte la stessa factory per la creazione degli stessi oggetti.
                La costruzione di un oggetto è molto costosa, allora meglio optare per farla una sola volta e riusare la stessa instanza.
                Previene la creazione di oggetti duplicati non necessari.
                Usato per la creazione di oggetti pigri o thred-safe
                E' un componente che viene instanziato una sola volta e resiste all'idea di essere re instanziato più di una volta.
            */
                Singleton singleton = new Singleton(Singleton.Tipo.Standard);
                break;
            default:
                System.Console.WriteLine("Nessun tipo definito");
                break;

        }
    }
}