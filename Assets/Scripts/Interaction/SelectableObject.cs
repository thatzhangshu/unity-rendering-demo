using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private string displayName = "Selectable Object";

    public string DisplayName => displayName;
}