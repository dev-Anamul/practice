#include <iostream>
#include <proc_service.h>
using namespace std;

class Stack
{
protected:
    static const int max = 5;
    int st[max];
    int top;

public:
    Stack() : top(-1) {}
    void push(int var)
    {
        st[++top] = var;
    }

    int pop()
    {
        return st[top--];
    }
};

class Stack2 : public Stack
{
public:
    void push(int var)
    {
        if (top >= max - 1)
        {
            cout << "Stack overflow";
            return;
        }
        Stack::push(var);
    }

    int pop()
    {
        if (top < 0)
        {
            cout << "Stack is empty";
           exit(1);
        }
        return Stack::pop();
    }
};

int main()
{

    Stack2 s1;

    s1.push(11);
    s1.push(22);
    s1.push(33);

    cout << s1.pop() << endl;
    cout << s1.pop() << endl;
    cout << s1.pop() << endl;
    cout << s1.pop() << endl;
    cout << endl;

    return 0;
}