#include <stdio.h>
#include <math.h>
#include <stdlib.h>

int sum(int a, int b);

int main()
{
    int a, b;
    printf("Enter a number: ");
    scanf("%d", &a);

    printf("Enter another number: ");
    scanf("%d", &b);

    printf("The sum of %d and %d is %d\n", a, b, sum(a, b));

    return 0;
}

int sum(int a, int b)
{
    return a + b;
}