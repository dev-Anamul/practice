#include <iostream>
using namespace std;

const int SIZE = 4;

struct part
{
    int modelNumber;
    int partNumber;
    int cost;
};

int main()
{

    part apart[SIZE];

    for (int i = 0; i < SIZE; i++)
    {
        cin >> apart[i].modelNumber >> apart[i].partNumber >> apart[i].cost;
    }

    for (int i = 0; i < SIZE; i++)
    {
        cout << "Model: " << apart[i].modelNumber << " Part: " << apart[i].partNumber << " Cost: " << apart[i].cost << endl;
    }

    return 0;
}