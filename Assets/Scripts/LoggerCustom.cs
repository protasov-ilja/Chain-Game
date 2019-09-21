using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace ProjectName
{
    public static class Logger
    {
        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]  
        public static void Print(string str)
        {
            Debug.Log(str);
        }
    
        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]  
        public static void Print(string str, string color)
        {
            Debug.Log($"<color={color}>{str}</color>");
        }
    }
}