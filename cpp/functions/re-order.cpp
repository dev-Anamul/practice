#include <iostream>
using namespace std;

void order(int &, int &);

int main()
{

    int num1 = 99, num2 = 11;
    int num3 = 22, num4 = 88;
    order(num1, num2);
    order(num3, num4);

    cout << "NUM 1: " << num1 << " NUM 2: " << num2 << endl;
    cout << "NUM 3: " << num3 << " NUM 4: " << num4 << endl;

    return 0;
}

void order(int &num1, int &num2)
{
    if (num1 > num2)
    {
        int temp = num1;
        num1 = num2;
        num2 = temp;
    }
}