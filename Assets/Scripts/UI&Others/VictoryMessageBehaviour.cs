using System.Collections;
using TMPro;
using UnityEngine;

public class VictoryMessageBehaviour : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS
    
    private TextMeshProUGUI text;
    

//FLUJO DE UNITY

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FlashingText());
    }


//CORRUTINAS

    //Parpadeo del texto
    private IEnumerator FlashingText()
    {
        //Bucle infinito
        while (true)
        {
            text.enabled = true;
            yield return new WaitForSeconds(0.5f);
            text.enabled = false;
            yield return new WaitForSeconds(0.15f);
        }
    }

}
