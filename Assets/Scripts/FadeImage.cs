using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeImage : MonoBehaviour
{

    [SerializeField] GameObject blackOutSquare;
    float duration = 0.8f;
    float timeOnScreen = 1f;

    private void Start()
    {
        StartCoroutine(FadeBlackOutSquare(false, duration));
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack, float fadeSpeed)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if(fadeToBlack)
        {
            while(blackOutSquare.GetComponent<Image>().color.a < 1f)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            SceneManager.LoadScene((int)SceneHandler.LEVELS.mainMenu);
        }
        else
        {
            while(blackOutSquare.GetComponent<Image>().color.a > 0f)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            yield return new WaitForSeconds(timeOnScreen);
            StartCoroutine(FadeBlackOutSquare(true, duration));
            
        }

    }

}
