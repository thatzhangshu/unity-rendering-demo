using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public sealed class OutlineDemoController : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField]
    private UniversalRendererData rendererData;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI statusText;

    [Header("Controls")]
    [SerializeField]
    [Min(0.1f)]
    private float widthStep = 0.5f;

    private ScreenSpaceOutlineFeature _outlineFeature;

    private void Awake()
    {
        _outlineFeature = FindOutlineFeature();

        if (_outlineFeature == null)
        {
            Debug.LogError(
                "OutlineDemoController: " +
                "ScreenSpaceOutlineFeature was not found.");

            enabled = false;
            return;
        }

        _outlineFeature.ResetRuntimeState();
        RefreshStatus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleEffect();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleOcclusionMode();
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            DecreaseWidth();
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            IncreaseWidth();
        }
    }

    public void ToggleEffect()
    {
        if (_outlineFeature == null)
        {
            return;
        }

        _outlineFeature.SetRuntimeEnabled(
            !_outlineFeature.IsRuntimeEnabled);

        RefreshStatus();
    }

    public void ToggleOcclusionMode()
    {
        if (_outlineFeature == null)
        {
            return;
        }

        OutlineOcclusionMode nextMode =
            _outlineFeature.RuntimeOcclusionMode ==
            OutlineOcclusionMode.Always
                ? OutlineOcclusionMode.VisibleOnly
                : OutlineOcclusionMode.Always;

        _outlineFeature.SetRuntimeOcclusionMode(nextMode);
        RefreshStatus();
    }

    public void IncreaseWidth()
    {
        ChangeWidth(widthStep);
    }

    public void DecreaseWidth()
    {
        ChangeWidth(-widthStep);
    }

    private void ChangeWidth(float delta)
    {
        if (_outlineFeature == null)
        {
            return;
        }

        _outlineFeature.SetRuntimeOutlineWidth(
            _outlineFeature.RuntimeOutlineWidth + delta);

        RefreshStatus();
    }

    private ScreenSpaceOutlineFeature FindOutlineFeature()
    {
        if (rendererData == null)
        {
            Debug.LogError(
                "OutlineDemoController: Renderer Data is not assigned.");

            return null;
        }

        foreach (ScriptableRendererFeature feature
                 in rendererData.rendererFeatures)
        {
            if (feature is ScreenSpaceOutlineFeature outlineFeature)
            {
                return outlineFeature;
            }
        }

        return null;
    }

    private void RefreshStatus()
    {
        if (statusText == null ||
            _outlineFeature == null)
        {
            return;
        }

        string enabledText =
            _outlineFeature.IsRuntimeEnabled
                ? "ON"
                : "OFF";

        string modeText =
            _outlineFeature.RuntimeOcclusionMode ==
            OutlineOcclusionMode.Always
                ? "Always"
                : "Visible Only";

        statusText.text =
            $"Screen Outline: {enabledText}\n" +
            $"Mode: {modeText} | " +
            $"Width: {_outlineFeature.RuntimeOutlineWidth:F1}px";
    }
}