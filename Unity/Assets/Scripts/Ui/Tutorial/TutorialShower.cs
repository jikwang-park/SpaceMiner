using UnityEngine;

public class TutorialShower : MonoBehaviour
{
    [SerializeField]
    private TutorialTable.QuestTypes targetType;

    private void Start()
    {
        return;


        if (!SaveLoadManager.Data.TutorialOpened[targetType])
        {
            var stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
            var uiManager = stageManager.StageUiManager;
            var tutorialWindow = uiManager.TutorialWindow;
            tutorialWindow.gameObject.SetActive(true);
            tutorialWindow.Show(targetType);
        }
    }
}
