using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeMaterialByInteraction : MonoBehaviour
{
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material pressMat;
    [SerializeField] private Renderer pokeCubeRender;

    public void OnPressAct()
    {
        pokeCubeRender.material = pressMat;
    }

    public void OnUnPressAct()
    {
        pokeCubeRender.material = defaultMat;
    }
}
