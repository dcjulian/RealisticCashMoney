using System;
using UnityEngine;

namespace RealisticCashMoney
{
    internal static class Dbg
    {
        private static readonly string Prefix = $"[{ModInfo.FullName}] ";

        public static void Log(string msg)
        {
            Debug.Log(Prefix + msg);
        }

        public static void Warn(string msg)
        {
            Debug.LogWarning(Prefix + msg);
        }

        public static void Err(string msg, Exception e)
        {
            Debug.LogError($"{Prefix}{msg} Exception Follows.");
            Debug.LogException(e);
        }
    }
}