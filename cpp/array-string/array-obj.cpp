#include <iostream>
using namespace std;

class Distance
{
private:
    int feet;
    float inches;

public:
    void getDist()
    {
        cin >> feet >> inches;
    }

    void showDist() const
    {
        cout << "Feet: " << feet << " Inches: " << inches << endl;
    }
};

int main()
{
    int n = 5;
    Distance dis[n];

    for (int i = 0; i < n; i++)
    {
        dis[i].getDist();
    }

    for (int i = 0; i < n; i++)
    {
        dis[i].showDist();
    }

    return 0;
}