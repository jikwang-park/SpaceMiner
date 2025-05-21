using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTabButton : MonoBehaviour
{
    [SerializeField]
    private TutorialPage tutorialPage;

    [SerializeField]
    private TutorialTable.QuestTypes type;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.interactable = SaveLoadManager.Data.TutorialOpened[type];
    }

    public void OnButtonClick()
    {
        tutorialPage.Show(type);
    }
}
