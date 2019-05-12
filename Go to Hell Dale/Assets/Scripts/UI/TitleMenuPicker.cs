using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuPicker : MonoBehaviour
{
    public GameObject FirstTimeMenu;
    public GameObject StandardMenu;

    // Start is called before the first frame update
    void Start()
    {
        string firstTime = PlayerPrefs.GetString("FirstTimeUser", "true");
        if (string.IsNullOrEmpty(firstTime) || firstTime == "true")
        {
            FirstTimeMenu.SetActive(true);
            StandardMenu.SetActive(false);
        }
        else if (!string.IsNullOrEmpty(firstTime) && firstTime == "false")
        {
            FirstTimeMenu.SetActive(false);
            StandardMenu.SetActive(true);
        }
    }
}
