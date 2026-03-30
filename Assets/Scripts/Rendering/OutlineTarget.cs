using UnityEngine;

public class OutlineTarget : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private GameObject outlineObject;

    public void SetOutline(bool visible)
    {
        if (outlineObject != null)
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