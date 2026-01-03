using UnityEngine;

public static class Constants
{
    public enum COLOR
    {
        RED,
        GREEN,
        BLUE,
        YELLOW,
        ORANGE,
        PURPLE,
        BROWN,
        WHITE
    }

    public static Color GetColorString(COLOR color)
    {
        string hex = color switch
        {
            COLOR.RED    => "#E74C3C",
            COLOR.GREEN  => "#2ECC71",
            COLOR.BLUE   => "#3498DB",
            COLOR.YELLOW => "#F1C40F",
            COLOR.ORANGE => "#E67E22",
            COLOR.PURPLE => "#9B59B6",
            COLOR.BROWN  => "#8B4513",
            _            => "#FFFFFF" // Mặc định (default)
        };
        if (ColorUtility.TryParseHtmlString(hex, out Color result))
        {
            return result;
        }
        return Color.white;
    }
}


