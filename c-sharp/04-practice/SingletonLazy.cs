namespace SingletonLazy
{
    public class Singleton
    {
        private static Singleton? instance;
        private Singleton()
        {
        }

        public static Singleton GetInstance()
        {
            instance ??= new Singleton();
            return instance;
        }

        public void PrintMessage()
        {
            Console.WriteLine("Hello World!");
        }
    }
}