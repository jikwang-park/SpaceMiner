using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private FirebaseManager firebaseManager;
    private void Awake()
    {
        firebaseManager = FirebaseManager.Instance;
    }
    public void OnClickStartButton()
    {
        FirebaseManager.Instance.OnClickStartButton();
    }
}
