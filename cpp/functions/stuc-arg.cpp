#include <iostream>
using namespace std;

struct Distance
{
    int feet;
    int inches;
};

void printStruc(Distance);

int main()
{

    Distance d1 = {12, 8};
    Distance d2 = {10, 9};

    printStruc(d1);
    printStruc(d2);

    return 0;
}

void printStruc(Distance d)
{
    cout << "Feet: " << d.feet << " Inches: " << d.inches;
}