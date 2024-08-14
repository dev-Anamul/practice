namespace UserInputLibraray
{
    public class UserInput
    {
        public static string GetUserInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}