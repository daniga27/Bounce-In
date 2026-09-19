using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonsMain : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS

    //Fade entre escenas
    private GameObject fadeManager;
    private FadeSceneManager fadeSceneManager;

    //Sonido
    public Slider slider; //Barra de volumen //Asignar desde editor
    public AudioSource musicSource; 
    public AudioSource audioSource;
    [SerializeField] private AudioClip selectedUI; //El audio a reproducir al seleccionar el botón Start

    //Botones
    public GameObject optionsMenu, creditsMenu, exit, title, mainMenu, backButtonInOptions, backButtonInCredits; //Asignar desde editor
    public Button firstButton;


//FLUJO DE UNITY

    public void Awake()
    {
        fadeManager = GameObject.FindGameObjectWithTag("FadeManager");
        fadeSceneManager = fadeManager.GetComponent<FadeSceneManager>();    

        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);

        //Desactivar
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);

        //Activar
        mainMenu.SetActive(true);
        title.SetActive(true);
    }

    private void Start()
    {
        musicSource.volume = 0.5f;
        slider.value = 0.5f;
        slider.onValueChanged.AddListener(VolumeManagement);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null && optionsMenu.activeSelf == false)
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }


//FUNCIONES

    //Abrir el juego (pulsar START)
    public void OpenGame()
    {
        audioSource.Play();

        //Cambio de escena
        fadeSceneManager.FadeAndLoad("Game", 1f);
    }

    //Abrir el menú principal
    public void OpenMainMenu()
    {
        //Deseleccionar botón
        EventSystem.current.SetSelectedGameObject(null);

        //Desactivar
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);

        //Activar
        title.SetActive(true);
        mainMenu.SetActive(true);

        //Seleccionar botón inicial del respectivo menú
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);

        audioSource.Play();
    }

    //Abrir el menú de opciones
    public void OpenOptionsMenu()
    {
        //Deseleccionar botón
        EventSystem.current.SetSelectedGameObject(null);

        //Desactivar
        title.SetActive(false);
        mainMenu.SetActive(false);

        //Activar
        optionsMenu.SetActive(true);

        //Seleccionar botón inicial del respectivo menú
        EventSystem.current.SetSelectedGameObject(backButtonInOptions);

        audioSource.Play();
    }

    //Abrir el menú de créditos
    public void OpenCreditsMenu()
    {
        //Deseleccionar botón
        EventSystem.current.SetSelectedGameObject(null);

        //Desactivar
        title.SetActive(false);
        mainMenu.SetActive(false);

        //Activar
        creditsMenu.SetActive(true);

        //Seleccionar botón inicial del respectivo menú
        EventSystem.current.SetSelectedGameObject (backButtonInCredits); 

        audioSource.Play();
    }

    //Salir del juego
    public void ExitGame()
    {
        Application.Quit();
    }

    //Manejar el volumen (desde el menú de opciones)
    private void VolumeManagement(float value)
    { 
        musicSource.volume = value;
    }
}
