using Firebase.Crashlytics;
using Sirenix.Serialization;
using UnityEngine;

public static class Logger
{
    public static void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#else
        Crashlytics.Log(message);
#endif
    }

    public static void LogError(System.Exception exception)
    {
#if UNITY_EDITOR
        Debug.LogError(exception);
#else
        Crashlytics.LogException(exception);
#endif
    }

}