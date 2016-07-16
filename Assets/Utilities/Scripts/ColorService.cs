using UnityEngine;
using System.Collections;

public class ColorService : MonoBehaviour
{

    Vector4 color;
    int hueValue;

    public Vector4 GetColorByHue(int hue)
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

    public int GetHueByColor(string color)
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
            hueValue = 12750;
        }
        // green
        else if (color == "Green")
        {
            hueValue = 25500;
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
        else if (color == "Purple")
        {
            hueValue = 56100;
        }
        // pink
        else if (color == "Pink")
        {
            hueValue = 56100;
        }

        return hueValue;

    }
}
