#include <iostream>
using namespace std;

class Distance
{
private:
    int feet;
    float inches;

public:
    Distance() : feet(0), inches(0.0)
    {
        cout << "Init" << endl;
    }

    Distance(int f, float inc) : feet(f), inches(inc)
    {
        cout << "Arg init" << endl;
    }

    void getDis()
    {
        cin >> feet >> inches;
    }

    void showDis()
    {
        cout << "Feet: " << feet << " INC: " << inches << endl;
    }

    void addDis(Distance, Distance);
};

void Distance::addDis(Distance d1, Distance d2)
{
    inches = d1.inches + d2.inches;
    feet = 0;

    if (inches >= 12.0)
    {
        inches -= 12.0;
        feet++;
    }

    feet += d1.feet + d2.feet;
}

int main()
{

    Distance dis1, dis3;

    Distance dis2(11, 6.25);
    Distance dis4(dis3);

    dis1.getDis();
    dis3.addDis(dis1, dis2);

    dis1.showDis();
    dis2.showDis();
    dis3.showDis();
    dis4.showDis();

    return 0;
}