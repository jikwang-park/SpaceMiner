using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEditorOnlyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject viewer;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F10))
        {
            if (viewer.activeSelf)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }

            viewer.SetActive(!viewer.activeSelf);
        }
    }
}
