#include <iostream>
using namespace std;

int main()
{
    float radius;
    const float PI = 3.14159;

    cin >> radius;

    float area = PI * radius * radius;

    cout << "Area is: " << area << endl;

    return 0;
}