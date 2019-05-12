using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using UnityEngine.UI;
using Luminosity.IO;

public class DialogueManager : MonoBehaviour
{
    public GameObject container_NPC;
    public GameObject container_PLAYER;
    public GameObject dialogue_Background;
    public Image image_NPC;
    public Text text_NPC;
    public Text[] text_Choices;

    // Start is called before the first frame update
    void Start()
    {
        container_NPC.SetActive(false);
        container_PLAYER.SetActive(false);
        dialogue_Background.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!VD.isActive)
                BeginDialogue();
            else
                VD.Next();
        }        
    }

    void BeginDialogue()
    {
        VD.OnNodeChange += OnNodeChange;
        VD.OnEnd += OnEnd;
        VD.BeginDialogue(GetComponent<VIDE_Assign>());
        dialogue_Background.SetActive(true);
    }

    private void OnNodeChange(VD.NodeData data)
    {
        container_NPC.SetActive(false);
        container_PLAYER.SetActive(false);

        if (data.isPlayer)
        {
            container_PLAYER.SetActive(true);

            for (int i = 0; i < text_Choices.Length; i++)
            {
                if (i < data.comments.Length)
                {
                    text_Choices[i].transform.parent.gameObject.SetActive(true);
                    text_Choices[i].text = data.comments[i];
                }
                else
                    text_Choices[i].transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            container_NPC.SetActive(true);
            text_NPC.text = data.comments[data.commentIndex];
            image_NPC.sprite = data.sprite;
        }
    }

    private void OnEnd(VD.NodeData data)
    {
        container_NPC.SetActive(false);
        container_PLAYER.SetActive(false);
        dialogue_Background.SetActive(false);
        VD.OnNodeChange -= OnNodeChange;
        VD.OnEnd -= OnEnd;
        VD.EndDialogue();
    }

    void OnDisable()
    {
        if (container_NPC != null)
            OnEnd(null);
    }

    public void SetPlayerChoice(int choice)
    {
        VD.nodeData.commentIndex = choice;
        if (InputManager.GetButtonUp("UI_Submit"))
            VD.Next();
    }
}
