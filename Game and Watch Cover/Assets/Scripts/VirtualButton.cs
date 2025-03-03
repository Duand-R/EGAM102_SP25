using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualButton : MonoBehaviour
{
    [Header("component reference")]
    public Animator animator;
    public Image buttonImage;

    [Header("visual settings")]
    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;

    private void Start()
    {
        if (buttonImage == null)
        {
            buttonImage = GetComponent<Image>();
        }

        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }

    public void Press()
    {
        if (animator != null)
        {
            animator.SetBool("IsPressed", true);
        }

        if (buttonImage != null)
        {
            buttonImage.color = pressedColor;
        }
    }

    public void Release()
    {
        if (animator != null)
        {
            animator.SetBool("IsPressed", false);
        }

        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }
}
