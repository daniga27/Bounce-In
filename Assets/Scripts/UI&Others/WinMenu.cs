using UnityEngine;

public class WinMenu : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS

    //Fade entre escenas
    private GameObject fadeManager;
    private FadeSceneManager fadeSceneManager;

    //Sonido
    private AudioSource audioSource;

    //Botón
    public GameObject menuButton; //Asignar desde editor


//FLUJO DE UNITY

    private void Awake()
    {
        fadeManager = GameObject.FindGameObjectWithTag("FadeManager");
        fadeSceneManager = fadeManager.GetComponent<FadeSceneManager>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
    }


//FUNCIONES

    //Volver al menú principal
    public void MainMenu()
    {
        audioSource.Play();
        fadeSceneManager.FadeAndLoad("MainMenu", 1f);
    }

    //Salir del juego
    public void Exit()
    {
        audioSource.Play();
        Application.Quit();
    }
}
