using Luminosity.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    public bool DebugMenuOpen = false;
    public GameObject CheckpointPrefab;

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
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
        DebugMenuOpen = false;
        SetMenuVisibility();
    }

    public void God()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().God();
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
        DebugMenuOpen = false;
        SetMenuVisibility();
    }

    public void SpawnCheckpoint ()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Instantiate(CheckpointPrefab, playerPos, Quaternion.identity);
        DebugMenuOpen = false;
        SetMenuVisibility();
    }

    public void BackToMainMenu ()
    {
        SceneManager.LoadScene("Splashes");
        DebugMenuOpen = false;
        SetMenuVisibility();
    }

    public void DevMap(string scene)
    {
        SceneManager.LoadScene(scene);
        DebugMenuOpen = false;
        SetMenuVisibility();
    }
}
