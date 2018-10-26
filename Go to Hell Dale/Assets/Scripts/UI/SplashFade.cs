using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashFade : MonoBehaviour {

    Image _Image;
    public AudioClip AudioClip;
    public float AudioDelay = 2f;
    public string LevelToLoadAfter = "";

	// Use this for initialization
	void Start () {
        _Image = GetComponent<Image>();
        StartCoroutine(FadeImage(false, 1f));
        StartCoroutine(PlayAudioIntro(AudioDelay));
        StartCoroutine(FadeImage(true, 4f));
        StartCoroutine(LoadLevelAfterDelay(LevelToLoadAfter, 7f));
    }
	

    IEnumerator FadeImage(bool fadeAway, float pause)
    {
        yield return new WaitForSeconds(pause);

        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                _Image.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                _Image.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

    IEnumerator PlayAudioIntro (float pause)
    {
        yield return new WaitForSeconds(pause);
        Camera.main.GetComponent<AudioSource>().PlayOneShot(AudioClip);
    }

    IEnumerator LoadLevelAfterDelay (string level, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(level);
    }
}
