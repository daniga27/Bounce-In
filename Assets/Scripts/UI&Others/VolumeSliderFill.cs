using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSliderFill : MonoBehaviour, ISelectHandler, IDeselectHandler
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS
    
    public Image fillImage; //Asignar desde editor
    private Color normalColor, selectedColor; 


//FLUJO DE UNITY

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        normalColor = fillImage.color; //Asigna el color inicial que tenga la imagen del relleno
        selectedColor = Color.red;
    }

    
//FUNCIONES (que, al igual que en el script de ButtonsSound, necesitábamos el método OnSelect(), solo que aquí cambia el color de la barra de volumen)

    public void OnSelect(BaseEventData eventData)
    {
        fillImage.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        fillImage.color = normalColor;
    }
}
