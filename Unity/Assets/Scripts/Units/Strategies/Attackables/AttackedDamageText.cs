using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AttackedDamageText : MonoBehaviour, IAttackable
{
    private const string textPrefabAddress = "Assets/Addressables/Prefabs/UI/DamageText.prefab";

    public float showingDuration = 1.5f;
    public float risingSpeed = 2f;

    private StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        var textObject = stageManager.ObjectPoolManager.Get(textPrefabAddress);
        textObject.transform.SetParent(null);
        textObject.transform.position = transform.position;
        textObject.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        DamageText damageText = textObject.GetComponent<DamageText>();
        damageText.SetSpeed(showingDuration, risingSpeed);
        damageText.SetText(attack);
        damageText.Show();
    }
}

