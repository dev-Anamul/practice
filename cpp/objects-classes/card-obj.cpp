#include <iostream>
using namespace std;

enum Suit
{
    clubs,
    diamonds,
    hearts,
    spades
};

const int jack = 11;
const int queen = 12;
const int king = 13;
const int ace = 14;

class Card
{
private:
    int number;
    Suit suit;

public:
    Card() {}
    Card(int n, Suit s) : number(n), suit(s) {}

    void display();
    bool isEqual(Card);
};

void Card::display()
{
    if (number >= 2 && number <= 10)
        cout << number << " of ";
    else
        switch (number)
        {
        case jack:
            cout << "jack of ";
            break;
        case queen:
            cout << " queen of ";
            break;
        case king:
            cout << " king of ";
            break;
        case ace:
            cout << " ace of ";
            break;
        }

    switch (suit)
    {
    case clubs:
        cout << "clubs";
        break;
    case diamonds:
        cout << "diamonds";
        break;
    case hearts:
        cout << "hearts";
        break;
    case spades:
        cout << "spades";
        break;
    }
}

bool Card::isEqual(Card c)
{
    return (number == c.number && suit == c.suit);
}

int main()
{
    Card temp, chosen, prize;
    int position;

    Card card1(7, clubs);
    cout << "\nCard 1 is the ";
    card1.display();

    Card card2(jack, hearts);
    cout << "\nCard 2 is the ";
    card2.display();

    Card card3(ace, spades);
    cout << "\nCard 3 is the ";
    card3.display();

    prize = card3;

    cout << "\nI am swapping card 1 and card 3";
    temp = card1;
    card1 = card3;
    card3 = temp;

    cout << "\nI am swapping card 2 and card 3";
    temp = card2;
    card2 = card3;
    card3 = temp;

    cout << "\nI am swapping card 1 and card 2";
    temp = card1;
    card1 = card2;
    card2 = temp;

    cout << "\nNow where (1, 2, or 3) is the ";
    prize.display();
    cout << " ? ";
    cin >> position;

    switch (position)
    {
    case 1:
        chosen = card1;
        break;
    case 2:
        chosen = card2;
        break;
    case 3:
        chosen = card3;
        break;
    }

    if (chosen.isEqual(prize))
        cout << "That's right! You win!";
    else
        cout << "Sorry. You lose.";

    cout << "You chose the ";
    chosen.display();

    cout << endl;

    return 0;
}