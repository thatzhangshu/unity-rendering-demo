using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RenderEffectPresenter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI selectedNameText;
    [SerializeField] private TextMeshProUGUI hintText;

    private RenderEffectTarget _currentTarget;

    public RenderEffectTarget CurrentTarget => _currentTarget;

    private void Start()
    {
        RefreshUI();
    }

    public void SetCurrentTarget(RenderEffectTarget newTarget)
    {
        if (_currentTarget == newTarget)
            return;

        ClearCurrentTargetVisual();

        _currentTarget = newTarget;

        if (_currentTarget != null)
        {
            _currentTarget.SetSelectedVisual(true);
        }

        RefreshUI();
    }

    public void ClearCurrentTarget()
    {
        ClearCurrentTargetVisual();
        _currentTarget = null;
        RefreshUI();
    }

    public void TriggerDissolveOnCurrent()
    {
        if (_currentTarget != null)
        {
            _currentTarget.SetSelectedVisual(false);
            _currentTarget.PlayDissolve();
        }
    }

    public void ResetDissolveOnCurrent()
    {
        if (_currentTarget != null)
        {
            _currentTarget.ResetDissolve();
            _currentTarget.SetSelectedVisual(true);
        }
    }

    private void ClearCurrentTargetVisual()
    {
        if (_currentTarget != null)
        {
            _currentTarget.SetSelectedVisual(false);
        }
    }

    private void RefreshUI()
    {
        if (selectedNameText != null)
        {
            selectedNameText.text = _currentTarget != null
                ? $"Selected: {_currentTarget.GetComponent<SelectableObject>().DisplayName}"
                : "Selected: None";
        }

        if (hintText != null)
        {
            hintText.text = "Left Click: Select | F: Dissolve | G: Reset Dissolve";
        }
    }

    private void Update()
    {
        if (_currentTarget == null)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            TriggerDissolveOnCurrent();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ResetDissolveOnCurrent();
        }
    }
}