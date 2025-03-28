using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButton : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    [SerializeField]
    private TextMeshProUGUI text;

    private int planet;
    private int stage;

    [field: SerializeField]
    public Button Button { get; private set; }
    private StageManager stageManager;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    public void Release()
    {
        Button.onClick.RemoveAllListeners();
        Button.interactable = true;
        ObjectPool.Release(gameObject);
    }

    public void Set(int planet, int stage)
    {
        this.planet = planet;
        this.stage = stage;
        text.text = $"{planet}-{stage}";
        Button.onClick.AddListener(MoveStage);
    }

    private void MoveStage()
    {
        Variables.planetNumber = planet;
        Variables.stageNumber = stage;
        SceneManager.LoadScene(0);
        //Addressables.LoadSceneAsync("StageDevelopScene");
    }
}
