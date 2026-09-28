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
    }

    [SerializeField] private Settings settings = new();

    private Material _maskMaterial;
    private OutlineMaskPass _maskPass;

    public override void Create()
    {
        ReleaseResources();

        if (settings.maskShader == null)
        {
            return;
        }

        _maskMaterial = CoreUtils.CreateEngineMaterial(settings.maskShader);

        _maskPass = new OutlineMaskPass(
            _maskMaterial,
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

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_maskPass == null ||
            _maskMaterial == null ||
            !IsSupportedCamera(ref renderingData))
        {
            return;
        }

        renderer.EnqueuePass(_maskPass);
    }

    private void ReleaseResources()
    {
        _maskPass?.Dispose();
        _maskPass = null;

        CoreUtils.Destroy(_maskMaterial);
        _maskMaterial = null;
    }

    protected override void Dispose(bool disposing)
    {
        ReleaseResources();
        base.Dispose(disposing);
    }
}