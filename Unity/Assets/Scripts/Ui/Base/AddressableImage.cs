using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AddressableImage : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private int spriteID;

    [SerializeField]
    private Image.Type type;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        LoadSprite();
    }

    private void LoadSprite()
    {
        if (image is null)
        {
            return;
        }
        int currentSpriteID = spriteID;
        var imageAddress = DataTableManager.AddressTable.GetData(currentSpriteID);
        if (string.IsNullOrEmpty(imageAddress))
        {
            return;
        }

        Addressables.LoadAssetAsync<Sprite>(imageAddress).Completed += (handle) => AddressableIcon_Completed(handle, currentSpriteID);
    }

    private void AddressableIcon_Completed(AsyncOperationHandle<Sprite> handle, int currentSpriteID)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            return;
        }
        if (currentSpriteID != spriteID)
        {
            return;
        }
        image.sprite = handle.Result;
        image.type = type;
    }

    public void SetSprite(int spriteID)
    {
        this.spriteID = spriteID;
        LoadSprite();
    }

    public void SetItemSprite(int itemID)
    {
        var itemData = DataTableManager.ItemTable.GetData(itemID);
        if (itemData is not null)
        {
            this.spriteID = itemData.SpriteID;
            LoadSprite();
        }
    }

    public void SetSprite(int spriteID, Image.Type type)
    {
        this.type = type;
        this.spriteID = spriteID;
        LoadSprite();
    }

    public void SetEnabled(bool isOn)
    {
        image.enabled = isOn;
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }
}
