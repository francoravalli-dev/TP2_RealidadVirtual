using UnityEngine;
using UnityEngine.UI;

public enum TipoIcono
{
    Alien,
    Luna,
    Estrella,
    Rayo
}

public class Celda : MonoBehaviour
{
    [Header("Configuración")]
    public TipoIcono IDCorrecto;
    public bool esFija = false;

    [Header("Referencias visuales")]
    public Image fondo;
    public Image icono;

    [Header("Sprites de fondo")]
    public Sprite fondoFija;        // violeta clarito (celdas fijas)
    public Sprite fondoVacia;       // gris (celda vacía disponible)
    public Sprite fondoOcupada;     // blanco (celda no fija con ficha colocada)
    public Sprite fondoError;       // rojo
    public Sprite fondoCorrecto;    // verde

    [HideInInspector]
    public string IDActual = "";

    void Start()
    {
        if (esFija)
        {
            IDActual = IDCorrecto.ToString();
            fondo.sprite = fondoFija;
        }
        else
        {
            fondo.sprite = fondoVacia;
        }
    }

    public void MarcarError()
    {
        fondo.sprite = fondoError;
    }

    public void MarcarCorrecto()
    {
        fondo.sprite = fondoCorrecto;
    }

    public void ResetearAVacia()
    {
        fondo.sprite = fondoVacia;
    }

    public void ResetearAOcupada()
    {
        fondo.sprite = fondoOcupada;
    }
}