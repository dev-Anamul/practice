#include <iostream>
using namespace std;

void repChar();
void repChar(char);
void repChar(char, int);

int main()
{
    repChar();
    repChar('=');
    repChar('-', 30);

    return 0;
}

void repChar()
{

    for (int i = 0; i < 20; i++)
        cout << "*";

    cout << endl;
}

void repChar(char ch)
{

    for (int i = 0; i < 20; i++)
        cout << ch;

    cout << endl;
}

void repChar(char ch, int n)
{

    for (int i = 0; i < n; i++)
        cout << ch;

    cout << endl;
}