using UnityEngine;

public class InteraccionCables : MonoBehaviour {

    [Header("Identificacion")]
    public string tagInteractuable = "PanelCables";

    [Header("Panel a abrir")]
    public GameObject panelPuzzle;

    [Header("UI de ayuda (opcional)")]
    public GameObject textoAyuda;

    private bool panelAbierto = false;
    private static InteraccionCables instanciaActiva = null;

    void Update() {
        if (panelAbierto) {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape))
                CerrarPanel();
            return;
        }

        if (instanciaActiva != null) return;

        RaycastHit hit;
        bool mirando = Physics.Raycast(
            ControladorInteraccion.camaraJugador.transform.position,
            ControladorInteraccion.camaraJugador.transform.forward,
            out hit,
            ControladorInteraccion.distanciaInteraccion
        );

        if (mirando && hit.collider.gameObject == this.gameObject
            && hit.collider.CompareTag(tagInteractuable)) {
            if (textoAyuda != null) textoAyuda.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F)) AbrirPanel();
        } else {
            if (textoAyuda != null) textoAyuda.SetActive(false);
        }
    }

    void AbrirPanel() {
        panelPuzzle.SetActive(true);
        panelAbierto = true;
        instanciaActiva = this;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (textoAyuda != null) textoAyuda.SetActive(false);
        ControladorInteraccion.SetMovimientoHabilitado(false);
    }

    void CerrarPanel() {
        panelPuzzle.SetActive(false);
        panelAbierto = false;
        instanciaActiva = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ControladorInteraccion.SetMovimientoHabilitado(true);
    }

    public void NotificarPanelCerrado() {
        panelAbierto = false;
        instanciaActiva = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ControladorInteraccion.SetMovimientoHabilitado(true);
    }
}
