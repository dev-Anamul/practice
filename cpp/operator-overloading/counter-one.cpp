#include <iostream>
using namespace std;

class Counter
{
private:
    unsigned int count;

public:
    Counter() : count(0) {}

    unsigned int getCount() const
    {
        return count;
    }

    void operator++()
    {
        ++count;
    }
};

int main()
{
    Counter c1, c2;

    cout << "C1: " << c1.getCount() << endl;
    cout << "C2: " << c2.getCount() << endl;

    ++c1;
    ++c2;
    ++c2;

    cout << "C1: " << c1.getCount() << endl;
    cout << "C2: " << c2.getCount() << endl;

    return 0;
}