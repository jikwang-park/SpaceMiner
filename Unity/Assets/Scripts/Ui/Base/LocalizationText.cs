using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationText : MonoBehaviour
{
    [SerializeField]
    private string stringId;
    [SerializeField]
    private string[] stringArgs;

#if UNITY_EDITOR
    [SerializeField]
    private Languages editorLang;
#endif

    private TextMeshProUGUI text;

    public bool TextEnabled
    {
        get => text.enabled;
        set => text.enabled = value;
    }

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        OnChangedLanguage();
    }

    public void OnChangedLanguage(Languages lang)
    {
        var stringTableId = DataTableIds.stringTables[(int)lang];
        var stringTable = DataTableManager.GetTable<StringTable>(stringTableId);
        if (int.TryParse(stringId, out int id))
        {
            if (stringArgs != null && stringArgs.Length > 0)
                text.text = string.Format(stringTable.GetData(id), stringArgs);
            else
                text.text = stringTable.GetData(id);
        }
    }

    private void OnChangedLanguage()
    {
        if (Application.isPlaying)
        {
            OnChangedLanguage(Variables.currentLanguage);
        }
#if UNITY_EDITOR
        else
        {
            OnChangedLanguage(editorLang);
        }
#endif
    }

    public void SetString(string stringId)
    {
        this.stringId = stringId;
        stringArgs = null;
        OnChangedLanguage();
    }

    public void SetStringArguments(params string[] stringArguments)
    {
        stringArgs = stringArguments;
        OnChangedLanguage();
    }

    public void SetString(string stringId, params string[] stringArguments)
    {
        this.stringId = stringId;
        stringArgs = stringArguments;
        OnChangedLanguage();
    }

    public void SetString(string stringId, params int[] stringIds)
    {
        this.stringId = stringId;
        stringArgs = new string[stringIds.Length];
        var stringTableId = DataTableIds.stringTables[(int)Variables.currentLanguage];
        var stringTable = DataTableManager.GetTable<StringTable>(stringTableId);
        for (int i = 0; i < stringIds.Length; ++i)
        {
            stringArgs[i] = stringTable.GetData(stringIds[i]);
        }
        OnChangedLanguage();
    }
}
