#include <iostream>
#include <iomanip>
using namespace std;

int main()
{

    int j;

    for (j = 1; j <= 10; j++)
    {
        cout << setw(2) << j;
        int cube = j * j * j;
        cout << setw(6) << cube << endl;
    }

    return 0;
}