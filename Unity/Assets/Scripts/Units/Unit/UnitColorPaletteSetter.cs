using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitColorPaletteSetter : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<Grade, UnitColorPalette> GradeColor;

    [SerializeField]
    private SkinnedMeshRenderer targetSkin;

    public void SetColor(Grade grade, UnitTypes type)
    {
        var materials = GradeColor[grade].GetMaterial(type);
        for (int i = 0; i < materials.Length; ++i)
        {
            targetSkin.materials = materials;
        }
    }

#if UNITY_EDITOR
    [SerializeField]
    private UnitTypes type;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetColor(Grade.Normal, type);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetColor(Grade.Rare, type);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetColor(Grade.Epic, type);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetColor(Grade.Legend, type);
        }
    }
#endif
}
