using UnityEngine;
using System.Collections;

public class UtiliyBag
{

    private const int MAX_WIDTH = 1024;
    private const int MAX_HEIGTH = 768;

    public float GetRelativeWidth(int original_w)
    {
        return Screen.width * (((original_w * 100.0f) / MAX_WIDTH) / 100.0f);
    }

    public float GetRelativeHeigth(int original_h)
    {
        return Screen.height * (((original_h * 100.0f) / MAX_HEIGTH) / 100.0f);
    }

}