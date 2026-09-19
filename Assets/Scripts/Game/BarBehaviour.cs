using UnityEngine;

public class BarBehaviour : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS
    
    //Variables
    private bool selected = false;
    public float strengthMultiplier, speed; //La velocidad es asignada desde el editor
    private Vector2 initialPosition = new Vector2(1.0f, 0.0f); //Coordenadas locales de la posición inicial de la barra indicadora
    private float currentPositionX; //La posición actual de la barra indicadora


//FLUJO DE UNITY
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localPosition = initialPosition; //Posición inicial de la barra indicadora (posición LOCAL)
    }

    // Update is called once per frame
    void Update()
    {
        currentPositionX = transform.localPosition.x; //De nuevo, LOCAL
        if(!selected)
            SelectionBarMovement();
        UpdateStrengthMultiplier(); //Actualizar valor del multiplicador constantemente en función de la posición LOCAL
    }

    //Cada vez que se active la barra de fuerza reseteamos "selected" y mandamos la barra indicadora a su posición inicial
    private void OnEnable()
    {
        selected = false;
        transform.localPosition = initialPosition;
    }

    //FUNCIONES

    //Movimiento de la barra indicadora
    private void SelectionBarMovement()
    {
        //Cambiar la velocidad si pasa de los límites
        if (transform.localPosition.x > 4.6f)
            speed = -speed;
        if (transform.localPosition.x < 1.0f)
            speed = Mathf.Abs(speed);
        
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y); //Mover la barra indicadora
    }

    //Actualiza el valor del multiplicador de fuerza en función de sus coordenadas locales
    private void UpdateStrengthMultiplier()
    {
        strengthMultiplier = currentPositionX;
    }

    //Detener la barra indicadora
    public void StopBar()
    {
        selected = true;
        speed = 0.0f;
    }

    //Devuelve el valor del multiplicador (para que el disco lo emplee)
    public float GetStrengthMultiplier()
    {
        return strengthMultiplier;
    }
}