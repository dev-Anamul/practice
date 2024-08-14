#include <iostream>
using namespace std;

int main()
{

    int chCount = 0;
    int wdCount = 1;
    char ch = 'a';

    while (ch != '\t')
    {
        ch = getchar();
        cout << ch << " ";

        if (ch == ' ')
            wdCount++;
        else
            chCount++;
    }

    cout << chCount << wdCount << endl;

    return 0;
}