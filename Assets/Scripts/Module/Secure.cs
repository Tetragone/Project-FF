using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class SecureFloat
{
    private string Value { get; set; }
    private static readonly int FloatToCharAdder = 45;

    private static string FloatToValue(float value)
    {
        string temp = value.ToString();
        List<char> result = new List<char>();
        for (int i = 0; i < temp.Length; i++)
        {
            result.Add((char)((temp[i] + FloatToCharAdder) % 128));
        }
        
        return new string(result.ToArray());
    }

    private static float ValueToString(string value)
    {
        List<char> result = new List<char>();
        for (int i = 0; i < value.Length; i++)
        {
            result.Add((char)((value[i] - FloatToCharAdder) % 128));
        }

        string temp = new string(result.ToArray());

        return float.Parse(temp);
    }

    public SecureFloat(float value)
    {
        Value = FloatToValue(value);
    }

    public SecureFloat(string value)
    {
        if (value == "")
        {
            Value = FloatToValue(0f);
        }
        else
        {
            Value = value;
        }
    }

    public static implicit operator float(SecureFloat myFloat)
    {
        return myFloat.GetValue();
    }

    public static implicit operator SecureFloat(float value)
    {
        return new SecureFloat(value);
    }

    public float GetValue()
    {
        return ValueToString(Value);
    }

    public string GetBase()
    {
        return Value;
    }

    // ToString 메서드 오버라이드
    public override string ToString()
    {
        return GetValue().ToString();
    }

    #region Operator
    public static SecureFloat operator +(SecureFloat a, SecureFloat b)
    {
        return new SecureFloat(a.GetValue() + b.GetValue());
    }

    public static SecureFloat operator +(SecureFloat a, float b)
    {
        return new SecureFloat(a.GetValue() + b);
    }

    public static SecureFloat operator +(float a, SecureFloat b)
    {
        return new SecureFloat(a + b.GetValue());
    }

    public static SecureFloat operator -(SecureFloat a, SecureFloat b)
    {
        return new SecureFloat(a.GetValue() - b.GetValue());
    }

    public static SecureFloat operator -(SecureFloat a, float b)
    {
        return new SecureFloat(a.GetValue() - b);
    }

    public static SecureFloat operator -(float a, SecureFloat b)
    {
        return new SecureFloat(a - b.GetValue());
    }

    public static SecureFloat operator *(SecureFloat a, SecureFloat b)
    {
        return new SecureFloat(a.GetValue() * b.GetValue());
    }

    public static SecureFloat operator *(SecureFloat a, float b)
    {
        return new SecureFloat(a.GetValue() * b);
    }

    public static SecureFloat operator *(float a, SecureFloat b)
    {
        return new SecureFloat(a * b.GetValue());
    }

    public static SecureFloat operator /(SecureFloat a, SecureFloat b)
    {
        return new SecureFloat(a.GetValue() / b.GetValue());
    }

    public static SecureFloat operator /(SecureFloat a, float b)
    {
        return new SecureFloat(a.GetValue() / b);
    }

    public static SecureFloat operator /(float a, SecureFloat b)
    {
        return new SecureFloat(a / b.GetValue());
    }
    #endregion

}

public class SecureInt
{
    private string Value { get; set; }
    private static readonly int IntToCharAdder = 17;

    private static string IntToValue(int value)
    {
        string temp = value.ToString();
        List<char> result = new List<char>();
        for (int i = 0; i < temp.Length; i++)
        {
            result.Add((char)((temp[i] + IntToCharAdder) % 128));
        }

        return new string(result.ToArray());
    }

    private static int ValueToString(string value)
    {
        List<char> result = new List<char>();
        for (int i = 0; i < value.Length; i++)
        {
            result.Add((char)((value[i] - IntToCharAdder) % 128));
        }

        string temp = new string(result.ToArray());

        return int.Parse(temp);
    }

    public SecureInt(int value)
    {
        Value = IntToValue(value);
    }

    public SecureInt(string value)
    {
        if (value == "")
        {
            Value = IntToValue(0);
        }
        else
        {
            Value = value;
        }
    }

    public static implicit operator int(SecureInt myInt)
    {
        return myInt.GetValue();
    }

    public static implicit operator SecureInt(int value)
    {
        return new SecureInt(value);
    }

    public int GetValue()
    {
        return ValueToString(Value);
    }

    public string GetBase()
    {
        return Value;
    }

    // ToString 메서드 오버라이드
    public override string ToString()
    {
        return GetValue().ToString();
    }

    #region Operator
    public static SecureInt operator +(SecureInt a, SecureInt b)
    {
        return new SecureInt(a.GetValue() + b.GetValue());
    }

    public static SecureInt operator +(SecureInt a, int b)
    {
        return new SecureInt(a.GetValue() + b);
    }

    public static SecureInt operator +(int a, SecureInt b)
    {
        return new SecureInt(a + b.GetValue());
    }

    public static SecureInt operator -(SecureInt a, SecureInt b)
    {
        return new SecureInt(a.GetValue() - b.GetValue());
    }

    public static SecureInt operator -(SecureInt a, int b)
    {
        return new SecureInt(a.GetValue() - b);
    }

    public static SecureInt operator -(int a, SecureInt b)
    {
        return new SecureInt(a - b.GetValue());
    }

    public static SecureInt operator *(SecureInt a, SecureInt b)
    {
        return new SecureInt(a.GetValue() * b.GetValue());
    }

    public static SecureInt operator *(SecureInt a, int b)
    {
        return new SecureInt(a.GetValue() * b);
    }

    public static SecureInt operator *(int a, SecureInt b)
    {
        return new SecureInt(a * b.GetValue());
    }

    public static SecureInt operator /(SecureInt a, SecureInt b)
    {
        return new SecureInt(a.GetValue() / b.GetValue());
    }

    public static SecureInt operator /(SecureInt a, int b)
    {
        return new SecureInt(a.GetValue() / b);
    }

    public static SecureInt operator /(int a, SecureInt b)
    {
        return new SecureInt(a / b.GetValue());
    }

    public static SecureInt operator %(SecureInt a, SecureInt b)
    {
        return new SecureInt(a.GetValue() / b.GetValue());
    }

    public static SecureInt operator %(SecureInt a, int b)
    {
        return new SecureInt(a.GetValue() / b);
    }

    public static SecureInt operator %(int a, SecureInt b)
    {
        return new SecureInt(a / b.GetValue());
    }
    #endregion
}