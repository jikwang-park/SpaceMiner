using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    [SerializeField]
    private TutorialTable.QuestTypes targetType;

    private bool opened = false;

    public void OnButtonClick()
    {
        //TODO: ���̺� ������ �߰� �� ����
        if (!opened)
        {
            opened = true;

            var stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
            var uiManager = stageManager.StageUiManager;
            var tutorialWindow = uiManager.TutorialWindow;
            tutorialWindow.gameObject.SetActive(true);
            tutorialWindow.Show(targetType);
        }
    }
}
