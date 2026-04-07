using UnityEngine;

public class PBRDebugController : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;

    private void Update()
    {
        if (targetMaterial == null) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            float m = targetMaterial.GetFloat("_Metallic");
            targetMaterial.SetFloat("_Metallic", Mathf.Clamp01(m + Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            float m = targetMaterial.GetFloat("_Metallic");
            targetMaterial.SetFloat("_Metallic", Mathf.Clamp01(m - Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            float s = targetMaterial.GetFloat("_Smoothness");
            targetMaterial.SetFloat("_Smoothness", Mathf.Clamp01(s + Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float s = targetMaterial.GetFloat("_Smoothness");
            targetMaterial.SetFloat("_Smoothness", Mathf.Clamp01(s - Time.deltaTime));
        }
    }
}