#include <iostream>
using namespace std;

class Counter
{
private:
    unsigned int count;

public:
    Counter() : count(0) {}
    Counter(int c) : count(c) {}

    unsigned int getCount() const
    {
        return count;
    }

    Counter operator++()
    {
        ++count;
        return Counter(count);
    }
};

int main()
{

    Counter c1, c2;

    cout << "Count One: " << c1.getCount() << endl;
    cout << "Count two: " << c2.getCount() << endl;

    ++c1;
    c2 = ++c1;

    cout << "Count One: " << c1.getCount() << endl;
    cout << "Count two: " << c2.getCount() << endl;

    return 0;
}