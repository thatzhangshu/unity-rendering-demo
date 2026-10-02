using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public sealed class OutlineCompositePass : ScriptableRenderPass
{
    private const string ProfilerTag = "Outline Composite Pass";

    private static readonly int OutlineColorId =
        Shader.PropertyToID("_OutlineColor");

    private static readonly int OutlineWidthId =
        Shader.PropertyToID("_OutlineWidth");

    private readonly Material _compositeMaterial;
    private readonly MaterialPropertyBlock _properties;
    private readonly ProfilingSampler _profilingSampler;

    private RTHandle _cameraColorTarget;

    public OutlineCompositePass(
        Material compositeMaterial,
        RenderPassEvent passEvent)
    {
        _compositeMaterial = compositeMaterial;
        _properties = new MaterialPropertyBlock();
        _profilingSampler = new ProfilingSampler(ProfilerTag);

        renderPassEvent = passEvent;
    }

    public void Setup(
        RTHandle cameraColorTarget,
        Color outlineColor,
        float outlineWidth)
    {
        _cameraColorTarget = cameraColorTarget;

        _properties.Clear();
        _properties.SetColor(OutlineColorId, outlineColor);
        _properties.SetFloat(OutlineWidthId, outlineWidth);
    }

    public override void OnCameraSetup(
        CommandBuffer cmd,
        ref RenderingData renderingData)
    {
        if (_cameraColorTarget == null)
        {
            return;
        }

        ConfigureTarget(_cameraColorTarget);
        ConfigureClear(ClearFlag.None, Color.clear);
    }

    public override void Execute(
        ScriptableRenderContext context,
        ref RenderingData renderingData)
    {
        if (_compositeMaterial == null ||
            _cameraColorTarget == null)
        {
            return;
        }

        CommandBuffer cmd =
            CommandBufferPool.Get(ProfilerTag);

        try
        {
            using (new ProfilingScope(cmd, _profilingSampler))
            {
                CoreUtils.DrawFullScreen(
                    cmd,
                    _compositeMaterial,
                    _properties);
            }

            context.ExecuteCommandBuffer(cmd);
        }
        finally
        {
            CommandBufferPool.Release(cmd);
        }
    }
}