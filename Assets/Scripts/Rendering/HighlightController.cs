using UnityEngine;

public class HighlightController : MonoBehaviour
{
    private HighlightTarget _currentTarget;

    public void SetCurrentTarget(HighlightTarget newTarget)
    {
        if (_currentTarget == newTarget)
            return;

        ClearCurrentTarget();

        _currentTarget = newTarget;

        if (_currentTarget != null)
        {
            _currentTarget.SetHighlight(true);
        }
    }

    public void ClearCurrentTarget()
    {
        if (_currentTarget != null)
        {
            _currentTarget.SetHighlight(false);
            _currentTarget = null;
        }
    }


    public HighlightTarget GetCurrentTarget()
    {
        return _currentTarget;
    }
}