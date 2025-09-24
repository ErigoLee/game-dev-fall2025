using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class PokeInteraction : MonoBehaviour
{
    [SerializeField] private PokeInteractor pokeInteractor;
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material activeMat;

    private bool isPoke;
    [SerializeField] private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        isPoke = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(pokeInteractor != null)
        {
            isPoke = pokeInteractor.IsPassedSurface;
            if(!isPoke)
            {
                renderer.material = defaultMat;
            }
            else{
                renderer.material = activeMat;
            }
        }
    }
}
