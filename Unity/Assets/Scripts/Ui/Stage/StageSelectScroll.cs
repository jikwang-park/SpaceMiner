using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectScroll : MonoBehaviour
{

    [SerializeField]
    private string buttonReference = "StageButton";

    [SerializeField]
    private Transform contents;

    private void Start()
    {
        var objectpoolManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectPoolManager>();

        for (int i = 1; i <= 10; ++i)
        {
            var button = objectpoolManager.gameObjectPool[buttonReference].Get();
            button.transform.SetParent(contents);
            button.transform.localScale = Vector3.one;
            button.GetComponent<StageButton>().Set(1, i);
        }
    }
}
