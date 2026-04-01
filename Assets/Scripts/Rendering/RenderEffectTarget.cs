using UnityEngine;

[RequireComponent(typeof(SelectableObject))]
public class RenderEffectTarget : MonoBehaviour
{
    [Header("Optional References")]
    [SerializeField] private HighlightTarget highlightTarget;
    [SerializeField] private OutlineTarget outlineTarget;
    [SerializeField] private DissolveController dissolveController;

    public HighlightTarget HighlightTarget => highlightTarget;
    public OutlineTarget OutlineTarget => outlineTarget;
    public DissolveController DissolveController => dissolveController;

    private void Reset()
    {
        if (highlightTarget == null)
            highlightTarget = GetComponent<HighlightTarget>();

        if (outlineTarget == null)
            outlineTarget = GetComponent<OutlineTarget>();

        if (dissolveController == null)
            dissolveController = GetComponent<DissolveController>();
    }

    public void SetSelectedVisual(bool selected)
    {
        if (highlightTarget != null)
            highlightTarget.SetHighlight(selected);

        if (outlineTarget != null)
            outlineTarget.SetOutline(selected);
    }

    public void PlayDissolve()
    {
        if (dissolveController != null)
            dissolveController.PlayDissolve();
    }

    public void ResetDissolve()
    {
        if (dissolveController != null)
            dissolveController.ResetDissolve();
    }
}