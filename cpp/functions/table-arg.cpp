#include <iostream>
using namespace std;

void repChar(char, int);

int main()
{
    repChar('-', 15);
    cout << "Some text inside the function call.";
    repChar('-', 15);

    return 0;
}

void repChar(char ch, int n)
{
    for (int i = 0; i < n; i++)
        cout << ch;

    cout << endl;
}