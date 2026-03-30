using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask raycastMask = ~0;
    [SerializeField] private HighlightController highlightController;
    [SerializeField] private OutlineController outlineController;

    private SelectableObject _currentSelection;

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
            SelectableObject selectable = hit.collider.GetComponent<SelectableObject>();
            if (selectable != null)
            {
                _currentSelection = selectable;
                Debug.Log($"Selected: {_currentSelection.DisplayName}");

                HighlightTarget highlightTarget = hit.collider.GetComponent<HighlightTarget>();
                if (highlightTarget != null && highlightController != null)
                {
                    highlightController.SetCurrentTarget(highlightTarget);
                }

                OutlineTarget outlineTarget = hit.collider.GetComponent<OutlineTarget>();
                if (outlineTarget != null && outlineController != null)
                {
                    outlineController.SetCurrentTarget(outlineTarget);
                }

                return;
            }
        }

        _currentSelection = null;

        if (highlightController != null)
        {
            highlightController.ClearCurrentTarget();
        }

        if (outlineController != null)
        {
            outlineController.ClearCurrentTarget();
        }

        Debug.Log("Selection cleared.");
    }
}