#include <iostream>
using namespace std;

class Distance
{
private:
    int feet;
    float inches;

public:
    Distance() : feet(0), inches(0.0) {}

    Distance(int f, float inc) : feet(f), inches(inc) {}

    void getDist()
    {
        cin >> feet >> inches;
    }

    void showDis() const
    {
        cout << "Feet: " << feet << " INC: " << inches << endl;
    }

    void operator+=(Distance);
};

void Distance::operator+=(Distance d)
{
    feet += d.feet;
    inches += d.inches;

    while (inches >= 12.0)
    {
        inches -= 12.0;
        feet++;
    }
}

int main()
{

    Distance dist1;
    dist1.getDist();
    dist1.showDis();

    Distance dist2(11, 6.25);
    dist2.showDis();

    dist1 += dist2;
    dist1.showDis();

    return 0;
}