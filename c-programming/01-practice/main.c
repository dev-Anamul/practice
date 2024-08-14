#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#define PI 3.14159
#define MY_NAME "John Doe"

int main()
{
    const int AGE = 30;
    printf("My name is %s\n", MY_NAME);
    printf("My age is %d\n", AGE);

    const char MY_NAME2[] = "Jane Doe";
    printf("My name is %s\n", MY_NAME2);
    printf("My favorite programming language is C\n");
    printf("My favorite number is %f\n", 7.7 + 8.8);
    printf("The power of 2 is %f\n", pow(2, 2));

    return 0;
}