using TMPro;
using UnityEngine;
public class Timer : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS

    //Script Turns Manager
    public TurnsManager turnsManager; //Asignar desde editor

    //Valores
    [SerializeField] private int seconds;
    public float countdown;
    private int P1Score, P2Score;

    //Texto
    [SerializeField] private TextMeshProUGUI text; //Asignar desde editor
    [SerializeField] private TextMeshProUGUI P1ScoreText; //Asignar desde editor
    [SerializeField] private TextMeshProUGUI P2ScoreText; //Asignar desde editor


//FLUJO DE UNITY

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        countdown = 21.0f;
        P1Score = 0;
        P2Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Countdown();
        TextManagement();
        UpdateScores();
        ShowWarning();
    }

//FUNCIONES
    
    //Contador regresivo
    private void Countdown()
    {
        countdown -= Time.deltaTime;
        seconds = Mathf.FloorToInt(countdown % 60);
        //Reiniciar contador si el turno comienza
        if (turnsManager.GetTurnActive() == 0)
            countdown = 21.0f;
    }

    //Devolver el tiempo actual (útil para otros scripts)
    public float GetCurrentTime()
    {
        return countdown;
    }

    //Manejo de los textos del contador
    private void TextManagement()
    {
        switch (turnsManager.GetTurnActive())
        {
            //RECORDATORIO: 0 --> comienza      1--> selección de movimientos      2 --> disparo       3 --> discos en movimiento

            case 0:
                text.text = string.Format("{00}", seconds);
                break;
            case 1:
                text.text = string.Format("{00}", seconds);
                break;
            case 2:
                text.text = "Shoot!";
                break;
            case 3:
                text.text = "Shoot!";
                break;
        }
    }

    //Actualizar marcadores de puntuación
    private void UpdateScores()
    {
        P1Score = turnsManager.GetScore("Player 1");
        P2Score = turnsManager.GetScore("Player 2");
        P1ScoreText.text = P1Score + "";
        P2ScoreText.text = P2Score + "";
    }

    //Mostrar aviso (esperamos que no tenga que salir el aviso de inactividad, pero igualmente lo añadimos como función extra
    private void ShowWarning()
    {
        bool showWarning = turnsManager.GetShowWarning();
        if (showWarning)
        {
            text.text = "YOU GOTTA SELECT A MOVEMENT!";
        }
        return;
    }
}
