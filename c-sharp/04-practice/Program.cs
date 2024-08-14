
using ArrayLibrary;
using CalculatorLibrary;
using CarLibrary;
using ConcreateCreator;
using CreatorNamespace;
using EncryptionLibrary;
using SingletonLazy;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App: Launched with the ConcreteCreator1.");
            ClientCode(new ConcreateCreatorOne());

            Console.WriteLine();

            Console.WriteLine("App: Launched with the ConcreteCreator2.");
            ClientCode(new ConcreateCreatorTwo());

            Console.WriteLine();

            Console.WriteLine("App: Launched with the ConcreteCreator3.");
            ClientCode(new ConcreateCreatorThree());


        }

        public static void ClientCode(Creator creator)
        {
            Console.WriteLine("Client: I'm not aware of the creator's class, but it still works.\n" + creator.SomeOperation());
        }
    }
}
