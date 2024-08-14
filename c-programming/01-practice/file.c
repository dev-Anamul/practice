#include <stdio.h>
#include <math.h>
#include <stdlib.h>

int main()
{

    FILE *fpointer = fopen("employees.txt", "w");

    fprintf(fpointer, "\nSue, Human Resources\nShaniqua,IT\nJohn, Sales\nJane, Marketing\n");

    fclose(fpointer);

    return 0;
}