using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurnsManager : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS

    //Fade entre escenas
    private GameObject fadeManager;
    private FadeSceneManager fadeSceneManager;
    
    //Script Timer (contador)
    public Timer timer; //Asignar desde editor

    //Valores
    private int turnState = 0; // 0 --> turno comienza     1 --> turno dura    2 --> discos se disparan     3 --> discos en movimiento  
    private bool fixFrameErrorInput = false, isShowingWarning = false, gameEnded = false;

    //Listas de Discos
    public List<GameObject> P1Disks; //Asignar desde editor
    public List<GameObject> P2Disks; //Asignar desde editor

    //Marcadores
    public int P1Score = 0;
    public int P2Score = 0;

    //Strings para actualizar los marcadores (en la UI)
    private string P1 = "Player 1";
    private string P2 = "Player 2";

    //Sonido
    private AudioSource audioSource; //Emite un sonido cuando los discos se disparan


//FLUJO DE UNITY

    private void Awake()
    {
        fadeManager = GameObject.FindGameObjectWithTag("FadeManager");
        fadeSceneManager = fadeManager.GetComponent<FadeSceneManager>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        turnState = 0;
        audioSource = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        TurnState();
        CheckWarning(P1Disks, P2Disks);
    }


//FUNCIONES

    //Máquina de Estados de los Turnos
    private void TurnState()
    {
        switch (turnState)
        {
            //TURNO COMIENZA
            case 0:
                Debug.Log("ESTADO 0");
                ResetDisksValues(P1Disks);
                ResetDisksValues(P2Disks);
                turnState++;
                break;

            //TURNO DURA (jugadores seleccionan)
            case 1:
                DisksMovements(P1Disks);
                DisksMovements(P2Disks);

                if (timer.GetCurrentTime() < 0 || (DisksAreReady(P1Disks) && DisksAreReady(P2Disks)))
                    turnState++;
                break;

            //DISPARO (los discos son disparados)
            case 2:
                Debug.Log("ESTADO 2");
                audioSource.Play();
                ShootDisks(P1Disks);
                ShootDisks(P2Disks);
                fixFrameErrorInput = true;
                turnState++;
                break;
            
            //DISCOS EN MOVIMIENTO
            case 3:
                Debug.Log("ESTADO 3");
                if (fixFrameErrorInput)
                {
                    fixFrameErrorInput = false; //Lo mismo que en DiskBehaviour, pasaba al case 3 en el mismo frame (justo antes de que se disparasen)
                    return;
                }

                //Si disco marca, se destruye y actualiza la lsita
                DisksScored(P1Disks);
                DisksScored(P2Disks);

                //Finaliza el turno cuando todos los discos se hayan quedado quietos
                if (DisksAreStill(P1Disks) && DisksAreStill(P2Disks))
                {
                    turnState = 0;

                }
                CheckIfWin();
                break;
        }
    }

    //Devuelve el estado actual de la máquina de estados de Turnos, útil para otros Scripts
    public int GetTurnActive()
    {
        return turnState;
    }

    //Movimientos e inputs de los discos
    private void DisksMovements(List<GameObject> disks)
    {
        foreach(var disk in disks)
        {
            var diskScript = disk.GetComponent<DiskBehaviour>();
            
            //Dirección del movimiento
            if(!diskScript.directionSelected)
            {
                if(diskScript.HasPressedInput())
                {
                    diskScript.SelectDirection();
                }
            }
            //Selección de fuerza
            else
            {
                if(!diskScript.strengthSelected)
                {
                    diskScript.ShowStrengthBar();

                    if(diskScript.HasPressedInput())
                    {
                        diskScript.SelectStrength();
                    }
                }
            }
        }
    }

    //Dispara los discos
    private void ShootDisks(List<GameObject> disks)
    {
        foreach (var disk in disks)
        {
            var diskScript = disk.GetComponent<DiskBehaviour>();
            if (!diskScript.hasShot)
            {
                diskScript.Shoot();
            }
        }
    }

    //Comprueba si todos los discos están quietos (para dar por acabado el turno)
    private bool DisksAreStill(List<GameObject> disks)
    {
        if(disks.Count == 0)
            return false;
        
        foreach (var disk in disks)
        {
            var diskScript = disk.GetComponent<DiskBehaviour>();
            if (!diskScript.isStill)
                return false;
        }
        return true;
    }

    //Comprueba si los discos están listos para disparar los discos (en caso de que seleccionen todos)
    private bool DisksAreReady(List<GameObject> disks)
    {
        int counter = 0;
        foreach (var disk in disks)
        {
            var diskScript = disk.GetComponent<DiskBehaviour>();
            if (diskScript.isReady)
                counter++;
        }

        if (counter == disks.Count)
            return true;

        return false;
    }

    //Resetea los valores de los discos
    private void ResetDisksValues(List<GameObject> disks)
    {
        foreach (var disk in disks)
        {
            var diskScript = disk.GetComponent<DiskBehaviour>();
            diskScript.ResetValues();
        }
    }

    //Cuando un disco marca, se aumenta el contador del jugador y se destruye el disco
    private void DisksScored(List<GameObject> disks)
    {
        GameObject diskToDestroy = null;
        foreach(var disk in disks)
        {
            var diskScript = disk.GetComponent<DiskBehaviour>();
            if(diskScript.scored)
            {
                //Aumentar puntuación
                if(disks == P1Disks)
                    P1Score += diskScript.updateScore;
                else
                    P2Score += diskScript.updateScore;

                diskToDestroy = disk;
            }
        }
        //Actualizar lista y destruir disco
        if(diskToDestroy != null)
        {
            disks.Remove(diskToDestroy);
            Destroy(diskToDestroy);
        }
    }

    //Devuelve la puntuación de cada jugador (para actualizar el marcador en el script Timer)
    public int GetScore(string player)
    {
        if (player == P1)
            return P1Score;
        if (player == P2)
            return P2Score;
        return 0;
    }

    //Comprobar si NO han seleccionado movimiento para mostrar mensaje de aviso
    private void CheckWarning(List<GameObject> disksP1, List<GameObject> disksP2)
    {
        if (isShowingWarning) return; //Si ya lo muestra, no lo muestra de nuevo
        if (turnState == 0) return; //Si el turno comienza, no se muestra el aviso (dado que al comenzar ninguno ha seleccionado todavía)
        if (timer.GetCurrentTime() > 0) return; //Si el contador no llega a 0, todavía no se muestra

        //Si algún disco ha seleccionado, entonces el mensaje de aviso NO salta
        foreach (var disk in disksP1)
        {
            if (disk.GetComponent<DiskBehaviour>().directionSelected)
                return;
        }

        foreach (var disk in disksP2)
        {
            if (disk.GetComponent<DiskBehaviour>().directionSelected)
                return;
        }

        StartCoroutine(ShowWarningAndForceStart());
    }

    //Devuelve si está mostrando o no el aviso
    public bool GetShowWarning()
    {
        return isShowingWarning;
    }

    //Comprobar si algún jugador ha ganado
    private void CheckIfWin()
    {
        if (gameEnded)
            return;
        
        if (P1Disks.Count == 0 || P2Disks.Count == 0)
        {
            gameEnded = true;
            if (P1Score > P2Score)
            {
                //JUGADOR 1 GANA
                fadeSceneManager.FadeAndLoad("Player1Wins", 1f);
            }

            if (P2Score > P1Score)
            {
                //JUGADOR 2 GANA
                fadeSceneManager.FadeAndLoad("Player2Wins", 1f);
            }

            if(P1Score == P2Score)
            {
                //NADIE GANA
                fadeSceneManager.FadeAndLoad("NobodyWins", 1f);
            }
        }
    }


//CORRUTINAS
    
    //Corrutina para mostrar aviso por inactividad
    private IEnumerator ShowWarningAndForceStart()
    {
        isShowingWarning = true;
        yield return new WaitForSeconds(2f);
        turnState = 0;
        isShowingWarning = false;
        timer.countdown = 21f;
    }
}
