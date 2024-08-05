using System.Collections.Generic;

public class FishData
{
    public float Size = 0f;
    public float Speed = 0f;
    public float TotalValue = 0f;
    // 이 값을 넘어가면 조금씩 오르도록 하자
    public float MaxGrowValue = 100f;
    public float DiscountValue = 0.01f;

    public void EatFood(Food food) 
    {
        AddValue(food.GetValue());
        food.EatThis();
    }

    private void AddValue(float value)
    {
        float sum = TotalValue + value;

        if (sum < MaxGrowValue)
        {
            TotalValue = sum;
        }
        else
        {
            float discount = sum - MaxGrowValue;

            if (discount > value)
            {
                discount = value;
            }

            TotalValue = sum - discount + discount * DiscountValue;
        }
    }

    public bool IsMaxGrow()
    {
        return TotalValue > MaxGrowValue;
    }
}
