#include <iostream>
using namespace std;

class Distance
{
private:
    const float MTF;
    int feet;
    float inches;

public:
    Distance() : feet(0), inches(0.0), MTF(3.280833F) {}
    Distance(float meters) : MTF(3.280833F)
    {
        float mtToFeet = meters * MTF;
        feet = int(mtToFeet);
        inches = 12 * (mtToFeet - feet);
    }
    Distance(int f, float inc) : feet(f), inches(inc), MTF(3.280833F) {}
    void getDis()
    {
        cin >> feet >> inches;
    }

    void showDis() const
    {
        cout << "Feet: " << feet << " INC: " << inches << endl;
    }

    operator float() const
    {
        float fracFeet = inches / 12;
        fracFeet += static_cast<float>(feet);
        return fracFeet / MTF;
    }
};

int main()
{

    float mtrs;
    Distance dist1 = 2.35F;

    dist1.showDis();

    mtrs = static_cast<float>(dist1);
    cout << "MTRS: " << mtrs << endl;

    Distance dist2(5, 10.25);
    mtrs = dist2;

    cout << "MTRS: " << mtrs << endl;

    return 0;
}