namespace CarLibrary
{
    public class Car(string make, string model, int year, string color, double price)
    {
        public string Make { get; set; } = make;
        public string Model { get; set; } = model;
        public int Year { get; set; } = year;
        public string Color { get; set; } = color;
        public double Price { get; set; } = price;

        public override string ToString()
        {
            return $"Make: {Make}, Model: {Model}, Year: {Year}, Color: {Color}, Price: {Price}";
        }
    }
}