#include <iostream>
using namespace std;

class Counter
{
protected:
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
        return Counter(++count);
    }
};

class CountDn : public Counter
{
public:
    Counter operator--()
    {
        return Counter(--count);
    }
};

int main()
{

    CountDn c1;

    cout << "Before INC: " << c1.getCount() << endl;
    ++c1;
    ++c1;
    ++c1;

    cout << "After INC: " << c1.getCount() << endl;
    --c1;
    --c1;

    cout << "After DEC: " << c1.getCount() << endl;

    return 0;
}