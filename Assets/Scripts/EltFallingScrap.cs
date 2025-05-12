using UnityEngine;
using UnityEngine.UI;

public class EltFallingScrap : EltScrap
{
    const float HeightMax = 400f;
    const float HeightMin = 100f;
    const float speed = 20f;

    RectTransform rect;
    bool isFalling = false;


    public void Start()
    {
    }

    void Update()
    {
        if (isFalling)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - speed * Time.deltaTime);
        }

        if (rect.anchoredPosition.y <= 0)
            isFalling = false;
    }

    public override void OnClick()
    {
        base.OnClick();
        DestroyImmediate(gameObject);
    }

    public void OnFalling()
    {
        gameObject.SetActive(true);
        if (rect == null)
            rect = GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(Random.Range(0, 1920), Random.Range(HeightMin, HeightMax));
        isFalling = true;
    }
}
