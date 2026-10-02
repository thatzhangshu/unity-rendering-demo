using UnityEngine;

[DisallowMultipleComponent]
public sealed class OutlineTarget : MonoBehaviour
{
    [SerializeField]
    private Renderer targetRenderer;

    private bool _isOutlined;

    public Renderer TargetRenderer => targetRenderer;
    public bool IsOutlined => _isOutlined;

    private void Awake()
    {
        ResolveRenderer();
    }

    private void OnEnable()
    {
        ResolveRenderer();

        if (_isOutlined)
        {
            OutlineTargetRegistry.Register(this);
        }
    }

    private void OnDisable()
    {
        OutlineTargetRegistry.Unregister(this);
        _isOutlined = false;
    }

    private void OnValidate()
    {
        ResolveRenderer();
    }

    private void Reset()
    {
        ResolveRenderer();
    }

    public void SetOutline(bool visible)
    {
        _isOutlined = visible;

        if (visible && isActiveAndEnabled)
        {
            OutlineTargetRegistry.Register(this);
        }
        else
        {
            OutlineTargetRegistry.Unregister(this);
        }
    }

    private void ResolveRenderer()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
    }
}