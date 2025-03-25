using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectWindow : MonoBehaviour
{
    [SerializeField]
    private PlanetSelectScroll planetScroll;

    [SerializeField]
    private StageSelectScroll stageScroll;


    private void Start()
    {
        planetScroll.OnPlanetSelected += stageScroll.SetButtons;
    }
}
