namespace SingletonLibrary
{
    public class Singleton
    {
        private readonly static Singleton instance = new();
        private Singleton()
        {
        }

        public static Singleton GetInstance()
        {
            return instance;
        }

        public void PrintMessage()
        {
            Console.WriteLine("Hello World!");
        }
    }
}