using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataCell : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    public TMP_InputField inputField;

    public string CellText => buttonText.text;

    public void Init(string str)
    {
        buttonText.text = str;
    }
    public void ActiveInputFiled()
    {
        inputField.gameObject.SetActive(true);
        inputField.text = CellText;
        inputField.Select();
    }

    public void InputEnd()
    {
        buttonText.text = inputField.text;
        inputField.gameObject.SetActive(false);
    }
}
