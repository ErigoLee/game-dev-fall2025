using UnityEngine;
using Oculus.Interaction;

public class PokeMaterialByInteractor : MonoBehaviour
{
    [SerializeField] private PokeInteractor interactor;
    [SerializeField] private Material pressedMat;
    private PokeInteractable _lastSelected;
    private Material _lastDefaultMat;
    private Renderer _lastRenderer;

    void Update()
    {
        // Check if the hand is in the Select state
        if (interactor && interactor.State == InteractorState.Select)
        {
            var cur = interactor.Interactable; // The currently selected PokeInteractable
            if (cur != _lastSelected)
            {
                RestoreLast();
                _lastSelected = cur;
                _lastRenderer = cur ? cur.GetComponentInChildren<Renderer>(true) : null;
                _lastDefaultMat = _lastRenderer ? _lastRenderer.material : null;
            }

            if (_lastRenderer && pressedMat && _lastRenderer.material != pressedMat)
                _lastRenderer.material = pressedMat;
        }
        else
        {
            // Restore the material when the selection is released
            RestoreLast();
        }
    }

    private void RestoreLast()
    {
        if (_lastRenderer && _lastDefaultMat)
            _lastRenderer.material = _lastDefaultMat;
        _lastSelected = null;
        _lastRenderer = null;
        _lastDefaultMat = null;
    }
}
