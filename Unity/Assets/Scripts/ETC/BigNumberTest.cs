using System;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.Profiling;    // Profiler API (¿øÇÏ½Ã¸é ³²°ÜµÎ¼¼¿ä)
using Debug = UnityEngine.Debug;

public class BigNumberTest : MonoBehaviour
{
    [SerializeField] private int iterations = 10000;
    [SerializeField] private int digitLength = 50;

    private string[] inputsA;
    private string[] inputsB;

    void Start()
    {
        PrepareInputs();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            BigIntegerTimeBench();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            BigNumberTimeBench();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            BigIntegerMemoryBench();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            BigNumberMemoryBench();
        }
    }
    private void BigIntegerTimeBench()
    {
        BigIntegerAdd();
        BigIntegerSubtract();
        BigIntegerMultiply();
        BigIntegerDivide();
        Debug.Break();
    }
    private void BigNumberTimeBench()
    {
        BigNumberAdd();
        BigNumberSubtract();
        BigNumberMultiply();
        BigNumberDivide();
        Debug.Break();
    }
    private void BigIntegerMemoryBench()
    {
        BigIntegerAddMemory();
        BigIntegerSubMemory();
        BigIntegerMulMemory();
        BigIntegerDivMemory();
        Debug.Break();
    }
    private void BigNumberMemoryBench()
    {
        BigNumberAddMemory();
        BigNumberSubMemory();
        BigNumberMulMemory();
        BigNumberDivMemory();
        Debug.Break();
    }
    private void PrepareInputs()
    {
        var rnd = new System.Random(1234);
        inputsA = new string[iterations];
        inputsB = new string[iterations];
        for (int i = 0; i < iterations; i++)
        {
            inputsA[i] = RandomNumericString(rnd, digitLength);
            inputsB[i] = RandomNumericString(rnd, digitLength);
        }
    }

    private void BigIntegerAdd()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(BigInteger.Parse(inputsA[i]).ToByteArray());
            var b = new BigInteger(BigInteger.Parse(inputsB[i]).ToByteArray());
            var c = a + b;
        }
        sw.Stop();


        Debug.Log($"[BigInteger µ¡¼À] {iterations} È¸ : ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }

    private void BigIntegerSubtract()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(BigInteger.Parse(inputsA[i]).ToByteArray());
            var b = new BigInteger(BigInteger.Parse(inputsB[i]).ToByteArray());
            var c = a - b;
        }
        sw.Stop();

        Debug.Log($"[BigInteger »¬¼À] {iterations} È¸ : ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }

    private void BigIntegerMultiply()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(BigInteger.Parse(inputsA[i]).ToByteArray());
            var b = new BigInteger(BigInteger.Parse(inputsB[i]).ToByteArray());
            var c = a * b;
        }
        sw.Stop();

        Debug.Log($"[BigInteger °ö¼À] {iterations} È¸ : ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }

    private void BigIntegerDivide()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(BigInteger.Parse(inputsA[i]).ToByteArray());
            var b = new BigInteger(BigInteger.Parse(inputsB[i]).ToByteArray());
            var c = a / b;
        }
        sw.Stop();

        Debug.Log($"[BigInteger ³ª´°¼À] {iterations} È¸ : ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }

    private void BigNumberAdd()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a + b;
        }
        sw.Stop();

        Debug.Log($"[BigNumber µ¡¼À] {iterations} È¸ : ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }

    private void BigNumberSubtract()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a - b;
        }
        sw.Stop();

        Debug.Log($"[BigNumber »¬¼À] {iterations} È¸ : ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }

    private void BigNumberMultiply()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a * b;
        }
        sw.Stop();

        Debug.Log($"[BigNumber °ö¼À] {iterations} È¸ : ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }

    private void BigNumberDivide()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a.DivideToFloat(b);
        }
        sw.Stop();

        long memAfter = GC.GetTotalMemory(true);
        Debug.Log($"[BigNumber ³ª´°¼À] {iterations} È¸ : ½Ã°£: {sw.ElapsedMilliseconds} ms");
    }
    public void BigIntegerAddMemory()
    {
        Profiler.BeginSample("BigInteger µ¡¼À (GC)");
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(BigInteger.Parse(inputsA[i]).ToByteArray());
            var b = new BigInteger(BigInteger.Parse(inputsB[i]).ToByteArray());
            var c = a + b;
        }
        Profiler.EndSample();
    }
    public void BigIntegerSubMemory()
    {
        Profiler.BeginSample("BigInteger »¬¼À (GC)");
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(BigInteger.Parse(inputsA[i]).ToByteArray());
            var b = new BigInteger(BigInteger.Parse(inputsB[i]).ToByteArray());
            var c = a - b;
        }
        Profiler.EndSample();
    }
    public void BigIntegerMulMemory()
    {
        Profiler.BeginSample("BigInteger °ö¼À (GC)");
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(BigInteger.Parse(inputsA[i]).ToByteArray());
            var b = new BigInteger(BigInteger.Parse(inputsB[i]).ToByteArray());
            var c = a * b;
        }
        Profiler.EndSample();
    }
    public void BigIntegerDivMemory()
    {
        Profiler.BeginSample("BigInteger ³ª´°¼À (GC)");
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(BigInteger.Parse(inputsA[i]).ToByteArray());
            var b = new BigInteger(BigInteger.Parse(inputsB[i]).ToByteArray());
            var c = a / b;
        }
        Profiler.EndSample();
    }
    public void BigNumberAddMemory()
    {
        Profiler.BeginSample("BigNumber µ¡¼À (GC)");
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a + b;
        }
        Profiler.EndSample();
    }
    public void BigNumberSubMemory()
    {
        Profiler.BeginSample("BigNumber »¬¼À (GC)");
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a - b;
        }
        Profiler.EndSample();
    }
    public void BigNumberMulMemory()
    {
        Profiler.BeginSample("BigNumber °ö¼À (GC)");
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a * b;
        }
        Profiler.EndSample();
    }
    public void BigNumberDivMemory()
    {
        Profiler.BeginSample("BigNumber ³ª´°¼À (GC)");
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigNumber(inputsA[i]);
            var b = new BigNumber(inputsB[i]);
            var c = a.DivideToFloat(b);
        }
        Profiler.EndSample();
    }
    private string RandomNumericString(System.Random rnd, int length)
    {
        var chars = new char[length];
        chars[0] = (char)('1' + rnd.Next(0, 9));
        for (int i = 1; i < length; i++)
            chars[i] = (char)('0' + rnd.Next(0, 10));
        return new string(chars);
    }
}
