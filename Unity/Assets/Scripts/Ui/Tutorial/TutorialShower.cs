using UnityEngine;

public class TutorialShower : MonoBehaviour
{
    [SerializeField]
    private TutorialTable.QuestTypes targetType;

    private bool opened = false;

    private void Start()
    {
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
