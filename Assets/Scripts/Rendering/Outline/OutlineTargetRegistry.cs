using System.Collections.Generic;
using UnityEngine;

public static class OutlineTargetRegistry
{
    private static readonly List<OutlineTarget> TargetsInternal = new();

    public static IReadOnlyList<OutlineTarget> Targets =>
        TargetsInternal;

    public static bool HasOutlinedTargets => TargetsInternal.Count > 0;

    public static void Register(OutlineTarget target)
    {
        if (target != null && !TargetsInternal.Contains(target))
        {
            TargetsInternal.Add(target);
        }
    }

    public static void Unregister(OutlineTarget target)
    {
        if (target != null)
        {
            TargetsInternal.Remove(target);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRegistry()
    {
        TargetsInternal.Clear();
    }
}