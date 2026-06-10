using UnityEditor;
using UnityEngine;

public static class Require
{
    public static T Component<T>(T component, string name) where T : Object
    {
        // In the editor you get a warning that’s easy to spot.
#if UNITY_EDITOR
        if (component == null)
            Debug.LogError($"<b>{name}</b> is missing on <i>{ObjectNames.NicifyVariableName(name)}</i> ({component?.GetType().Name ?? "null"})");
#endif
        // In a build you can choose to throw or just return null.
        return component;
    }
}
