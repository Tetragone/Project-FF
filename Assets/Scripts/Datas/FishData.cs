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
        AddValue(food.GetValue(), false);
        food.EatThis();
    }
    
    public void EatFish(FishData data)
    {
        AddValue(data.TotalValue, true);
    }

    private void AddValue(float value, bool isAlwaysDiscount)
    {
        float sum = TotalValue + value;

        if (sum < MaxGrowValue && !isAlwaysDiscount)
        {
            TotalValue = sum;
        }
        else
        {
            float discount = sum - MaxGrowValue;

            if (discount > value || isAlwaysDiscount)
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

    public void SetDataValueForEnemy(float speed, float size)
    {
        Speed = speed;
        Size = size;
        // value 계산법을 만든다면 역산을 해서 넣어두자.
        TotalValue = 0f;
    }
}
