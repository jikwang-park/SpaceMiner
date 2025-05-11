using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nickName;
    [SerializeField]
    private TextMeshProUGUI id;

    private void Start()
    {
        nickName.text = FirebaseManager.Instance.User.DisplayName;
        //id.text = FirebaseManager.Instance.User.UserId;
    }
}
