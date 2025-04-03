using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class DungeonEndWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nextText;

    private float closeTime;
    private WaitForSeconds wait = new WaitForSeconds(1f);

    private bool isCleared;

    public void Set(bool isCleared)
    {
        this.isCleared = isCleared;
    }

    private void OnEnable()
    {
        if (isCleared)
        {
            if(Variables.currentDungeonStage == DataTableManager.DungeonTable.CountOfStage(Variables.currentDungeonType))
            {
                nextText.text = "Retry";
                return;
            }
            nextText.text = "Next";
        }
        else
        {
            nextText.text = "Retry";
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void RightButton()
    {
        if (isCleared
            && Variables.currentDungeonStage < DataTableManager.DungeonTable.CountOfStage(Variables.currentDungeonType))
        {
            Lift();
        }
        else
        {
            Retry();
        }
    }

    public void Lift()
    {
        ++Variables.currentDungeonStage;
        Retry();
    }

    public void Retry()
    {
        Addressables.LoadSceneAsync("Scenes/DungeonScene").WaitForCompletion();
    }
}
