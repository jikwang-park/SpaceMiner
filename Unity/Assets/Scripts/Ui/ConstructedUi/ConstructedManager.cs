using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructedManager : MonoBehaviour
{
    [SerializeField]
    private ConstructionElement element;
    [SerializeField]
    private Transform parentTransform;








    private void InIt()
    {
        for (int i = 0; i < 5; ++i)
        {
            Instantiate(element);
        }
    }
}
