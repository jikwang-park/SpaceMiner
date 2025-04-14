using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        var imageAddress = DataTableManager.AddressTable.GetData(spriteID);
        if (string.IsNullOrEmpty(imageAddress))
        {
            return;
        }

        Addressables.LoadAssetAsync<Sprite>(imageAddress).Completed += AddressableIcon_Completed;
    }

    private void AddressableIcon_Completed(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
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


    public void SetSprite(int spriteID, Image.Type type)
    {
        this.type = type;
        this.spriteID = spriteID;
        LoadSprite();
    }
}
