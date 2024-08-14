#include <iostream>
using namespace std;

class Stack
{
private:
    static const int SIZE = 10;
    int arr[SIZE];
    int top;

public:
    Stack() : top(-1)
    {
    }

    void push(int val)
    {
        if (top < SIZE - 1)
            arr[++top] = val;
        else
            cout << "Stack overflow." << endl;
    }

    int pop()
    {
        return arr[top--];
    }
};

int main()
{

    Stack s1;

    s1.push(11);
    s1.push(22);

    cout << "1: " << s1.pop() << endl;
    cout << "2: " << s1.pop() << endl;

    s1.push(33);
    s1.push(44);
    s1.push(55);
    s1.push(66);
    s1.push(77);
    s1.push(77);
    s1.push(77);
    s1.push(77);
    s1.push(77);
    s1.push(77);

    cout << "3: " << s1.pop() << endl;
    cout << "4: " << s1.pop() << endl;
    cout << "5: " << s1.pop() << endl;
    cout << "6: " << s1.pop() << endl;
    cout << "7: " << s1.pop() << endl;

    s1.push(88);

    cout << "8: " << s1.pop() << endl;

    return 0;
}