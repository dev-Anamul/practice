#include <iostream>
using namespace std;

int main()
{
    int fTemp;

    cin >> fTemp;

    int cTemp = (fTemp - 32) * 5 / 9;

    cout << "Celsius temperature is: " << cTemp;

    return 0;
}