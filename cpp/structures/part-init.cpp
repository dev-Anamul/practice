#include <iostream>
using namespace std;

struct part
{
    int modelNumber;
    int partNumber;
    int cost;
};

int main()
{

    part part1 = {6244, 373, 43247};

    cout << "MODEL: " << part1.modelNumber << " PART: " << part1.partNumber << " COST: " << part1.cost << endl;

    return 0;
}