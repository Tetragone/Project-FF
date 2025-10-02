public class FishData
{
    public string Fid = "1001";
    
    public float Size = 0f;
    private float SizeMultiValue = 0f;
    
    public float Speed = 0f;
    private float SpeedMultiValue = 0f;

    public float TotalValue = 0f;
    // 이 값을 넘어가면 조금씩 오르도록 하자
    private float BaseGrowValue = 0f;
    public float MaxGrowValue = 100f;
    public float DiscountValue = 0.01f;

    public float AdditionalValue = 0f;

    public void SetDataInit(string fid)
    {
        Fid = fid;

        Size = 0f;
        SizeMultiValue = TableMgr.GetTableFloat("fish", Fid, "size");
        Speed = 0f;
        SpeedMultiValue = TableMgr.GetTableFloat("fish", Fid, "speed")
            * GameStaticValue.FishMoveMulti(UpgradeMgr.Instance.GetGoldUpgrade(GoldUpgrade.fish_move_multi));
        TotalValue = 0f;

        BaseGrowValue = TableMgr.GetTableFloat("fish", Fid, "base_value");
        float random = TableMgr.GetTableFloat("fish", Fid, "random_value");
        MaxGrowValue = BaseGrowValue + UnityEngine.Random.Range(random * -1, random);
        DiscountValue = TableMgr.GetTableFloat("fish", Fid, "discount_value");
        AdditionalValue = 0f;
    }
    
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

        Size = GetSizeFromValue(TotalValue);
        Speed = GetSpeedFromValue(TotalValue);
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

    public void AddAdditionalValue(float value)
    {
        AdditionalValue += value * DiscountValue;
    }

    public float GetSpeedFromValue(float value)
    {
        return value / BaseGrowValue * SpeedMultiValue;
    }

    public float GetSizeFromValue(float value)
    {
        return value / BaseGrowValue * SizeMultiValue;
    }
}
