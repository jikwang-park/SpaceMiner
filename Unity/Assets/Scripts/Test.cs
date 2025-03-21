using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TextMeshProUGUI text;
    BigNumber testA = new BigNumber("100");
    BigNumber testB = new BigNumber("100.23E");
    // Start is called before the first frame update
    void Start()
    {
        BigNumber testA = new BigNumber("12305496");
        BigNumber testB = new BigNumber("100.23E");

        text.text = $"{testA}\n{testB}\n{testA + testB}\n{testA - testB}\n{testA * 0.01f}\n{testA * 1000000000}\n{testA / 100}\n{testA / 0.1f}" +
            $"\n{testA > testB}\n{testA < testB}\n{testA > 0}\n{testB < 0}";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
