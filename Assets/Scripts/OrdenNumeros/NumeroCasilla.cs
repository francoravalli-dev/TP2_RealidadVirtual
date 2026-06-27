using UnityEngine;
using UnityEngine.UI;


public class NumeroCasilla : MonoBehaviour
{
    [Header("Manager")]
    public OrdenManager manager;

    [Header("Referencias visuales")]
    public Image fondo;         
    public Image imagenNumero;  

    [Header("Sprites de fondo")]
    public Sprite fondoNormal;
    public Sprite fondoSeleccionado;
    public Sprite fondoError;
    public Sprite fondoCorrecto;

    [Header("Valor de esta casilla (asignar en Inspector, 1-9)")]
    public int valor = 1;


    [HideInInspector]
    public bool seleccionada = false;

    private Button boton;

    void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(() => manager.ClickEnCasilla(this));
    }

    public void MarcarSeleccionada()
    {
        seleccionada = true;
        fondo.sprite = fondoSeleccionado;
    }

    public void MarcarError()
    {
        fondo.sprite = fondoError;
    }

    public void MarcarCorrecto()
    {
        fondo.sprite = fondoCorrecto;
    }

    public void ResetearANormal()
    {
        seleccionada = false;
        fondo.sprite = fondoNormal;
    }
}