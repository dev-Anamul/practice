#include <iostream>
using namespace std;

void repChar(char = '*', int = 25);

int main()
{
    repChar();
    repChar('=');
    repChar('+', 27);

    return 0;
}

void repChar(char ch, int n)
{
    for (int i = 0; i < n; i++)
        cout << ch;
    cout << endl;
}