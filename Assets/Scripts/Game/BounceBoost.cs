using System.Collections;
using UnityEngine;

public class BounceBoost : MonoBehaviour
{
    //VARIABLES, COMPONENTES Y GAMEOBJECTS
    
    //Valores
    [SerializeField] private float bounceMultiplier; //Asignar valor desde editor
    //Sonido
    private AudioSource audioSource; //Sonido de bounce boost
    //Escala inicial
    private Vector3 initialScale;

    //FLUJO DE UNITY
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialScale = transform.localScale;
    }

    //FUNCIONES

    //Cuando un disco colisiona con el objeto que tenga este script, su velocidad es multiplicada
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity *= bounceMultiplier;
            audioSource.Play();
            StartCoroutine(ChangeScaleTemporally());
        }
    }

    //CORRUTINAS

    //Aumentar la escala para hacer el "efecto" del rebote
    private IEnumerator ChangeScaleTemporally()
    {
        transform.localScale += Vector3.one * 0.3f;
        yield return new WaitForSeconds(0.2f);
        transform.localScale = initialScale;

    }
}
