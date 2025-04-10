using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationText : MonoBehaviour
{
    private const string notParsedFormat = "Not Parsed {0}";
    private const string notFoundFormat = "Not Found {0}";

    [SerializeField]
    private string stringId;
    [SerializeField]
    private string[] stringArgs;

    private int parsedStringId;

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
        if (int.TryParse(stringId, out parsedStringId))
        {
            OnChangedLanguage();
        }
        else
        {
            text.text = string.Format(notParsedFormat, stringId);
        }
    }

    public void OnChangedLanguage(Languages lang)
    {
        var stringTableId = DataTableIds.stringTables[(int)lang];
        var stringTable = DataTableManager.GetTable<StringTable>(stringTableId);

        if (string.IsNullOrEmpty(stringTable.GetData(parsedStringId)))
        {
            text.text = string.Format(notFoundFormat, stringId);
            return;
        }

        if (stringArgs != null && stringArgs.Length > 0)
        {
            text.text = string.Format(stringTable.GetData(parsedStringId), stringArgs);
        }
        else
        {
            text.text = stringTable.GetData(parsedStringId);
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

        if (int.TryParse(stringId, out parsedStringId))
        {
            OnChangedLanguage();
        }
        else
        {
            text.text = string.Format(notParsedFormat, stringId);
        }
    }

    public void SetString(int stringId)
    {
        this.stringId = stringId.ToString();
        parsedStringId = stringId;
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

        if (int.TryParse(stringId, out parsedStringId))
        {
            OnChangedLanguage();
        }
        else
        {
            text.text = string.Format(notParsedFormat, stringId);
        }
    }

    public void SetString(string stringId, params int[] stringIds)
    {
        this.stringId = stringId;

        if (int.TryParse(stringId, out parsedStringId))
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
        else
        {
            text.text = string.Format(notParsedFormat, stringId);
        }
    }
}
