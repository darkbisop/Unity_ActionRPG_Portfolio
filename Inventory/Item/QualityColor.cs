using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ItemQuality { Common, Rare, Unique, Epic, Legendary, Total }

public static class QualityColor
{
    private static Dictionary<ItemQuality, string> colors = new Dictionary<ItemQuality, string>()
    {
        {ItemQuality.Common, "#ffffffff" },
        {ItemQuality.Rare, "#2aff00" },
        {ItemQuality.Unique, "#1b26ff" },
        {ItemQuality.Epic, "#7200ff" },
        {ItemQuality.Legendary, "#ff0000" }
    };

    public static Dictionary<ItemQuality, string> MyColor { get => colors; }
}