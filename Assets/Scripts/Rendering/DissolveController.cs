using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveController : MonoBehaviour
{
    [SerializeField] private float dissolveSpeed = 0.5f;
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private OutlineTarget outlineTarget;

private Renderer _renderer;
    private MaterialPropertyBlock _block;

    private static readonly int DissolveAmountId = Shader.PropertyToID("_DissolveAmount");

    private float _currentAmount = 0f;
    private bool _isPlaying = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _block = new MaterialPropertyBlock();
        ApplyDissolveAmount();
    }

    private void Start()
    {
        if (playOnStart)
        {
            PlayDissolve();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetDissolve();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayDissolve();
        }

        if (_isPlaying)
        {
            _currentAmount += dissolveSpeed * Time.deltaTime;
            _currentAmount = Mathf.Clamp01(_currentAmount);

            ApplyDissolveAmount();

            if (_currentAmount >= 1f)
            {
                _isPlaying = false;
            }
        }
    }

    public void PlayDissolve()
    {
        _isPlaying = true;

        if (outlineTarget != null)
        {
            outlineTarget.SetOutline(false);
        }
    }

    public void ResetDissolve()
    {
        _isPlaying = false;
        _currentAmount = 0f;
        ApplyDissolveAmount();
    }

    private void ApplyDissolveAmount()
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetFloat(DissolveAmountId, _currentAmount);
        _renderer.SetPropertyBlock(_block);
    }
}