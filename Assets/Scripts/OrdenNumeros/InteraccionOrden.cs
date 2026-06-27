using UnityEngine;

// Copia renombrada de InteraccionSudoku.cs (ver seccion 4.2 y 5 del informe del Sudoku).
// Va como componente en el objeto 3D que representa el puzzle de ordenar numeros
// (por ahora un Cube de Unity). Detecta cuando el jugador lo mira y presiona F.
public class InteraccionOrden : MonoBehaviour
{
    [Header("Identificacion")]
    public string tagInteractuable = "TableroOrden";

    [Header("Panel a abrir")]
    public GameObject panelPuzzle;

    [Header("UI de ayuda (opcional)")]
    public GameObject textoAyuda;

    private bool panelAbierto = false;
    private static InteraccionOrden instanciaActiva = null;

    void Update()
    {
        if (PuzzleManager.instancia != null && PuzzleManager.instancia.puzzleOrdenCompletado) return;

        if (panelAbierto)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape))
                CerrarPanel();
            return;
        }

        if (instanciaActiva != null) return;

        RaycastHit hit;
        bool mirandoObjeto = Physics.Raycast(
            ControladorInteraccion.camaraJugador.transform.position,
            ControladorInteraccion.camaraJugador.transform.forward,
            out hit, ControladorInteraccion.distanciaInteraccion);

        if (mirandoObjeto && hit.collider.gameObject == this.gameObject
            && hit.collider.CompareTag(tagInteractuable))
        {
            if (textoAyuda != null) textoAyuda.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F)) AbrirPanel();
        }
        else
        {
            if (textoAyuda != null) textoAyuda.SetActive(false);
        }
    }

    void AbrirPanel()
    {
        if (PuzzleManager.instancia != null && PuzzleManager.instancia.puzzleOrdenCompletado) return;

        panelPuzzle.SetActive(true);
        panelAbierto = true;
        instanciaActiva = this;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (textoAyuda != null) textoAyuda.SetActive(false);
        ControladorInteraccion.SetMovimientoHabilitado(false);
    }

    void CerrarPanel()
    {
        panelPuzzle.SetActive(false);
        panelAbierto = false;
        instanciaActiva = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ControladorInteraccion.SetMovimientoHabilitado(true);
    }

    public void NotificarPanelCerrado()
    {
        panelAbierto = false;
        instanciaActiva = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ControladorInteraccion.SetMovimientoHabilitado(true);
    }
}
