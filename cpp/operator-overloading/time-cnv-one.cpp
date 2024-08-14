#include <iostream>
#include <string>
using namespace std;

class Time12
{
private:
    bool pm;
    int hrs;
    int mins;

public:
    Time12() : pm(true), hrs(0), mins(0) {}
    Time12(bool ap, int h, int m) : pm(ap), hrs(h), mins(m) {}
    void display() const
    {
        string amPm = pm ? "p.m" : "a.m";
        cout << hrs << ":" << mins << " " << amPm << endl;
    }
};

class Time24
{
private:
    int hours;
    int minutes;
    int seconds;

public:
    Time24() : hours(0), minutes(0), seconds(0) {}
    Time24(int h, int m, int s) : hours(h), minutes(m), seconds(s) {}
    void display()
    {
        cout << hours << ":" << minutes << ":" << seconds << endl;
    }
    operator Time12() const;
};

Time24::operator Time12() const
{
    int hrs24 = hours;
    bool pm = hours < 12 ? false : true;
    int roundMin = seconds < 30 ? minutes : minutes + 1;
    if (roundMin == 60)
    {
        roundMin = 0;
        ++hrs24;
        if (hrs24 == 12 || hrs24 == 24)
            pm = (pm == true) ? false : true;
    }
    int hrs12 = (hrs24 < 13) ? hrs24 : hrs24 - 12;
    if (hrs12 == 0)
    {
        hrs12 = 12;
        pm = false;
    }

    return Time12(pm, hrs12, roundMin);
}

int main()
{
    int h, m, s;

    cin >> h >> m >> s;

    if (h > 23)
        return (1);

    Time24 t24(h, m, s);
    t24.display();

    Time12 t12 = t24;
    t12.display();

    return 0;
}