using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private OutlineTarget _currentTarget;

    public void SetCurrentTarget(OutlineTarget newTarget)
    {
        if (_currentTarget == newTarget)
            return;

        ClearCurrentTarget();

        _currentTarget = newTarget;

        if (_currentTarget != null)
        {
            _currentTarget.SetOutline(true);
        }
    }

    public void ClearCurrentTarget()
    {
        if (_currentTarget != null)
        {
            _currentTarget.SetOutline(false);
            _currentTarget = null;
        }
    }
}