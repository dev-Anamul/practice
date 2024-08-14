#include <iostream>
#include <ncurses.h>
using namespace std;

char ch = 'a';

void getAChar();
void putAChar();

int main()
{

    while (ch != '\r')
    {
        getAChar();
        putAChar();
        }

    return 0;
}

void getAChar()
{
    ch = getch();
}

void putAChar()
{
    cout << ch;
}