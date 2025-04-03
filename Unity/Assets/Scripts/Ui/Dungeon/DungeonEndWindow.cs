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
        if (isCleared)
        {
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
        if (isCleared)
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
        ++Variables.selectedDungeonStage;
        Retry();
    }

    public void Retry()
    {
        Addressables.LoadSceneAsync("Scenes/DungeonScene").WaitForCompletion();
    }
}
