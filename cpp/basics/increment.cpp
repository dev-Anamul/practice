#include <iostream>
using namespace std;

int main()
{

    int count = 10, val = 10;
    int res = val * ++count;

    cout << "Pre-increment: " << res << endl;

    res = val * count++;
    cout << "Post-increment: " << res << endl;

    return 0;
}