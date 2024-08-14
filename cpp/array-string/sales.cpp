#include <iostream>
using namespace std;

int main()
{

    const int SIZE = 6;
    double sales[SIZE];

    for (int i = 0; i < SIZE; i++)
    {
        cin >> sales[i];
    }

    double total = 0;

    for (int j = 0; j < SIZE; j++)
    {
        total += sales[j];
    }

    double avg = total / SIZE;

    cout << "Total = " << total << endl;
    cout << "Average = " << avg << endl;

    return 0;
}