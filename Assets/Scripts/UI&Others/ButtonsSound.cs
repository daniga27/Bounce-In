using UnityEngine;
using UnityEngine.EventSystems;

//Script sacado de un tutorial para emitir un sonido cada vez que se selecciona un botón (OnSelect(), que Unity en el editor solo tiene OnClick())

public class ButtonsSound : MonoBehaviour, ISelectHandler
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS
    
    //Sonido
    private AudioSource audioSource;
    [SerializeField] private AudioClip selectSound;


//FLUJO DE UNITY

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


//FUNCIONES

    //Emitir sonido al seleccionar un botón
    public void OnSelect(BaseEventData eventData)
    {     
        audioSource.PlayOneShot(selectSound);
    }
}
