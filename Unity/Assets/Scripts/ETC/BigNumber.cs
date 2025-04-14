using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public struct BigNumber : ISerializationCallbackReceiver
{
    [JsonProperty]
    [SerializeField]
    private string currentValue;
    private List<int> parts;
    private int sign;
    private static readonly string[] units = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", 
                                "N", "O", "P", "Q", "R", "S", "T", "U", "V",  "W", "X", "Y", "Z"};
    public BigNumber(string input)
    {
        parts = new List<int>();
        sign = 1;
        input = input.Trim();
        currentValue = "";

        if (input.Length == 0 || input.Equals("0"))
        {
            parts.Add(0);
            currentValue = "0";
            return;
        }

        if (input[0] == '-')
        {
            sign = -1;
            input = input.Substring(1).Trim();
        }

        if (char.IsLetter(input[input.Length - 1]))
        {
            string unitChar = input[input.Length - 1].ToString();
            int targetUnitIndex = Array.IndexOf(units, unitChar);
            string numberPart = input.Substring(0, input.Length - 1).Trim();
            string[] tokens = numberPart.Split('.');

            int integerPart = int.Parse(tokens[0]);
            int fractionPart = 0;

            if(tokens.Length > 1 && tokens[1] != "") 
            {
                fractionPart = int.Parse(tokens[1]);
                while(fractionPart != 0 && fractionPart * 10 < 1000)
                {
                    fractionPart *= 10;
                }
            }

            for(int i = 0; i <= targetUnitIndex; i++)
            {
                parts.Add(0);
            }

            parts[targetUnitIndex] = integerPart;
            parts[targetUnitIndex - 1] = fractionPart;
        }
        else
        {
            for(int i = input.Length; i > 0; i -= 3)
            {
                int start = Math.Max(0, i - 3);
                string partStr = input.Substring(start, i - start);
                parts.Add(int.Parse(partStr));
            }
        }
        currentValue = ToString();
    }
    public BigNumber(int input)
    {
        parts = new List<int>();
        sign = 1;
        currentValue = "";

        if (input < 0)
        {
            sign = -1;
        }
        if(input == 0)
        {
            parts.Add(0);
            return;
        }
        while(input > 0)
        {
            parts.Add(input % 1000);
            input /= 1000;
        }
        currentValue = ToString();
    }
    public BigNumber(float input)
    {
        int truncated = (int)input;
        this = new BigNumber(truncated);
    }
    private BigNumber(List<int> parts)
    {
        this.parts = parts;
        sign = 1;
        currentValue = "";

        Normalize();    
        currentValue = ToString();
    }
    public static implicit operator BigNumber(int value)
    {
        return new BigNumber(value);
    }
    public static implicit operator BigNumber(string value)
    {
        return new BigNumber(value);
    }
    public static BigNumber operator -(BigNumber a)
    {
        BigNumber result = new BigNumber(new List<int>(a.parts));
        result.sign = -a.sign;
        return result;
    }
    public static BigNumber operator +(BigNumber a, BigNumber b)
    {
        if (a.sign == b.sign)
        {
            List<int> newParts = AddAbsolute(a.parts, b.parts);
            BigNumber result = new BigNumber(newParts);
            result.sign = a.sign;
            return result;
        }
        else
        {
            int cmp = CompareAbsolute(a.parts, b.parts);
            if (cmp == 0)
            {
                return new BigNumber("0");
            }
            else if (cmp > 0)
            {
                List<int> newParts = SubtractAbsolute(a.parts, b.parts);
                BigNumber result = new BigNumber(newParts);
                result.sign = a.sign;
                return result;
            }
            else
            {
                List<int> newParts = SubtractAbsolute(b.parts, a.parts);
                BigNumber result = new BigNumber(newParts);
                result.sign = b.sign;
                return result;
            }
        }
    }
    public static BigNumber operator +(BigNumber a, int b)
    {
        BigNumber other = new BigNumber(b.ToString());

        return a + other;
    }
    public static BigNumber operator +(BigNumber a, string b)
    {
        BigNumber other = new BigNumber(b);

        return a + other;
    }
    public static BigNumber operator +(int a, BigNumber b)
    {
        BigNumber other = new BigNumber(a.ToString());

        return other + b;
    }
    public static BigNumber operator +(string a, BigNumber b)
    {
        BigNumber other = new BigNumber(a);

        return other + b;
    }
    public static BigNumber operator -(BigNumber a, BigNumber b)
    {
        return a + (-b);
    }
    public static BigNumber operator -(BigNumber a, int b)
    {
        BigNumber other = new BigNumber(b.ToString());

        return a + (-other);
    }
    public static BigNumber operator -(BigNumber a, string b)
    {
        BigNumber other = new BigNumber(b);

        return a + (-other);
    }
    public static BigNumber operator -(int a, BigNumber b)
    {
        BigNumber other = new BigNumber(a.ToString());

        return other + (-b);
    }
    public static BigNumber operator -(string a, BigNumber b)
    {
        BigNumber other = new BigNumber(a);

        return other + (-b);
    }
    public static BigNumber operator *(BigNumber a, float multiplier)
    {
        if (multiplier == 0f)
        {
            return new BigNumber("0");
        }
        int scale = 10000;

        int intMultiplier = (int)Math.Round(Math.Abs(multiplier) * scale);

        BigNumber temp = a * intMultiplier;
        BigNumber result = temp / scale;
        result.sign = a.sign * (multiplier < 0 ? -1 : 1);
        return result;
    }
    public static BigNumber operator *(float multiplier, BigNumber a)
    {
        return a * multiplier;
    }
    public static BigNumber operator *(BigNumber a, int multiplier)
    {
        if (multiplier == 0)
        {
            return new BigNumber("0");
        }

        int newSign = a.sign * (multiplier < 0 ? -1 : 1);
        multiplier = Math.Abs(multiplier);

        List<int> resultParts = new List<int>();
        int carry = 0;
        for (int i = 0; i < a.parts.Count; i++)
        {
            long prod = (long)a.parts[i] * multiplier + carry;
            resultParts.Add((int)(prod % 1000));
            carry = (int)(prod / 1000);
        }
        while (carry > 0)
        {
            resultParts.Add(carry % 1000);
            carry /= 1000;
        }

        BigNumber result = new BigNumber(resultParts);
        result.sign = newSign;
        result.Normalize();
        return result;
    }
    public static BigNumber operator *(int multiplier, BigNumber a)
    {
        return a * multiplier;
    }
    public static BigNumber operator /(BigNumber a, int divisor)
    {
        if (divisor == 0)
        {
            throw new DivideByZeroException();
        }

        int newSign = a.sign * (divisor < 0 ? -1 : 1);
        divisor = Math.Abs(divisor);

        List<int> dividendParts = new List<int>(a.parts);
        dividendParts.Reverse();

        List<int> quotientDigits = new List<int>();
        long remainder = 0;

        foreach (int part in dividendParts)
        {
            long current = remainder * 1000 + part;
            int q = (int)(current / divisor);
            remainder = current % divisor;
            quotientDigits.Add(q);
        }

        while (quotientDigits.Count > 1 && quotientDigits[0] == 0)
        {
            quotientDigits.RemoveAt(0);
        }
        quotientDigits.Reverse();

        BigNumber result = new BigNumber(quotientDigits);
        result.sign = newSign;
        result.Normalize();
        return result;
    }
    public static BigNumber operator /(BigNumber a, float divisor)
    {
        if (divisor == 0f)
        {
            throw new DivideByZeroException("Cannot divide by zero");
        }
        int scale = 10000;
        int intDivisor = (int)Math.Round(Math.Abs(divisor) * scale);
        BigNumber temp = a * scale;
        BigNumber result = temp / intDivisor;
        result.sign = a.sign * (divisor < 0 ? -1 : 1);
        return result;
    }
    public float DivideToFloat(BigNumber other)
    {
        if(other == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero");
        }

        int n = Math.Min(3, Math.Min(this.parts.Count, other.parts.Count));

        float thisValue = 0f;
        float otherValue = 0f;

        for(int i = this.parts.Count - 1; i > this.parts.Count - 1 - n; i--)
        {
            thisValue = thisValue * 1000f + (float)this.parts[i];
        }
        for (int i = other.parts.Count - 1; i > other.parts.Count - 1 - n; i--)
        {
            otherValue = otherValue * 1000f + (float)other.parts[i];
        }

        int diff = this.parts.Count - other.parts.Count;

        if(diff > 0)
        {
            thisValue *= Mathf.Pow(1000f, Math.Abs(diff));
        }
        if (diff < 0)
        {
            otherValue *= Mathf.Pow(1000f, Math.Abs(diff));
        }

        return (this.sign * thisValue) / (other.sign * otherValue);
    }
    public int CompareTo(BigNumber other)
    {
        if(other.Equals(new BigNumber("0")))
        {
            return 1;
        }

        if (this.sign != other.sign)
        {
            return this.sign < other.sign ? -1 : 1;
        }

        int cmp = CompareAbsolute(this.parts, other.parts);
        return this.sign == 1 ? cmp : -cmp;
    }
    public static bool operator <(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) < 0;
    }
    public static bool operator <(BigNumber a, int b)
    {
        BigNumber other = new BigNumber(b.ToString());
        return a < other;
    }
    public static bool operator <(int a, BigNumber b)
    {
        BigNumber other = new BigNumber(a.ToString());
        return other < b;
    }
    public static bool operator >(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) > 0;
    }
    public static bool operator >(BigNumber a, int b)
    {
        BigNumber other = new BigNumber(b.ToString());
        return a > other;
    }
    public static bool operator >(int a, BigNumber b)
    {
        BigNumber other = new BigNumber(a.ToString());
        return other > b;
    }
    public static bool operator <=(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) <= 0;
    }
    public static bool operator <=(BigNumber a, int b)
    {
        BigNumber other = new BigNumber(b.ToString());
        return a <= other;
    }
    public static bool operator <=(int a, BigNumber b)
    {
        BigNumber other = new BigNumber(a.ToString());
        return other <= b;
    }
    public static bool operator >=(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) >= 0;
    }
    public static bool operator >=(BigNumber a, int b)
    {
        BigNumber other = new BigNumber(b.ToString());
        return a >= other;
    }
    public static bool operator >=(int a, BigNumber b)
    {
        BigNumber other = new BigNumber(a.ToString());
        return other >= b;
    }
    public static bool operator ==(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) == 0;
    }
    public static bool operator ==(BigNumber a, int b)
    {
        BigNumber other = new BigNumber(b.ToString());
        return a == other;
    }
    public static bool operator ==(int a, BigNumber b)
    {
        BigNumber other = new BigNumber(a.ToString());
        return other == b;
    }
    public static bool operator !=(BigNumber a, BigNumber b)
    {
        return !(a == b);
    }
    public static bool operator !=(BigNumber a, int b)
    {
        BigNumber other = new BigNumber(b.ToString());
        return a != other;
    }
    public static bool operator !=(int a, BigNumber b)
    {
        BigNumber other = new BigNumber(a.ToString());
        return other != b;
    }
    private static List<int> AddAbsolute(List<int> aParts, List<int> bParts)
    {
        List<int> result = new List<int>();
        int sizeCount = Math.Max(aParts.Count, bParts.Count);
        int carry = 0;
        for (int i = 0; i < sizeCount; i++)
        {
            int valueA = i < aParts.Count ? aParts[i] : 0;
            int valueB = i < bParts.Count ? bParts[i] : 0;
            int sum = valueA + valueB + carry;
            result.Add(sum % 1000);
            carry = sum / 1000;
        }
        if (carry > 0)
        {
            result.Add(carry);
        }
        return result;
    }
    private static List<int> SubtractAbsolute(List<int> aParts, List<int> bParts)
    {
        List<int> result = new List<int>();
        int borrow = 0;
        int sizeCount = aParts.Count;

        for (int i = 0; i < sizeCount; i++)
        {
            int valueA = aParts[i];
            int valueB = i < bParts.Count ? bParts[i] : 0;
            int diff = valueA - valueB - borrow;
            if (diff < 0)
            {
                diff += 1000;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }
            result.Add(diff);
        }
        while (result.Count > 1 && result[result.Count - 1] == 0)
        {
            result.RemoveAt(result.Count - 1);
        }
        return result;
    }
    private static int CompareAbsolute(List<int> aParts, List<int> bParts)
    {
        if (aParts.Count != bParts.Count)
        {
            return aParts.Count.CompareTo(bParts.Count);
        }

        for (int i = aParts.Count - 1; i >= 0; i--)
        {
            if (aParts[i] != bParts[i])
            {
                return aParts[i].CompareTo(bParts[i]);
            }
        }
        return 0;
    }
    private void Normalize()
    {
        int i = parts.Count - 1;

        while (i > 0 && parts[i] == 0) 
        {
            parts.RemoveAt(i);
            i--;
        }
    }
    public override string ToString()
    {
        if (parts == null || parts.Count == 0)
        {
            return "0";
        }

        string stringSign = sign == 1 ? "" : "-";
        if(parts.Count > 1)
        {
            return $"{stringSign}{parts[parts.Count - 1]}.{parts[parts.Count - 2] / 100}{units[parts.Count - 1]}";
        }
        else 
        {
            return $"{stringSign}{parts[parts.Count - 1]}";
        }
    }
    [OnDeserialized]
    private void OnDeserializedMethod(StreamingContext context)
    {
        if (!string.IsNullOrEmpty(currentValue))
        {
            BigNumber temp = new BigNumber(currentValue);
            this.parts = temp.parts;
            this.sign = temp.sign;
        }
    }
    public void OnBeforeSerialize()
    {
    }
    public void OnAfterDeserialize()
    {
        if (!string.IsNullOrEmpty(currentValue))
        {
            BigNumber temp = new BigNumber(currentValue);
            this.parts = temp.parts;
            this.sign = temp.sign;
        }
    }
}