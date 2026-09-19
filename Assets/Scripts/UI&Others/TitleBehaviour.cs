using UnityEngine;

//Script que he reusado del Plataformas que hice para el Trabajo Individual 

public class TitleBehaviour : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS
    
    private Vector3 initialPosition;
    [SerializeField] private float amplitude; //Valor asignado desde editor
    [SerializeField] private float frequency; //Valor asignado desde editor


//FLUJO DE UNITY

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPosition + amplitude * Mathf.Sin(Time.time * frequency) * Vector3.up;
    }
}

