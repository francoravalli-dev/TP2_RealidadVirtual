using System.Collections;
using UnityEngine;

// Objeto vacío OrdenManager dentro del panel PanelOrden.
// Los valores de las casillas se asignan desde el Inspector de cada NumeroCasilla.
public class OrdenManager : MonoBehaviour
{
    [Header("Casillas (las 9, en el orden que quieras)")]
    public NumeroCasilla[] casillas;

    [Header("Paneles")]
    public GameObject panelCompletado;
    public GameObject panelOrden;

    [Header("Interaccion")]
    public InteraccionOrden interaccionOrden;

    [Header("Tiempos")]
    public float tiempoMensajeCompletado = 2f;
    public float tiempoMostrarError = 1f;

    [Header("Pieza que otorga este puzzle")]
    public Sprite spritePieza;

    private int siguienteNumero = 1;
    private bool bloqueado = false;

    void OnEnable()
    {
        IniciarPuzzle();
    }

    void IniciarPuzzle()
    {
        siguienteNumero = 1;
        bloqueado = false;
        if (panelCompletado != null) panelCompletado.SetActive(false);

        // Solo resetea el estado visual; los valores y sprites ya están en el Inspector
        foreach (NumeroCasilla c in casillas)
            c.ResetearANormal();
    }

    public void ClickEnCasilla(NumeroCasilla casilla)
    {
        if (bloqueado || casilla.seleccionada) return;

        if (casilla.valor == siguienteNumero)
        {
            casilla.MarcarSeleccionada();
            siguienteNumero++;

            if (siguienteNumero > 9)
                StartCoroutine(PuzzleCompletado());
        }
        else
        {
            StartCoroutine(MostrarError());
        }
    }

    IEnumerator MostrarError()
    {
        bloqueado = true;

        foreach (NumeroCasilla c in casillas)
            c.MarcarError();

        yield return new WaitForSeconds(tiempoMostrarError);

        // Solo resetea el visual, NO reordena ni reasigna valores
        IniciarPuzzle();
    }

    IEnumerator PuzzleCompletado()
{
    bloqueado = true;

    foreach (NumeroCasilla c in casillas)
        c.MarcarCorrecto();

    if (panelCompletado != null) panelCompletado.SetActive(true);
     if (PuzzleManager.instancia != null)                            
        PuzzleManager.instancia.ReproducirSonidoExito();
    yield return new WaitForSeconds(tiempoMensajeCompletado);
    if (panelCompletado != null) panelCompletado.SetActive(false);
    if (panelOrden != null) panelOrden.SetActive(false);

    if (PuzzleManager.instancia != null)
        PuzzleManager.instancia.CompletarPuzzleOrden(spritePieza);

    if (interaccionOrden != null)
        interaccionOrden.NotificarPanelCerrado();
}
}