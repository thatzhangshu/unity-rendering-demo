using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask raycastMask = ~0;
    [SerializeField] private RenderEffectPresenter renderEffectPresenter;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectObject();
        }
    }

    private void TrySelectObject()
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("SelectionManager: Main Camera is not assigned.");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, raycastMask))
        {
            RenderEffectTarget effectTarget = hit.collider.GetComponent<RenderEffectTarget>();
            if (effectTarget != null)
            {
                renderEffectPresenter?.SetCurrentTarget(effectTarget);
                Debug.Log($"Selected: {effectTarget.GetComponent<SelectableObject>().DisplayName}");
                return;
            }
        }

        renderEffectPresenter?.ClearCurrentTarget();
        Debug.Log("Selection cleared.");
    }
}