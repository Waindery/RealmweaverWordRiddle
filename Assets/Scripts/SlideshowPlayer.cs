using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteSlideshow : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public float duration = 1f;

    void Start()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        foreach (var sprite in sprites)
        {
            image.sprite = sprite;
            yield return new WaitForSeconds(duration);
        }
    }
}
