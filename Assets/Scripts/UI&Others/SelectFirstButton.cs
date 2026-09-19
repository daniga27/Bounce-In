using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectFirstButton : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS

    public Button firstButton;
    
//FLUJO DE UNITY

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }
}
