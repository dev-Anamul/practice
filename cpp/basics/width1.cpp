#include <iostream>
#include <iomanip>
using namespace std;

int main()
{

    long pop1 = 2425785, pop2 = 2323232, pop3 = 345652324;

    cout << "LOCATION " << "POP. " << endl
         << "Port city " << pop1 << endl
         << "High town " << pop2 << endl
         << "Low ville " << pop3 << endl;

    cout << "\n";

    cout << setw(8) << "LOCATION" << setw(12) << "POPULATION" << endl
         << setw(8) << "Portcity" << setw(12) << pop1 << endl
         << setw(8) << "Hightown" << setw(12) << pop2 << endl
         << setw(8) << "Lowville" << setw(12) << pop3 << endl;

    return 0;
}