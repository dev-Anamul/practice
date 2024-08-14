#include <iostream>
using namespace std;

struct Distance
{
    int feet;
    float inches;
};

void scale(Distance &, float);
void printStruc(Distance);

int main()
{

    Distance d1 = {12, 6.5};
    Distance d2 = {10, 5.5};

    scale(d1, 0.5);
    scale(d2, 0.25);

    printStruc(d1);
    printStruc(d2);

    return 0;
}

void scale(Distance &d, float inch)
{
    d.inches += inch;

    while (d.inches >= 12)
    {
        d.feet++;
        d.inches -= 12;
    }
}

void printStruc(Distance distance)
{
    cout << "Feet: " << distance.feet << " Inch: " << distance.inches << endl;
}