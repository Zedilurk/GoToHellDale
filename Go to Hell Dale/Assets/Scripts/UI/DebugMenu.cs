using Luminosity.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public bool DebugMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SetMenuVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetKeyDown(KeyCode.F4) || (InputManager.GetButtonDown("UI_Start") && InputManager.GetButtonDown("UI_Back")))
        {
            DebugMenuOpen = !DebugMenuOpen;
            SetMenuVisibility();
        }
            
    }

    public void SetMenuVisibility ()
    {
        if (DebugMenuOpen)
            this.GetComponent<Canvas>().enabled = true;
        else
            this.GetComponent<Canvas>().enabled = false;
    }

    public void NoClip ()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().NoClip();
    }

    public void God()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().God();
    }
}
