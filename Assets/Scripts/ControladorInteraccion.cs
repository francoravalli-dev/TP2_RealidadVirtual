using UnityEngine;

public class ControladorInteraccion : MonoBehaviour
{
    public static Camera camaraJugador;
    public static float distanciaInteraccion = 3f;
    public static MonoBehaviour scriptMovimiento;

    [Header("Configuración global")]
    public Camera camara;
    public float distancia = 3f;
    public MonoBehaviour movimiento;

    void Awake()
    {
        camaraJugador = camara;
        distanciaInteraccion = distancia;
        scriptMovimiento = movimiento;
    }

    public static void SetMovimientoHabilitado(bool habilitado)
    {
        if (scriptMovimiento != null)
            scriptMovimiento.enabled = habilitado;
    }
}