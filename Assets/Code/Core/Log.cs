using UnityEngine;

public static class Log
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public static void Info(string msg) => Debug.Log(msg);
    public static void Warn(string msg) => Debug.LogWarning(msg);
    public static void Error(string msg) => Debug.LogError(msg);
#else
    public static void Info(string msg) { }
    public static void Warn(string msg) { }
    public static void Error(string msg) { }
#endif
}