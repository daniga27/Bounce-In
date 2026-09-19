using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseOptionsManager : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOJBECTS

    //Valores
    [SerializeField] private KeyCode pauseKeyP1, pauseKeyP2; //Asignar desde editor (la tecla correspondiente a pausa en la máquina arcade)
    private bool isPaused = false;

    //GameObjects
    [SerializeField] private GameObject optionsMenu; //Asignar desde editor
    public GameObject resumeButton; //Asignarlos desde editor

    //Sonido
    public Slider slider; //Barra de volumen //Asignar desde edito
    public AudioSource musicSource; //Objeto que maneja la música del juego
    private AudioSource audioSource;

    //Fade entre escenas
    private GameObject fadeManager;
    private FadeSceneManager fadeSceneManager;


//FLUJO DE UNITY

    private void Awake()
    {
        fadeManager = GameObject.FindGameObjectWithTag("FadeManager");
        fadeSceneManager = fadeManager.GetComponent<FadeSceneManager>();

        optionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resumeButton);

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource.volume = 0.5f;
        slider.value = 0.5f;
        slider.onValueChanged.AddListener(VolumeManagement);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pauseKeyP1) || Input.GetKeyDown(pauseKeyP2))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

//FUNCIONES

    //El juego se pausa dando a la tecla correspondiente (por asignar en editor)
    private void Pause()
    {
        Time.timeScale = 0.0f; //Detener tiempo
        optionsMenu.SetActive(true);
        audioSource.Play();
        isPaused = true;
        musicSource.Pause(); //Pausar música
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    //Salir al menú principal
    public void MainMenu()
    {
        Time.timeScale = 1.0f; //Reactivar tiempo antes de volver al menú o no funcionará
        audioSource.Play();
        fadeSceneManager.FadeAndLoad("MainMenu", 1f);
    }

    //Se sale pulsando la misma tecla o clickando el botón (asignarlo desde el editor)
    public void Resume()
    {
        Time.timeScale = 1.0f; //Reactivar tiempo
        optionsMenu.SetActive(false);
        audioSource.Play();
        isPaused = false;
        musicSource.UnPause(); //Continuar música
    }

    //Salir del juego
    public void Exit()
    {
        audioSource.Play();
        Application.Quit();
    }

    //Manejar el volumen
    private void VolumeManagement(float value)
    {
        musicSource.volume = value;
    }
}
