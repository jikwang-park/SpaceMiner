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
    void Start()
    {
        Test();
    }

    private void Test()
    {
        var rnd = new System.Random(1234);

        int[] inputsA = new int[iterations];
        int[] inputsB = new int[iterations];

        for (int i = 0; i < iterations; i++)
        {
            inputsA[i] = rnd.Next(10000000, 1000000000);
            inputsB[i] = rnd.Next(10000000, 1000000000);
        }


        var stopWatch = new Stopwatch();

        
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(inputsA[i]);
            var b = new BigInteger(inputsB[i]);
            var c = a + b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigInteger] Æò±Õ µ¡¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(inputsA[i]);
            var b = new BigInteger(inputsB[i]);
            var c = a - b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigInteger] Æò±Õ »¬¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(inputsA[i]);
            var b = new BigInteger(inputsB[i]);
            var c = a * b;
        }
        stopWatch.Stop();
        Debug.Log($"[BigInteger] Æò±Õ °ö¼À {iterations} È¸: {stopWatch.ElapsedMilliseconds} ms");
        /*-------------------------------------------------------------------------*/
        stopWatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var a = new BigInteger(inputsA[i]);
            var b = new BigInteger(inputsB[i]);
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
}
