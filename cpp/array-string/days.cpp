#include <iostream>
using namespace std;

int main()
{
    int month, day, totalDays;
    int daysPerMonth[] = {31, 28, 31, 30, 31, 30,
                          31, 31, 30, 31, 30, 31};

    // take the input
    cin >> month >> day;

    // count the days
    totalDays = day;
    for (int i = 0; i < month - 1; i++)
    {
        totalDays += daysPerMonth[i];
    }

    // show the result in screen
    cout << "Total Days: " << totalDays;

    return 0;
}