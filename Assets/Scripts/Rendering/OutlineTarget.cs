using UnityEngine;

public class OutlineTarget : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private GameObject outlineObject;

    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private bool useLegacyOutline = true;

    private bool _isOutlined;

    public Renderer TargetRenderer => targetRenderer;
    public Color OutlineColor => outlineColor;
    public bool IsOutlined => _isOutlined;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
    }

    private void OnValidate()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
    }
    private void OnEnable()
    {
        OutlineTargetRegistry.Register(this);
    }

    private void OnDisable()
    {
        OutlineTargetRegistry.Unregister(this);
    }

    public void SetOutline(bool visible)
    {
        _isOutlined = visible;

        if (useLegacyOutline && outlineObject != null)
        {
            outlineObject.SetActive(visible);
        }
    }

    private void Reset()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
    }
}