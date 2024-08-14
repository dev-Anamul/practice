#include <iostream>
using namespace std;

int main()
{

    int age[4];

    for (int i = 0; i < 4; i++)
    {
        cin >> age[i];
    }

    for (int i = 0; i < 4; i++)
    {
        cout << age[i] << " ";
    }

    return 0;
}