using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonRewardRow : MonoBehaviour
{
    [field: SerializeField]
    public AddressableImage icon { get; private set; }
    [field: SerializeField]
    public TextMeshProUGUI text { get; private set; }
}
