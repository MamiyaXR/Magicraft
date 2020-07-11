using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ImageSlider : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites;
    public void Set(int value)
    {
        if (value < 0)
            value = 0;
        else if (value >= sprites.Count)
            value = sprites.Count;
        image.sprite = sprites[value];
    }
}
