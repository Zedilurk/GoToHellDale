using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesUIManager : MonoBehaviour {

    public TextMeshProUGUI Counter;

    public void SetLivesCount (int count)
    {
        Counter.text = "x" + count.ToString();
    }
}
