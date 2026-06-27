using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelPieza : MonoBehaviour
{
    [Header("Image que muestra el sprite de la pieza")]
    public Image imagenPieza;

    [Header("Tiempo visible (segundos)")]
    public float tiempoVisible = 3f;

    private Coroutine coroutineActiva;

    public void MostrarPieza(Sprite sprite)
    {
        if (coroutineActiva != null) StopCoroutine(coroutineActiva);
        coroutineActiva = StartCoroutine(Mostrar(sprite));
    }

    IEnumerator Mostrar(Sprite sprite)
    {
        imagenPieza.sprite = sprite;
        imagenPieza.gameObject.SetActive(true);

        yield return new WaitForSeconds(tiempoVisible);

        imagenPieza.gameObject.SetActive(false);
        coroutineActiva = null;
    }
}