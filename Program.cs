using System.Diagnostics;

namespace PhillCRIta.github.io;
class Program
{
    static void Main(string[] args)
    {
        /*
            Usati per la creazione e costruzione di oggetti, in modo 
            ESPLICITO > costruttore
            IMPLITICO > dependency injection, reflection, altro meccanismo che crea l'ogetto dietro le quinte
            singola istruzione (in massa), step by step (pezzo-pezzo, istruzioni da fare prima della costruzione dell'oggetto)
        */
        Creational creational;
        /*
            Usati per la gestione della struttura delle classi e dei membri.
            Involucri che imitano la classe sottostante, va fatta attenzione a un buon design della funzionalità
        */
        /*
            Non hanno un tema centrale, sono tutti diversi, ci sono alcune sovrapposizioni per alcuni pattern, ma comunque sono unici nel loro approccio.
            Ricoprono in modo unico la risoluzione di un determinato problema.
        */
        //BUILDER
        // creational = new Creational(Creational.TipoPatter.Builder);
        //FACTORY METHOD
        creational = new Creational(Creational.TipoPatter.Prototype);

        //STOP
        Debug.WriteLine("");
    }
}

/*
IDEE AGGIUNTIVE, USARE UN BUILDER PER CREARE IL DICTIONARY DI UNA FATTURA??
*/


/*ypes in .Net can be either value types or reference types.

Value types -> enums, structs and built-in values types(bool, byte, short, int, long, sbyte, ushort, uint, ulong, char, double, decimal)

Reference types -> classes, interfaces, delegates, dynamic and strings

So, as you can see enums are types(like classes and structs, etc). more precisely they are value types. An important point about value types is that you should be able to create instances from them. For example, What is its benefit of int that is a struct (value type) if you can't create an instance of that for storing 2, 3 or any number in it?!

This is the general rule -> you cannot create custom value types (enums and structs) with the static modifier.

Some points:

If you write your enums or structs directly in a namespace they can not be marked as private or protected just like other types. They can be just public or internal just like other types.

If you write your enums or structs directly in a class you can mark them as private or protected too, as you can mark them as internal and public. class for inner types is like a namespace for types except you can mark inner types private or public too.*/