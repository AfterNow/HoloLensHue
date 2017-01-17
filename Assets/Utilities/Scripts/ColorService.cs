using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public static class ColorService
{
    static Vector4 color;
    static int hueValue;

    public enum Colors
    {
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet
    }

    public static Vector4 GetColorByHue(int hue)
    {
        // red
        if (hue < 5000 || hue > 62500)
        {
            color = new Vector4(1, 0, 0, 1);
        }
        // orange
        else if (hue >= 5000 & hue < 13000)
        {
            color = new Vector4(1, 0.65f, 0, 1);
        }
        // yellow
        else if (hue >= 13000 & hue < 20000)
        {
            color = new Vector4(1, 1, 0, 1);
        }
        // green
        else if (hue >= 20000 & hue < 31000)
        {
            color = new Vector4(0, 1, 0, 1);
        }
        // white
        else if (hue >= 31000 & hue < 41000)
        {
            color = new Vector4(1, 1, 1, 1);
        }
        // blue
        else if (hue >= 41000 & hue < 49000)
        {
            color = new Vector4(0, 0, 1, 1);
        }
        // indigo
        else if (hue >= 49000 & hue < 53500)
        {
            color = new Vector4(0.3f, 0, 0.5f, 1);
        }
        // hotpink
        else if (hue >= 53500 & hue < 59000)
        {
            color = new Vector4(1, 0.42f, 0.7f, 1);
        }
        // deeppink
        else if (hue >= 59000 & hue < 62500)
        {
            color = new Vector4(1, 0.08f, 0.58f, 1);
        }

        return color;

    }

    public static int GetHueByColor(string color)
    {
        // red
        if (color == "Red")
        {
            hueValue = 0;
        }
        // orange
        if (color == "Orange")
        {
            hueValue = 9000;
        }
        // yellow
        else if (color == "Yellow")
        {
            hueValue = 19000;
        }
        // green
        else if (color == "Green")
        {
            hueValue = 23500;
        }
        // white
        else if (color == "White")
        {
            hueValue = 35000;
        }
        // blue
        else if (color == "Blue")
        {
            hueValue = 46950;
        }
        // indigo
        else if (color == "Purple" || color == "Indigo")
        {
            hueValue = 50500;
        }
        // pink
        else if (color == "Pink" || color == "Violet")
        {
            hueValue = 57100;
        }

        return hueValue;
    }

    public static int GetHueByColorEnum(Colors color)
    {
        // red
        if (color == Colors.Red)
        {
            hueValue = 0;
        }
        // orange
        if (color == Colors.Orange)
        {
            hueValue = 9000;
        }
        // yellow
        else if (color == Colors.Yellow)
        {
            hueValue = 19000;
        }
        // green
        else if (color == Colors.Green)
        {
            hueValue = 23500;
        }
        // blue
        else if (color == Colors.Blue)
        {
            hueValue = 46950;
        }
        // indigo
        else if (color == Colors.Indigo)
        {
            hueValue = 50500;
        }
        // pink
        else if (color == Colors.Violet)
        {
            hueValue = 57100;
        }

        return hueValue;
    }

    public static int GetHueByRGBA(Color color)
    {
        // convert the RGBA into a Vector4 so we can compare properly
        Vector4 colorVector4 = new Vector4(color.r, color.g, color.b, color.a);

        // red
        if (colorVector4.Equals(new Vector4(1, 0, 0, 1)))
        {
            hueValue = 0;
        }
        // orange
        else if (colorVector4.Equals(new Vector4(1, 0.65f, 0, 1)))
        {
            hueValue = 9000;
        }
        // yellow
        else if (colorVector4.Equals(new Vector4(1, 1, 0, 1)))
        {
            hueValue = 19000;
        }
        // green
        else if (colorVector4.Equals(new Vector4(0, 1, 0, 1)))
        {
            hueValue = 23500;
        }
        // blue
        else if (colorVector4.Equals(new Vector4(0, 0, 1, 1)))
        {
            hueValue = 46950;
        }
        // indigo
        else if (colorVector4.Equals(new Vector4(0.3f, 0, 0.5f, 1)))
        {
            hueValue = 50500;
        }
        // violet
        else if (colorVector4.Equals(new Vector4(1, 0.42f, 0.7f, 1)))
        {
            hueValue = 57100;
        }

        return hueValue;
    }
}
