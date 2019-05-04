using Luminosity.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public enum DifficultyEnum
{
    [Description("Normal")]
    Normal,
    [Description("Super Baby Easy Mode")]
    SuperBabyEasyMode,
    [Description("H. E. 2x. Hockey Sticks. L. L.")]
    Hell
};
public class DifficultySelector : MonoBehaviour
{
    public DifficultyEnum Difficulty = DifficultyEnum.Normal;
    public Text DifficultyDisplay;


    private void Start()
    {
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown("UI_Right"))
        {
            Difficulty = Difficulty.Next();
            UpdateDisplay();
        }            
        else if (InputManager.GetButtonDown("UI_Left"))
        {
            Difficulty = Difficulty.Previous();
            UpdateDisplay();
        }
    }

    public void NextDifficulty ()
    {
        Difficulty = Difficulty.Next();
        UpdateDisplay();
    }

    private void UpdateDisplay ()
    {
        DifficultyDisplay.text = "◀    " + Difficulty.ToDescription() + "    ▶";
    }
}
