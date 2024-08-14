#include <stdio.h>
#include <stdlib.h>
#include <string.h>

struct Person
{
    char name[50];
    int age;
    float height;
};

int main()
{
    struct Person person1;
    person1.age = 30;
    person1.height = 5.8;
    printf("Enter your name: ");
    scanf("%s", person1.name);
    printf("Your name is %s\n", person1.name);
    printf("Your age is %d\n", person1.age);
    printf("Your height is %f\n", person1.height);

    strcpy(person1.name, "John Doe");
    strcpy(person1.name, "Jane Doe 2");
    printf("Your name is %s\n", person1.name);

    return 0;
}