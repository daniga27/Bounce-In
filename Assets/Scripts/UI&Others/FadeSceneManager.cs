using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script también sacado de un tutorial (añadido en la Documentación)

public class FadeSceneManager : MonoBehaviour
{
//VARIABLES, COMPONENTES Y GAMEOBJECTS
    
    public static FadeSceneManager Instance;
    public Image fadeImage;


//FLUJO DE UNITY

    private void Awake()
    {
        // Singleton (en la documentación explicamos lo que es y para qué sirve)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


//FUNCIONES

    //Cambio de escenas con fade intermedio --> Se le pasa como argumento la escena a la que cambia y la duración del efecto
    public void FadeAndLoad(string sceneName, float duration)
    {
        StartCoroutine(FadeAndLoadRoutine(sceneName, duration));
    }


//CORRUTINAS
    
    //Fade in (vuelve la pantalla negra)
    private IEnumerator FadeIn(string sceneName, float duration)
    {
        float time = 0;
        Color color = fadeImage.color;

        //Va subiendo el valor alpha (opacidad) de la imagen negra que ocupa toda la pantalla
        while(time < duration)
        {
            time += Time.deltaTime;
            color.a = time / duration;
            fadeImage.color = color;
            yield return null;
        }
        
        //Finalmente carga la escena
        SceneManager.LoadScene(sceneName);  
    }

    //Fade out (quita la pantalla negra)
    private IEnumerator FadeOut()
    {
        float time = 0;
        Color color = fadeImage.color;

        //Al contrario que el Fade in, va bajando el valor alpha (opacidad) de la imagen negra hasta dejar la escena visible
        while (time < 1)
        {
            time += Time.deltaTime;
            color.a = 1f - (time / 1f);
            fadeImage.color = color;
            yield return null;
        }
    }

    //Hace tanto el fade in como el fade out
    private IEnumerator FadeAndLoadRoutine(string sceneName, float duration)
    {
        //Fade In (la pantalla se oscurece)
        yield return StartCoroutine(FadeIn(sceneName, duration));

        SceneManager.LoadScene(sceneName);

        //Esperar un frame para que la escena cargue
        yield return null;

        //Fade Out (La pantalla se aclara)
        yield return StartCoroutine(FadeOut());
    }

}
