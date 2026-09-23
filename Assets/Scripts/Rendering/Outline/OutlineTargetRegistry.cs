using System.Collections.Generic;
using UnityEngine;

public static class OutlineTargetRegistry
{
    private static readonly HashSet<OutlineTarget> TargetsInternal = new();

    public static IReadOnlyCollection<OutlineTarget> Targets => TargetsInternal;

    public static void Register(OutlineTarget target)
    {
        if (target != null)
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