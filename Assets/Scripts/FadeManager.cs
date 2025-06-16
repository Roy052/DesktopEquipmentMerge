using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static IEnumerator FadeIn(SpriteRenderer spriteRenderer, float time)
    {
        float timeCheck = 0;
        Color tempColor;
        tempColor = spriteRenderer.color;
        tempColor.a = 0;
        spriteRenderer.color = tempColor;
        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            tempColor.a += Time.deltaTime / time;
            spriteRenderer.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator FadeOut(SpriteRenderer spriteRenderer, float time)
    {
        float timeCheck = 0;
        Color tempColor;
        tempColor = spriteRenderer.color;
        tempColor.a = 1;
        spriteRenderer.color = tempColor;
        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            tempColor.a -= Time.deltaTime / time;
            spriteRenderer.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator FadeIn(Image image, float time)
    {
        image.SetActive(true);
        if (image.gameObject.activeSelf == false)
            image.gameObject.SetActive(true);
        float timeCheck = 0;
        Color tempColor;
        tempColor = image.color;
        tempColor.a = 0;
        image.color = tempColor;
        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            tempColor.a += Time.deltaTime / time;
            image.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator FadeOut(Image image, float time)
    {
        float timeCheck = 0;
        Color tempColor;
        tempColor = image.color;
        tempColor.a = 1;
        image.color = tempColor;
        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            tempColor.a -= Time.deltaTime / time;
            image.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
        image.SetActive(false);
    }

    public static IEnumerator FadeIn(RawImage image, float time)
    {
        image.SetActive(true);
        float timeCheck = 0;
        Color tempColor;
        tempColor = image.color;
        tempColor.a = 0;
        image.color = tempColor;
        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            tempColor.a += Time.deltaTime / time;
            image.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator FadeOut(RawImage image, float time)
    {
        float timeCheck = 0;
        Color tempColor;
        tempColor = image.color;
        tempColor.a = 1;
        image.color = tempColor;
        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            tempColor.a -= Time.deltaTime / time;
            image.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
        image.SetActive(false);
    }

    public static IEnumerator FadeIn(Text tmpText, float time)
    {
        tmpText.SetActive(true);
        float timeCheck = 0;
        Color tempColor;
        tempColor = tmpText.color;
        tempColor.a = 0;
        tmpText.color = tempColor;
        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            tempColor.a += Time.deltaTime / time;
            tmpText.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator FadeOut(Text tmpText, float time)
    {
        float timeCheck = 0;
        Color tempColor;
        tempColor = tmpText.color;
        tempColor.a = 1;
        tmpText.color = tempColor;
        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            tempColor.a -= Time.deltaTime / time;
            tmpText.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
        tmpText.SetActive(false);
    }

    public static IEnumerator ZoomInOut(Transform tr, float time)
    {
        float timeCheck = 0;
        Vector3 zoomScale = tr.localScale;

        tr.SetActive(true);
        while (timeCheck < time / 2)
        {
            timeCheck += Time.deltaTime;
            zoomScale.x += 0.3f * Time.deltaTime;
            zoomScale.y += 0.3f * Time.deltaTime;
            tr.localScale = zoomScale;
            yield return new WaitForEndOfFrame();
        }

        while (timeCheck < time)
        {
            timeCheck += Time.deltaTime;
            zoomScale.x -= 0.3f * Time.deltaTime;
            zoomScale.y -= 0.3f * Time.deltaTime;
            tr.localScale = zoomScale;
            yield return new WaitForEndOfFrame();
        }

        tr.localScale = new Vector3(1, 1, 1);
        tr.SetActive(false);
    }

    public static IEnumerator MoveRectTransform(RectTransform rt, Vector2 posA, Vector2 posB, float time)
    {
        float currentTime = 0;
        rt.anchoredPosition = posA;
        while(currentTime < time)
        {
            rt.anchoredPosition = Vector2.Lerp(posA, posB, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield return Utilities.WaitForHalfSecond;
    }
}
