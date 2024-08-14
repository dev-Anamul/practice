#include <iostream>
using namespace std;

int main()
{

    int strLen = 200;
    char str[strLen];

    cin.get(str, strLen, '$');

    cout << "Your Entered: " << str << endl;

    return 0;
}