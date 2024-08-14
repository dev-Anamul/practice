#include <iostream>
using namespace std;

int main()
{
    unsigned int num;
    unsigned long fact = 1;

    cin >> num;

    for (int i = num; i > 0; i--)
    {
        fact *= i;
    }

    cout << "Factorial is: " << fact << endl;

    return 0;
}