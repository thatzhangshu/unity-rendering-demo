using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HighlightTarget : MonoBehaviour
{
    [Header("Highlight Settings")]
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float emissionIntensity = 5.0f;

    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private Color _originalBaseColor = Color.white;
    private bool _hasOriginalBaseColor;
    private bool _isHighlighted;

    public bool IsHighlighted => _isHighlighted;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();

        CacheOriginalMaterialValues();
    }

    private void CacheOriginalMaterialValues()
    {
        if (_renderer == null || _renderer.sharedMaterial == null)
            return;

        Material mat = _renderer.sharedMaterial;

        if (mat.HasProperty(BaseColorId))
        {
            _originalBaseColor = mat.GetColor(BaseColorId);
            _hasOriginalBaseColor = true;
        }
    }

    public void SetHighlight(bool highlighted)
    {
        _isHighlighted = highlighted;

        _renderer.GetPropertyBlock(_propertyBlock);

        if (highlighted)
        {
            if (_hasOriginalBaseColor)
            {
                Color boostedColor = _originalBaseColor * 1.2f;
                _propertyBlock.SetColor(BaseColorId, boostedColor);
            }

            Color emission = highlightColor * emissionIntensity;
            _propertyBlock.SetColor(EmissionColorId, emission);
        }
        else
        {
            if (_hasOriginalBaseColor)
            {
                _propertyBlock.SetColor(BaseColorId, _originalBaseColor);
            }

            _propertyBlock.SetColor(EmissionColorId, Color.black);
        }

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}