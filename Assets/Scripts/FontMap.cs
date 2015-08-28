using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


abstract public class FontMap
{
    protected Dictionary<int,Sprite> _fontMap = new Dictionary<int,Sprite>();

    public FontMap()
    {
        initMap();
    }

    protected void initMap()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("fontMap");

        foreach (Sprite sprite in sprites)
        {
            int index = int.Parse(sprite.name.Split('_')[1]);
            _fontMap.Add(index, sprite);
        }
    }
}

