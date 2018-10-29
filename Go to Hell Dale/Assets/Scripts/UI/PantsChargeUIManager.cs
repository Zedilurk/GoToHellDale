using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantsChargeUIManager : MonoBehaviour
{

    public List<GameObject> PantsChargeIcons = new List<GameObject>();
    public Sprite ChargedPantsIcon;
    public Sprite UsedPantsIcon;

    public Transform ReloadStartPosition;
    public Transform ReloadEndPosition;
    public Transform ReloadCursor;
    public Transform ReloadActiveReloadHandle;

    public Color SuccessColor = Color.green;
    public Color FailColor = Color.red;
    public Color DefaultColor = Color.white;

    bool _IsReloading = false;
    float _ReloadTime = 0f;
    float _ReloadStartTime;

    public GameObject reloadBar;

    // Use this for initialization
    void Start()
    {
        PantsChargeIcons = GetPantsIcons();
        reloadBar.SetActive(false);
        ChargesUpdated(3);
    }

    public void Update()
    {
        if (_IsReloading)
        {
            float timeSinceReload = Time.time - _ReloadStartTime;
            float percent = timeSinceReload / _ReloadTime;

            // Calculate the distance
            float distance = Vector3.Distance(ReloadStartPosition.position, ReloadEndPosition.position);
            distance = distance * percent;

            // Calculate the difference vector
            Vector3 difference = ReloadEndPosition.position - ReloadStartPosition.position;

            // Normalize and scale the difference vector
            difference = difference.normalized * distance;

            // Translate the vector back to A
            ReloadCursor.position = (ReloadStartPosition.position + difference);
        }
    }

    private List<GameObject> GetPantsIcons()
    {
        List<GameObject> icons = new List<GameObject>();
        foreach (Transform child in transform)
        {
            icons.Add(child.gameObject);
        }

        icons.Sort((p1, p2) => p1.name.CompareTo(p2.name));
        return icons;
    }

    public void ChargesUpdated(int currentCharges)
    {
        //turn off every pants icon with a higher value than current charges
        foreach (GameObject PantsIcon in PantsChargeIcons)
        {
            int num = -1;
            int.TryParse(PantsIcon.name, out num);

            if (UsedPantsIcon != null)
            { 
                if (num > currentCharges)
                    PantsIcon.GetComponent<Image>().sprite = UsedPantsIcon;
                else
                    PantsIcon.GetComponent<Image>().sprite = ChargedPantsIcon;
            }
            else
            {
                if (num > currentCharges)
                    PantsIcon.SetActive(false);
                else
                    PantsIcon.SetActive(true);
            }
        }
    }

    public void StartReload(float reloadTime, float activeReloadStartPercent, float activeReloadEndPercent)
    {
        if (FadeCoroutine != null)
        {
            StopCoroutine(FadeCoroutine);
        }

        ReloadCursor.GetComponent<Image>().CrossFadeColor(DefaultColor, 0, true, false);

        reloadBar.SetActive(true);

        _ReloadStartTime = Time.time;
        _ReloadTime = reloadTime;
        AdjustActiveReloadMeter(activeReloadStartPercent, activeReloadEndPercent);
        _IsReloading = true;
    }

    private void AdjustActiveReloadMeter(float activeReloadStartPercent, float activeReloadEndPercent)
    {
        RectTransform activeHandle = ReloadActiveReloadHandle.GetComponent<RectTransform>();
        RectTransform startHandle = ReloadStartPosition.GetComponent<RectTransform>();
        RectTransform endHandle = ReloadEndPosition.GetComponent<RectTransform>();
        float distance = Vector2.Distance(startHandle.anchoredPosition, endHandle.anchoredPosition);

        float StartDistance = distance * (activeReloadStartPercent / 100);
        float EndDistance = distance * (activeReloadEndPercent / 100);

        Vector2 Difference = endHandle.anchoredPosition - startHandle.anchoredPosition;
        Vector2 StartDifference = Difference.normalized * StartDistance;
        Vector2 EndDifference = Difference.normalized * EndDistance;

        //float xPos1 = (startHandle.anchoredPosition.x + StartDifference.x);
        //float xPos2 = (startHandle.anchoredPosition.x + EndDifference.x);

        //float width = xPos2 - xPos1;
        float width = EndDifference.x - StartDifference.x;

        activeHandle.sizeDelta = new Vector2(width, activeHandle.rect.height);
        activeHandle.anchoredPosition = new Vector2(StartDifference.x, activeHandle.anchoredPosition.y);
    }

    public void StopReload(bool success)
    {
        if (success)
        {
            ReloadCursor.GetComponent<Image>().CrossFadeColor(SuccessColor, 0.5f, true, false);
        }
        else
        {
            ReloadCursor.GetComponent<Image>().CrossFadeColor(FailColor, 0.5f, true, false);
        }

        _IsReloading = false;
        FadeCoroutine = StartCoroutine(FadeReloadBar(2));
    }

    Coroutine FadeCoroutine;
    IEnumerator FadeReloadBar(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        reloadBar.SetActive(false);
        StopCoroutine(FadeCoroutine);
    }
}
