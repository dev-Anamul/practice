
namespace DataTypeLibrary
{
    class DataTypes
    {
        public static void DataTy(string[] args)
        {
            string name = "John";
            char grade = 'A';
            int age = 30;
            double gpa = 3.5;
            bool isMale = true;

            Console.WriteLine(name);
            Console.WriteLine(name.ToUpper());
            Console.WriteLine(grade);
            Console.WriteLine(age);
            Console.WriteLine(gpa);
            Console.WriteLine(isMale);
            Console.WriteLine(Math.Floor(144.75));
            Console.WriteLine(Math.Ceiling(144.75));
            Console.WriteLine("Hello World!");
        }
    }
}
