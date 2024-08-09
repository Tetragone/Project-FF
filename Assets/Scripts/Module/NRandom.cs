using UnityEngine;

public static class NRandom
{
    public static float Range(float minInclude, float maxInclude, float middleValue = float.MaxValue)
    {
        float result = 0f;
        
        if (middleValue < minInclude || middleValue > maxInclude)
        {
            middleValue = (minInclude + maxInclude) / 2f;
        }

        float multi = Mathf.Sin(Mathf.PI * Random.value);

        result = multi > 0 ? (maxInclude - middleValue) * multi : (minInclude - middleValue) * multi;

        return result;
    }

    public static float NormalRandom(float mean, float standardDeviation = 1f)
    {
        float u1 = Random.value; // 0부터 1 사이의 균등 난수
        float u2 = Random.value;

        // Box-Muller 변환
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        // 평균과 표준 편차를 조정하여 정규분포 난수를 생성
        float randNormal = mean + standardDeviation * randStdNormal;

        return randNormal;
    }

    public static int Range(int minInclude, int maxExclude, float middleValue = float.MaxValue)
    {
        return Mathf.FloorToInt(Range((float)minInclude, (float)maxExclude, middleValue));
    }
}
