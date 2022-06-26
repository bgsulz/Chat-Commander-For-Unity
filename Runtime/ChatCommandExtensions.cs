using System;

using System.Collections.Generic;
using UnityEngine;

namespace BGSulz.ChatCommander
{
    public static class ChatCommandExtensions
    {
        public static int ToInt(this string self, int or = -1) => self != null && int.TryParse(self, out var res) ? res : or;
        public static int? ToIntOrNull(this string self) => self != null && int.TryParse(self, out var res) ? res : null;
        
        public static float ToFloat(this string self, float or = -1) => self != null && float.TryParse(self, out var res) ? res : or;
        public static float? ToFloatOrNull(this string self) => self != null && float.TryParse(self, out var res) ? res : null;
        
        public static bool ToBool(this string self, bool or = false) => self != null && bool.TryParse(self, out var res) ? res : or;
        public static bool? ToBoolOrNull(this string self) => self != null && bool.TryParse(self, out var res) ? res : null;

        public static Color ToColor(this string self, Color or = default) => self != null && CommandColor.TryParse(self, out var res) ? res : or;
        public static Color? ToColorOrNull(this string self) => self != null && CommandColor.TryParse(self, out var res) ? res : null;

        public static int Clamped(this int self, int min = int.MinValue, int max = int.MaxValue) => Mathf.Clamp(self, min, max);
        public static float Clamped(this float self, float min = float.NegativeInfinity, float max = float.PositiveInfinity) => Mathf.Clamp(self, min, max);
    }

    public static class CommandColor
    {
        public static readonly Dictionary<string, Color> StockColors = new()
        {
            ["red"] = Color.red,
            ["yellow"] = Color.yellow,
            ["white"] = Color.white,
            ["green"] = Color.green,
            ["blue"] = Color.blue,
            ["magenta"] = Color.magenta,
            ["cyan"] = Color.cyan,
            ["black"] = Color.black,
            ["white"] = Color.white,
            ["gray"] = Color.gray,
            ["grey"] = Color.grey,
            ["clear"] = Color.clear
        };

        public static bool TryParse(string self, out Color result)
        {
            if (StockColors.TryGetValue(self, out var stockRes))
            {
                result = stockRes;
                return true;
            }

            if (ColorUtility.TryParseHtmlString(self, out var htmlRes))
            {
                result = htmlRes;
                return true;
            }

            result = default;
            return false;
        }
    }
}