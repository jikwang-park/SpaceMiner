using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationText : MonoBehaviour
{
    private const string notFoundFormat = "Not Found {0}";

    [SerializeField]
    private int stringId;
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
        if (stringId != 0)
        {
            OnChangedLanguage();
        }
        else
        {
            text.text = string.Format(notFoundFormat, stringId);
        }
    }

    public void OnChangedLanguage(Languages lang)
    {
        if(text is null)
        {
            return;
        }
        var stringTableId = DataTableIds.stringTables[(int)lang];
        var stringTable = DataTableManager.GetTable<StringTable>(stringTableId);
        var gotString = stringTable.GetData(stringId);

        if (string.IsNullOrEmpty(gotString))
        {
            text.text = string.Format(notFoundFormat, stringId);
            return;
        }

        if (stringArgs != null && stringArgs.Length > 0)
        {
            text.text = string.Format(gotString, stringArgs);
        }
        else
        {
            text.text = gotString;
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

    public void SetString(int stringId)
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

    public void SetStringArguments(params int[] stringIds)
    {
        stringArgs = new string[stringIds.Length];
        var stringTableId = DataTableIds.stringTables[(int)Variables.currentLanguage];
        var stringTable = DataTableManager.GetTable<StringTable>(stringTableId);
        for (int i = 0; i < stringIds.Length; ++i)
        {
            stringArgs[i] = stringTable.GetData(stringIds[i]);
        }
        OnChangedLanguage();
    }

    public void SetString(int stringId, params string[] stringArguments)
    {
        this.stringId = stringId;
        stringArgs = stringArguments;

        OnChangedLanguage();
    }

    public void SetString(int stringId, params int[] stringIds)
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

    public void SetColor(Color color)
    {
        text.color = color;
    }
}
