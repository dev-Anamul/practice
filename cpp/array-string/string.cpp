#include <iostream>
using namespace std;

int main()
{

    int strLen = 100;
    char str[strLen];

    cin.get(str, strLen);

    cout << "Your entered: " << str << endl;

    return 0;
}