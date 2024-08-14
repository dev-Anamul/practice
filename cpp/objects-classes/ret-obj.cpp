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
        // do nothing
    }

    Distance(int f, float inc) : feet(f), inches(inc)
    {
        // do nothing
    }

    void getDis()
    {
        cin >> feet >> inches;
    }

    void showDis()
    {
        cout << " Feet: " << feet << " INC: " << inches << endl;
    }

    Distance addDis(Distance);
};

Distance Distance::addDis(Distance dis)
{
    Distance temp;

    temp.inches = inches + dis.inches;

    if (temp.inches >= 12.0)
    {
        temp.inches -= 12.0;
        temp.feet++;
    }

    temp.feet += feet + dis.feet;
    return temp;
}

int main()
{

    Distance dis1, dis3;
    Distance dis2(11, 6.25);

    dis1.getDis();
    dis3 = dis1.addDis(dis2);

    dis1.showDis();
    dis2.showDis();
    dis3.showDis();

    return 0;
}