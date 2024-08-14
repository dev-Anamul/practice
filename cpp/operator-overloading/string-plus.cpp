#include <iostream>
#include <string.h>
#include <stdlib.h>

using namespace std;

class String
{
private:
    static const int MSIZE = 120;
    char str[MSIZE];

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

    String operator+(String ss) const;
};

String String::operator+(String ss) const
{
    String temp;
    if (strlen(str) + strlen(ss.str) <= MSIZE)
    {
        strcpy(temp.str, str);
        strcat(temp.str, ss.str);
    }
    else
    {
        cout << "String overflow" << endl;
    }

    return temp;
}

int main()
{

    String s1 = "EID Mubarak!";
    String s2 = "Happy new year";
    String s3;

    s1.display();
    s2.display();
    s3.display();

    s3 = s1 + s2;

    s3.display();

    return 0;
}