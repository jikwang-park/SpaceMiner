using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.UI;

public class StageButton : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    [SerializeField]
    private TextMeshProUGUI text;

    private int planet;
    private int stage;

    private Button button;
    private StageManager stageManager;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Release()
    {
        button.onClick.RemoveAllListeners();
        ObjectPool.Release(gameObject);
    }

    public void Set(int planet, int stage)
    {
        this.planet = planet;
        this.stage = stage;
        text.text = $"{planet}-{stage}";
        button.onClick.AddListener(MoveStage);
    }

    private void MoveStage()
    {
        Variables.planetNumber = planet;
        Variables.stageNumber = stage;
        Addressables.LoadSceneAsync("StageDevelopScene");
    }
}
