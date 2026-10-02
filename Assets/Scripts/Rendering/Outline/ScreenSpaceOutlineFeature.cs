using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public sealed class ScreenSpaceOutlineFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public sealed class Settings
    {
        [Tooltip("Shader used to render selected objects into the mask texture.")]
        public Shader maskShader;

        [Tooltip("Point in the URP pipeline where the mask pass executes.")]
        public RenderPassEvent injectionPoint = RenderPassEvent.AfterRenderingOpaques;

        [Tooltip("Shader used to extract and composite the outline.")]
        public Shader compositeShader;

        [ColorUsage(false, true)]
        public Color outlineColor = new Color(0.0f, 0.85f, 1.0f, 1.0f);

        [Range(1.0f, 4.0f)]
        public float outlineWidth = 2.0f;

        [Tooltip("Controls whether outlines remain visible behind opaque geometry.")]
        public OutlineOcclusionMode occlusionMode =
            OutlineOcclusionMode.Always;

        [Tooltip("Eye-space tolerance used when comparing target and scene depth.")]
        [Min(0.0f)]
        public float depthBias = 0.01f;
    }

    [SerializeField] private Settings settings = new();

    private Material _maskMaterial;
    private OutlineMaskPass _maskPass;
    private Material _compositeMaterial;
    private OutlineCompositePass _compositePass;

    private bool _runtimeEnabled;
    private float _runtimeOutlineWidth;
    private OutlineOcclusionMode _runtimeOcclusionMode;

    public bool IsRuntimeEnabled => _runtimeEnabled;
    public float RuntimeOutlineWidth => _runtimeOutlineWidth;

    public OutlineOcclusionMode RuntimeOcclusionMode =>
        _runtimeOcclusionMode;

    public override void Create()
    {
        ReleaseResources();
        ResetRuntimeState();

        if (settings.maskShader == null ||
            settings.compositeShader == null)
        {
            return;
        }

        _maskMaterial =
            CoreUtils.CreateEngineMaterial(settings.maskShader);

        _compositeMaterial =
            CoreUtils.CreateEngineMaterial(settings.compositeShader);

        _maskPass = new OutlineMaskPass(
            _maskMaterial,
            settings.injectionPoint);

        _compositePass = new OutlineCompositePass(
            _compositeMaterial,
            settings.injectionPoint);

    }

    private static bool IsSupportedCamera(ref RenderingData renderingData)
    {
        CameraData cameraData = renderingData.cameraData;

        if (cameraData.renderType == CameraRenderType.Overlay)
        {
            return false;
        }

        CameraType cameraType = cameraData.cameraType;

        return cameraType == CameraType.Game ||
               cameraType == CameraType.SceneView;
    }

    public override void AddRenderPasses(
        ScriptableRenderer renderer, 
        ref RenderingData renderingData)
    {
        if (!_runtimeEnabled ||
            !OutlineTargetRegistry.HasOutlinedTargets ||
            _maskPass == null ||
            _compositePass == null ||
            _maskMaterial == null ||
            _compositeMaterial == null ||
            !IsSupportedCamera(ref renderingData))
        {
            return;
        }

        _maskPass.ConfigureOcclusion(
            _runtimeOcclusionMode,
            settings.depthBias);

        renderer.EnqueuePass(_maskPass);
        renderer.EnqueuePass(_compositePass);
    }

    public override void SetupRenderPasses(
        ScriptableRenderer renderer, 
        in RenderingData renderingData)
    {
        if (!_runtimeEnabled || _compositePass == null || _maskPass == null)
        {
            return;
        }

        CameraData cameraData = renderingData.cameraData;

        if (cameraData.renderType == CameraRenderType.Overlay)
        {
            return;
        }

        CameraType cameraType = cameraData.cameraType;

        if (cameraType != CameraType.Game &&
            cameraType != CameraType.SceneView)
        {
            return;
        }

        _compositePass.Setup(
            renderer.cameraColorTargetHandle, 
            settings.outlineColor,
            _runtimeOutlineWidth);
    }
    
    private void ReleaseResources()
    {
        _maskPass?.Dispose();
        _maskPass = null;
        _compositePass = null;

        CoreUtils.Destroy(_maskMaterial);
        CoreUtils.Destroy(_compositeMaterial);

        _maskMaterial = null;
        _compositeMaterial = null;
    }

    protected override void Dispose(bool disposing)
    {
        ReleaseResources();
        base.Dispose(disposing);
    }

    public void ResetRuntimeState()
    {
        _runtimeEnabled = true;
        _runtimeOutlineWidth = settings.outlineWidth;
        _runtimeOcclusionMode = settings.occlusionMode;
    }

    public void SetRuntimeEnabled(bool enabled)
    {
        _runtimeEnabled = enabled;
    }

    public void SetRuntimeOutlineWidth(float width)
    {
        _runtimeOutlineWidth =
            Mathf.Clamp(width, 1.0f, 4.0f);
    }

    public void SetRuntimeOcclusionMode(OutlineOcclusionMode occlusionMode)
    {
        _runtimeOcclusionMode = occlusionMode;
    }
}