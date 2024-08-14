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

    void getDis()
    {
        cin >> feet >> inches;
    }

    void showDis() const
    {
        cout << "Feet: " << feet << " Inc: " << inches << endl;
    }

    Distance operator+(Distance) const;
};

Distance Distance::operator+(Distance d1) const
{
    int f = feet + d1.feet;
    float inc = inches + d1.inches;

    while (inc >= 12.0)
    {
        inc -= 12.0;
        f++;
    }

    return Distance(f, inc);
}

int main()
{
    Distance dist1, dist3, dist4;
    dist1.getDis();

    Distance dist2(11, 6.25);

    dist3 = dist1 + dist2;
    dist4 = dist1 + dist2 + dist3;

    cout << "Dist 1: ";
    dist1.showDis();
    cout << "Dist 2: ";
    dist2.showDis();
    cout << "Dist 3: ";
    dist3.showDis();
    cout << "Dist 4: ";
    dist4.showDis();

    return 0;
}