#include <iostream>
using namespace std;

class Distance
{
private:
    int feet;
    float inches;

public:
    Distance() : feet(0), inches(0.0) {}
    Distance(int f, float i) : feet(f), inches(i) {}
    void getDis()
    {
        cin >> feet >> inches;
    }

    void showDis() const
    {
        cout << "Feet: " << feet << " INC: " << inches << endl;
    }

    bool operator<(Distance) const;
};

bool Distance::operator<(Distance d) const
{
    float thisF = feet + inches / 12;
    float thatF = d.feet + d.inches / 12;
    return thatF < thatF;
}

int main()
{
    Distance dist1;
    dist1.getDis();

    Distance dist2(6, 2.5);

    cout << "Dist 1: ";
    dist1.showDis();
    cout << "Dist 2: ";
    dist2.showDis();

    if (dist1 < dist2)
        cout << "Dis 1 is less thant dis 2" << endl;
    else
        cout << "Dis 2 is less than dis 1" << endl;

    return 0;
}