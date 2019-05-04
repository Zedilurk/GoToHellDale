using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using Luminosity.IO;

public class UIButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int StandardPosition = 300;
    public int SelectedPosition = 325;
    public bool IsFirstItem = false;
    public bool IsDifficultySelector;

    public AudioClip SelectedClip;
    private AudioSource SelectedSource;

    void Start ()
    {
        if (SelectedClip != null)
            SelectedSource = this.gameObject.AddComponent<AudioSource>();
    }

    void Update ()
    {
        if (EventSystem.current.currentSelectedGameObject == null && 
            (InputManager.GetButtonDown("UI_Down") || InputManager.GetButtonDown("UI_Up") || InputManager.GetButtonDown("UI_Left") || InputManager.GetButtonDown("UI_Right")))
            if (IsFirstItem)
                this.GetComponent<Button>().Select();
    }

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(SelectedPosition, 50);

        if (SelectedClip != null && SelectedSource != null)
            SelectedSource.PlayOneShot(SelectedClip);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(StandardPosition, 50);
    }
}