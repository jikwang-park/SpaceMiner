using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AttackedDamageText : MonoBehaviour, IAttackable
{
    public string textPrefabAddress = "Prefabs/UI/DamageText";

    public float showingDuration = 1.5f;
    public float risingSpeed = 2f;

    public void OnAttack(GameObject attacker, Attack attack)
    {
        var handle = Addressables.InstantiateAsync(textPrefabAddress, transform.position, Quaternion.LookRotation(Camera.main.transform.forward));
        handle.WaitForCompletion();
        TextMeshPro text = handle.Result.GetComponent<TextMeshPro>();
        text.text = attack.damage.ToString();
        text.color = attack.isCritical ? Color.red : Color.blue;
        text.StartCoroutine(coText(text));
    }

    private IEnumerator coText(TextMeshPro text)
    {
        float timer = 0f;
        while (timer < showingDuration)
        {
            timer += Time.deltaTime;
            var pos = text.transform.position;
            pos.y += risingSpeed * Time.deltaTime;
            text.transform.position = pos;
            yield return null;
        }
        Destroy(text.gameObject);
    }
}

