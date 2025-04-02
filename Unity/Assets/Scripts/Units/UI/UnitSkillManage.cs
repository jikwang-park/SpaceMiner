using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillManage : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;




    private void Start()
    {
        closeButton.onClick.AddListener(() => OnClickCloseButton());
    }

    private void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }
}
