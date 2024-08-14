#include <iostream>
using namespace std;

enum days_of_week
{
    Sun,
    Mon,
    Tue,
    Wed,
    Thu,
    Fri,
    Sat
};

int main()
{

    days_of_week day1, day2;

    day1 = Mon;
    day2 = Thu;

    int diff = day1 - day2;

    cout << "Days between = " << diff << endl;

    if (day1 < day2)
        cout << "Day one comes before day two";

    return 0;
}