using UnityEngine;

public class DiskBehaviour : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS

    //Script Turns Manager
    public TurnsManager turnsManager; //Asignar desde editor

    //Comprobaciones y más (booleanos)
    public bool directionSelected = false, strengthSelected = false, isStill, isReady = false, scored = false, hasShot = false;

    //Valores 
    [SerializeField] private float rotationSpeed, standardStrength = 100f, finalStrength = 0f, multiplier = 0f, maxSpeed;
    public int updateScore = 0;

    //Relativo al propio disco
    private Rigidbody2D rb;

    //Relativo al centro de rotación
    public Transform rotationCenter; //Asignar desde editor

    //Relativo a la flecha de dirección
    public Transform directionArrow; //Asignar desde editor
    private SpriteRenderer directionArrowSprite;
    private Vector2 directionArrowInitialPosition;

    //Relativo a la barra de fuerza (y la flecha indicadora)
    public GameObject strengthBar; //Asignar desde editor (cada disco tendrá una barra de fuerza propia)
    public GameObject selectorBar; //Asignar desde editor
    public BarBehaviour strengthBarScript;//Asignar desde editor

    //Sonido
    private AudioSource audioSource;
    [SerializeField] private AudioClip normalHit; //Asignar desde editor
    [SerializeField] AudioClip lowStrength, normalStrength, highStrength; //Asignar desde editor

    //Inputs
    public KeyCode input; //Asignar desde editor (Pues cada jugador tendrá 1 distinto)


//FLUJO DE UNITY

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Disco
        rb = GetComponent<Rigidbody2D>();
        
        //Flecha de Dirección
        directionArrowSprite = directionArrow.gameObject.GetComponent<SpriteRenderer>();
        directionArrowInitialPosition = directionArrow.localPosition;
        
        //Barra de Fuerza (y su flecha indicadora)
        strengthBar.SetActive(false);

        //Sonido
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Velocidad X: " + rb.linearVelocityX + ", Velocidad Y: " + rb.linearVelocityY);
        
        CheckIfStill();
        CheckIfReady();

        RotationCenterManagement();
        DirectionArrowManagement();
    }

    //Para evitar que los discos salgan disparados, limitaremos la velocidad (y, como es física, irá en el FixedUpdate)
    private void FixedUpdate()
    {
        if(rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

//FUNCIONES

    //Comprueba si el disco está quieto o no
    private void CheckIfStill()
    {
        if (rb.linearVelocity.magnitude < 0.01f) //Comprueba si se está moviendo o no
            isStill = true;
        else
            isStill = false;
    }

    //Comprueba si el disco está listo (es decir, ha seleccionado dirección y fuerza)
    private void CheckIfReady()
    {
        if (strengthSelected) //Como no se puede seleccionar fuerza sin seleccionar dirección, con comprobar esta ya es suficiente
            isReady = true;
        else
            isReady = false;
    }

    //Input general (para ahorrar funciones)
    public bool HasPressedInput()
    {
        return Input.GetKeyDown(input);
    }


    //Selección de Dirección
    public void SelectDirection()
    {
        directionSelected = true;
        audioSource.Play();
    }

    //Selección de Fuerza
    public void SelectStrength() 
    {
        strengthSelected = true;

        strengthBarScript.StopBar();
        multiplier = strengthBarScript.GetStrengthMultiplier();

        //Sonidos en función de la fuerza elegida
        if(multiplier > 1 && multiplier < 2)
        {
            audioSource.PlayOneShot(lowStrength);
        }
        else
        {
            if(multiplier > 2f && multiplier < 3.6f)
            {
                audioSource.PlayOneShot(normalStrength);
            }
            else
            {
                audioSource.PlayOneShot(highStrength);
            }
        }

        finalStrength = standardStrength * multiplier; //Guardar la fuerza final con el multiplicador escogido
        finalStrength *= 0.040f; //Reducimos la fuerza final para evitar que los discos salgan disparados con una velocidad realmente EXCESIVA
        strengthBar.SetActive(false);
    }

    //Activa la barra de fuerza
    public void ShowStrengthBar()
    {
        strengthBar.SetActive(true);
    }

    //Dispara el disco cuando es debido
    public void Shoot()
    {
        //Condición para que no dispare múltiples veces
        if (hasShot)
            return;
        
        hasShot = true;
        rb.linearVelocity = finalStrength * directionArrow.up.normalized; 

        //Si ha seleccionado dirección pero no le ha dado tiempo a seleccionar la fuerza, se aplica la fuerza estándar
        if (directionSelected && !strengthSelected)
            rb.linearVelocity = standardStrength * 0.040f * directionArrow.up.normalized;

        strengthBar.SetActive(false);
    }

    //El disco ha marcado (se le pasa como argumento los puntos que sumar o restar; se pasan desde el script de las porterías ("GoalBehaviour")
    public void SetScored(int points)
    {
        scored = true;
        updateScore = points; //Puntos a sumar en el marcador
    }

    //Reseteo de valores cuando el turno comienza
    public void ResetValues()
    {
        directionSelected = false;
        strengthSelected = false;
        isReady = false;
        hasShot = false;
        scored = false;
        finalStrength = 0f;
        strengthBarScript.speed = 3.0f;
        strengthBar.SetActive (false);
    }

    //Manejo del centro de rotación, children del disco
    private void RotationCenterManagement()
    {
        //RECORDATORIO: 0 --> comienza      1--> selección de movimientos      2 --> disparo       3 --> discos en movimiento
        int turnState = turnsManager.GetTurnActive();

        if(turnState == 0 || turnState == 3 || turnsManager.GetShowWarning())
        {
            rotationCenter.rotation = Quaternion.identity; 
        }
        else
        { 
            if(!directionSelected)
            {
                rotationCenter.Rotate(rotationSpeed * Time.deltaTime * Vector3.back);
            }
        }
    }

    //Manejo de la flecha de dirección, children del centro de rotación
    private void DirectionArrowManagement()
    {
        //RECORDATORIO: 0 --> comienza      1--> selección de movimientos      2 --> disparo       3 --> discos en movimiento
        int turnState = turnsManager.GetTurnActive();

        if(turnsManager.GetShowWarning())
            directionArrowSprite.enabled = false;
        else
        {
            switch (turnState)
            {
                case 0:
                    directionArrow.localPosition = directionArrowInitialPosition;
                    directionArrowSprite.enabled = true;
                    break;

                case 1:
                    directionArrowSprite.enabled = true;
                    break;

                case 2:
                    break;

                case 3:
                    directionArrowSprite.enabled = false;
                    break;
            }
        }       
    }

    //Emitir un sonido de choque con cualquier elemento
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.PlayOneShot(normalHit);
    }
}
