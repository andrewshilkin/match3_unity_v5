using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PopupScore : FontMap
{
    private static PopupScore _instance = null;

    private GameObject _target = null;

    public PopupScore()
    {

    }
    public static PopupScore getInstance()
    {
        if(_instance == null)
        {
            _instance = new PopupScore();
        }
        return _instance;
    }

    public void initTarget(GameObject gameObject)
    {
        _target = gameObject;
    }

    public void AddPopupScore(Vector3 pos, int score)
    {
        GameObject gameObject = new GameObject("popupScore");
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        gameObject.name = "popupScore";
        gameObject.transform.position = pos;
        float spacing = 0;

        GameObject container = new GameObject();
        container.transform.parent = gameObject.transform;
        container.transform.localScale = new Vector3(1, 1, 1);
        

        foreach(char c in score.ToString())
        {
            GameObject child = new GameObject();
            
            SpriteRenderer renderer = child.AddComponent<SpriteRenderer>();
            renderer.sprite = _fontMap[int.Parse(c.ToString())];

            child.transform.parent = container.transform;
            child.transform.localScale = new Vector3(1, 1);
            child.transform.localPosition = new Vector3(spacing, 0, 0);

            spacing += _fontMap[int.Parse(c.ToString())].bounds.size.x;
        
        }
        container.transform.localPosition = new Vector3(-(spacing / 2 - 0.1f), 0);
        
        Hashtable args = new Hashtable();
        args.Add("x", 4f);
        args.Add("y", 4f);

        iTween.ScaleTo(gameObject, args);

        args = new Hashtable();
        args.Add("x", 0f);
        args.Add("y", 0f);
        args.Add("z", 0f);
        args.Add("delay", 1.0f);

        if (_target != null)
            args.Add("oncompletetarget", _target);

        args.Add("oncomplete", "onScoreScale");
        args.Add("oncompleteparams", gameObject);

        iTween.ScaleTo(gameObject, args);

    }

}

