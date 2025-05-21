using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataCell : MonoBehaviour
{
    public Button button;

    public TextMeshProUGUI buttonText;

    public TMP_InputField inputField;

    public string CellText => buttonText.text;

    public void Init(string str)
    {
        buttonText.text = str;
    }
    public void ActiveInputFiled()
    {
        button.gameObject.SetActive(false);
        inputField.text = CellText;
        inputField.gameObject.SetActive(true);
        inputField.Select();
    }

    public void InputEnd()
    {
        buttonText.text = inputField.text;
        button.gameObject.SetActive(true);
        inputField.gameObject.SetActive(false);
    }
}
