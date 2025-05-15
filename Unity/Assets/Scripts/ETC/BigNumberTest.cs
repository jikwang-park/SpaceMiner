using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BigNumberTest : MonoBehaviour
{
    private readonly int iterations = 1000000;
    private int digitLength = 50;
    void Start()
    {
        Test();
    }

    private void Test()
    {
        var rnd = new System.Random(1234);

        var inputsA = new string[iterations];
        var inputsB = new string[iterations];

        for (int i = 0; i < iterations; i++)
        {
            inputsA[i] = RandomNumericString(rnd, digitLength);
            inputsB[i] = RandomNumericString(rnd, digitLength);
        }


        var stopWatch = new Stopwatch();

        
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(ParseNumericString(inputsA[i]));
            var b = new BigInteger(ParseNumericString(inputsB[i]));
            var c = a + b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigInteger] Æò±Õ µ¡¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(ParseNumericString(inputsA[i]));
            var b = new BigInteger(ParseNumericString(inputsB[i]));
            var c = a - b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigInteger] Æò±Õ »¬¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(ParseNumericString(inputsA[i]));
            var b = new BigInteger(ParseNumericString(inputsB[i]));
            var c = a * b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigInteger] Æò±Õ °ö¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(ParseNumericString(inputsA[i]));
            var b = new BigInteger(ParseNumericString(inputsB[i]));
            var c = a / b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigInteger] Æò±Õ ³ª´°¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a + b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigNumber] Æò±Õ µ¡¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a - b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigNumber] Æò±Õ »¬¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a * b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigNumber] Æò±Õ °ö¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a.DivideToFloat(b);
        }
        stopWatch.Stop();
        Debug.Log($"[BigNumber] Æò±Õ ³ª´°¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
    }

    private string RandomNumericString(System.Random rnd, int length)
    {
        var chars = new char[length];
        chars[0] = (char)('1' + rnd.Next(0, 9));
        for (int i = 1; i < length; i++)
        {
            chars[i] = (char)('0' + rnd.Next(0, 10));
        }
        return new string(chars);
    }
    private byte[] ParseNumericString(string s)
    {
        return BigInteger.Parse(s).ToByteArray();
    }
}
