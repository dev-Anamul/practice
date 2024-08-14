#include <iostream>
using namespace std;

const int DISTRICTS = 4;
const int MONTHS = 3;

int main()
{

    int d, m;
    int sales[DISTRICTS][MONTHS];

    for (d = 0; d < DISTRICTS; d++)
    {
        for (m = 0; m < MONTHS; m++)
        {
            cin >> sales[d][m];
        }
    }

    for (d = 0; d < DISTRICTS; d++)
    {
        for (m = 0; m < MONTHS; m++)
        {
            cout << "District: " << d + 1 << " Month: " << m + 1 << " Sales: " << sales[d][m] << endl;
        }
    }

    return 0;
}