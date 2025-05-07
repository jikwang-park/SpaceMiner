using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
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
        if (string.IsNullOrEmpty(input))
        {
            parts = new List<int> { 0 };
            sign = 1;
            currentValue = "0";
            return;
        }

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
            input = input.Replace(",", "");
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
        if (float.IsNaN(input) || float.IsInfinity(input))
        {
            throw new ArgumentException(nameof(input));
        }

        sign = input < 0 ? -1 : 1;
        float absVal = Math.Abs(input);
        float truncated = (float)Math.Truncate(absVal);

        if (truncated <= int.MaxValue)
        {
            int intPart = (int)truncated;
            this = new BigNumber(sign * intPart);
        }
        else
        {
            string s = truncated
                .ToString("F0", CultureInfo.InvariantCulture);
            if (sign < 0)
                s = "-" + s;
            this = new BigNumber(s);
        }

        currentValue = this.ToString();
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
    public static implicit operator BigNumber(float value)
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
    public static BigNumber operator +(BigNumber a, float b)
    {
        BigNumber other = new BigNumber(b);

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
    public static BigNumber operator +(float a, BigNumber b)
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
            return new BigNumber(0);

        bool negative = multiplier < 0f;
        float absVal = Math.Abs(multiplier);

        int exponent = (int)Math.Floor(Math.Log10(absVal));
        double mantissa = absVal / Math.Pow(10.0, exponent);

        string mantStr = mantissa
            .ToString("0.#######", CultureInfo.InvariantCulture)
            .TrimEnd('0').TrimEnd('.');

        if (string.IsNullOrEmpty(mantStr))
            mantStr = "0";

        int decCount = 0;
        int dotIdx = mantStr.IndexOf('.');
        if (dotIdx >= 0)
        {
            decCount = mantStr.Length - dotIdx - 1;
            mantStr = mantStr.Remove(dotIdx, 1);
        }

        BigNumber numerator = new BigNumber(mantStr);
        BigNumber prod = a * numerator;

        int scaleExp = exponent - decCount;
        BigNumber result;

        if (scaleExp >= 0)
        {
            result = prod * new BigNumber("1" + new string('0', scaleExp));
        }
        else
        {
            int decPower = -scaleExp;
            try
            {
                if (decPower <= 9)
                {
                    int divisor = checked((int)Math.Pow(10, decPower));
                    result = prod / divisor;
                }
                else
                {
                    throw new OverflowException();
                }
            }
            catch (OverflowException e)
            {

                float approx = prod.DivideToFloat(new BigNumber("1" + new string('0', decPower)));
                result = new BigNumber(approx);
            }
        }

        result.sign = a.sign * (negative ? -1 : 1);
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
    public static BigNumber operator *(BigNumber a, BigNumber b)
    {
        int n = a.parts.Count;
        int m = b.parts.Count;
        int[] resultArr = new int[n + m];

        for (int i = 0; i < n; i++)
        {
            int carry = 0;
            for (int j = 0; j < m; j++)
            {
                long mult = (long)a.parts[i] * (long)b.parts[j] + resultArr[i + j] + carry;
                resultArr[i + j] = (int)(mult % 1000);
                carry = (int)(mult / 1000);
            }
            resultArr[i + m] += carry;
        }

        List<int> resultParts = new List<int>(resultArr);

        BigNumber result = new BigNumber(resultParts);
        result.sign = a.sign * b.sign;
        result.Normalize();
        return result;
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

        const int PREC = 3;
        float thisValue = 0f;
        float otherValue = 0f;
        int thisCount = this.parts.Count;
        int otherCount = other.parts.Count;

        for (int k = 0; k < PREC; k++)
        {
            int thisIndex = thisCount - 1 - k;
            int otherIndex = otherCount - 1 - k;
            float a = (thisIndex >= 0 ? this.parts[thisIndex] : 0f);
            float b = (otherIndex >= 0 ? other.parts[otherIndex] : 0f);

            thisValue = thisValue * 1000f + a;
            otherValue = otherValue * 1000f + b;
        }

        int diff = thisCount - otherCount;
        float scale = Mathf.Pow(1000f, diff);

        return (this.sign * thisValue) / (other.sign * otherValue) * scale;
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
    public string GetSortKey()
    {
        string countKey = parts.Count.ToString("D2");
        var groupKeys = new List<string>(parts.Count);
        for (int i = parts.Count - 1; i >= 0; i--)
        {
            groupKeys.Add(parts[i].ToString("D3"));
        }

        return countKey + "-" + string.Join("-", groupKeys);
    }
}