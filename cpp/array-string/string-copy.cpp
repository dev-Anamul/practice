#include <iostream>
#include <cstring>

using namespace std;

int main()
{

    char str1[] = "Oh,Captain, my captain! "
                  "our fearful trip is done";

    const int max = 80;
    char str2[max];
    int i;

    for (i = 0; i < strlen(str1); i++)
        str2[i] = str1[i];
    str2[i] = '\0';

    cout << str2 << endl;

    char st1[] = "Tiger, tiger, burning bright\n"
                 "In the forests of the night";

    const int max2 = 80;
    char st2[max2];

    strcpy(st2, st1);

    cout << st2 << endl;

    return 0;
}