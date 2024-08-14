#include <iostream>
using namespace std;

class Part
{
private:
    int modelNumber;
    int partNumber;
    float cost;

public:
    Part() : modelNumber(10000), partNumber(200), cost(0.00)
    {
        cout << "Calling default" << endl;
    }

    Part(int modelNum, int partNum, float cs)
    {
        this->modelNumber = modelNum;
        this->partNumber = partNum;
        this->cost = cs;
    }

    int GetModelNumber() const
    {
        return this->modelNumber;
    }

    void SetModelNumber(int modelNumber)
    {
        this->modelNumber = modelNumber;
    }

    int GetPartNumber() const
    {
        return this->partNumber;
    }

    void SetPartNumber(int partNumber)
    {
        this->partNumber = partNumber;
    }

    float GetCost() const
    {
        return this->cost;
    }

    void SetCost(float cost)
    {
        this->cost = cost;
    }

    void showPart()
    {
        cout << "Model: " << this->modelNumber << " Part: " << this->partNumber << " Cost: " << this->cost << endl;
    }
};

int main()
{

    Part part1, part2;
    Part part3(434334, 3424, 23423.00);

    part1.SetModelNumber(42342);
    part1.SetPartNumber(423436);
    part1.SetCost(4532.00);
    part1.showPart();

    part2.showPart();
    part3.showPart();

    return 0;
}