using Product;
using ConcreateProduct;
using CreatorNamespace;

namespace ConcreateCreator
{
    public class ConcreateCreatorOne : Creator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreateProductOne();
        }
    }

    public class ConcreateCreatorTwo : Creator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreateProductTwo();
        }
    }

    public class ConcreateCreatorThree : Creator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreateProductThree();
        }
    }
}