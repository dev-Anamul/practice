#include <iostream>
#include <string.h>

using namespace std;

class String
{
private:
    static const int max = 120;
    char str[max];

public:
    String()
    {
        strcpy(str, "");
    }

    String(char ch[])
    {
        strcpy(str, ch);
    }

    void display() const
    {
        cout << str << endl;
    }

    void getStr()
    {
        cin.get(str, max);
    }

    bool operator==(String ss) const
    {
        return strcmp(str, ss.str) == 0;
    }
};

int main()
{

    String s1 = "yes";
    String s2 = "no";
    String s3;

    s3.getStr();

    if (s3 == s1)
        cout << "You typed yes" << endl;
    else if (s3 == s2)
        cout << "You typed no" << endl;
    else
        cout << "You didn't follow instructions" << endl;

    return 0;
}