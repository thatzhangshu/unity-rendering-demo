using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public sealed class OutlineMaskPass:ScriptableRenderPass, IDisposable
{
    private const string ProfilerTag = "Outline Mask Pass";
    private const string MaskTextureName = "_ScreenSpaceOutlineMask";

    private static readonly int MaskTextureId = Shader.PropertyToID(MaskTextureName);

    private readonly Material _maskMaterial;
    private readonly ProfilingSampler _profilingSampler;
    private readonly List<Material> _sharedMaterials = new();

    private RTHandle _maskTexture;

    public RTHandle MaskTexture => _maskTexture;

    public OutlineMaskPass(Material maskMaterial,RenderPassEvent passEvent)
    {
        _maskMaterial = maskMaterial;
        _profilingSampler = new ProfilingSampler(ProfilerTag);
        renderPassEvent = passEvent;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        descriptor.depthBufferBits = 0;
        descriptor.msaaSamples = 1;
        descriptor.useMipMap = false;
        descriptor.autoGenerateMips = false;
        descriptor.graphicsFormat = GraphicsFormat.R8_UNorm;

        RenderingUtils.ReAllocateIfNeeded(
            ref _maskTexture,
            descriptor,
            FilterMode.Point,
            TextureWrapMode.Clamp,
            name: MaskTextureName);

        ConfigureTarget(_maskTexture);
        ConfigureClear(ClearFlag.Color, Color.black);
    }
    private static bool CanRenderTarget(OutlineTarget target, out Renderer targetRenderer)
    {
        targetRenderer = null;

        if (target == null || !target.isActiveAndEnabled || !target.IsOutlined)
        {
            return false;
        }

        targetRenderer = target.TargetRenderer;

        return targetRenderer != null &&
               targetRenderer.enabled &&
               targetRenderer.gameObject.activeInHierarchy;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (_maskMaterial == null || _maskTexture == null)
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get(ProfilerTag);

        try
        {
            using (new ProfilingScope(cmd, _profilingSampler))
            {
                foreach (OutlineTarget target in OutlineTargetRegistry.Targets)
                {
                    if (!CanRenderTarget(target, out Renderer targetRenderer))
                    {
                        continue;
                    }

                    _sharedMaterials.Clear();
                    targetRenderer.GetSharedMaterials(_sharedMaterials);

                    int subMeshCount =
                        Mathf.Max(1, _sharedMaterials.Count);

                    for (int subMeshIndex = 0; subMeshIndex < subMeshCount; subMeshIndex++)
                    {
                        cmd.DrawRenderer(targetRenderer, _maskMaterial, subMeshIndex,0);
                    }
                }

                cmd.SetGlobalTexture(MaskTextureId, _maskTexture.nameID);
            }

            context.ExecuteCommandBuffer(cmd);
        }
        finally
        {
            CommandBufferPool.Release(cmd);
        }
    }

    public void Dispose()
    {
        _maskTexture?.Release();
        _maskTexture = null;
    }
}