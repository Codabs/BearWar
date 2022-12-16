using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMOD;

public class Button_Script : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] private Button button;
    private ColorBlock color1;
    [SerializeField] private ColorBlock color2;
    private void Awake()
    {
        color1 = button.colors;
        button.colors = color1;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/MouseSound/MouseOver");

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }
    public void ChangeColor()
    {
        if(button != null)
        {
            if(button.colors == color1)
            {
                button.colors = color2;
            }
            else
            {
                button.colors = color1;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/MouseSound/MouseClick");
    }
}
