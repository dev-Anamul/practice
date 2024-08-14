#include <iostream>
#include <string.h>
using namespace std;

class String
{
private:
    static const int SIZE = 120;
    char str[SIZE];

public:
    String()
    {
        str[0] = '\0';
    }

    String(char s[])
    {
        strcpy(str, s);
    }

    void display() const
    {
        cout << str << endl;
    }

    operator char *()
    {
        return str;
    }
};

int main()
{

    String s1;
    char xstr[] = "Hello string conversion testing.";
    s1 = xstr;
    s1.display();

    String s2 = "My name is Anamul Haque.";
    cout << static_cast<char *>(s2) << endl;

    return 0;
}