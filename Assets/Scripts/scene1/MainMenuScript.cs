using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    public void OnPlayBtnClick()
    {
        Application.LoadLevel(1);
    }

    public void OnExitBtnClick()
    {
        Application.Quit();
    }
}
