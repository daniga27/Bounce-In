using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS

    //Sonido
    private AudioSource audiosource;
    [SerializeField] private AudioClip scoredInOwn, scoredInRival; //Asignar desde editor

    //Script Turns Manager
    public TurnsManager turnsManager; //Asignar desde editor

    //Lista de discos (asignar desde editor con el fin de reusar el script para ambas porterías)
    public List<GameObject> ownDisksList;

    
//FLUJO DE UNITY

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }


//FUNCIONES
    
    //Si un disco entra en el Trigger, significa que ha marcado
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Disk"))
        {
            Scored(collision);
        }
    }
    
    //Función que usa la función de marcar dentro del disco y, además, emite un sonido u otro en función de si se ha marcado en propia o no
    private void Scored(Collider2D collision) 
    {
        var diskScript = collision.gameObject.GetComponent<DiskBehaviour>();
        if (ownDisksList.Contains(collision.gameObject)) //NO suma puntos si se marca en propia
        {
            diskScript.SetScored(-1);
            audiosource.PlayOneShot(scoredInOwn);
        }

        else
        {
            diskScript.SetScored(1);
            audiosource.PlayOneShot(scoredInRival);
        }
    }
}
