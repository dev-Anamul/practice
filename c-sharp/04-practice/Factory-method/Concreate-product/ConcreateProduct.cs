using Product;

namespace ConcreateProduct
{
    public class ConcreateProductOne : IProduct
    {
        public string Operation()
        {
            return "{Result of ConcreateProduct One}";
        }
    }

    public class ConcreateProductTwo : IProduct
    {
        public string Operation()
        {
            return "{Result of ConcreateProduct Two}";
        }
    }

    public class ConcreateProductThree : IProduct
    {
        public string Operation()
        {
            return "{Result of ConcreateProduct Three}";
        }
    }
}