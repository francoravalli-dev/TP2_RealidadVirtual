using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CablesManager : MonoBehaviour {

    [Header("Cables izquierda (en orden: Rojo, Verde, Azul, Cyan)")]
    public CableBoton[] cablesIzquierda;

    [Header("Cables derecha (en orden: Rojo, Verde, Azul, Cyan)")]
    public CableBoton[] cablesDerecha;

    [Header("Imagenes de conexion del medio (mismo orden)")]
    public GameObject[] imagenesConexion;

    [Header("Boton validar")]
    public GameObject botonValidar;

    [Header("Panel exito")]
    public GameObject panelExito;
    public GameObject panelCables;

    [Header("Interaccion")]
    public InteraccionCables interaccion;

    [Header("Tiempos")]
    public float tiempoAntesDeMostrar = 0.5f;
    public float tiempoVisible = 2f;

    private CableBoton cableSeleccionado = null;
    private Dictionary<string, bool> conexiones = new Dictionary<string, bool>();

    void Start() {
        botonValidar.SetActive(false);
    }

    public void ClickEnCable(CableBoton cable) {
        // Si el cable ya esta conectado, ignorar
        if (conexiones.ContainsKey(cable.colorCable) && conexiones[cable.colorCable]) return;

        // Si no hay nada seleccionado
        if (cableSeleccionado == null) {
            cableSeleccionado = cable;
            cable.PonerSeleccionado();
            return;
        }

        // Si clickeo el mismo cable, deseleccionar
        if (cableSeleccionado == cable) {
            cable.PonerNormal();
            cableSeleccionado = null;
            return;
        }

        // Si los dos son del mismo lado, cambiar seleccion
        if (cableSeleccionado.lado == cable.lado) {
            cableSeleccionado.PonerNormal();
            cableSeleccionado = cable;
            cable.PonerSeleccionado();
            return;
        }

        // Son de lados distintos -> intentar conectar
        if (cableSeleccionado.colorCable == cable.colorCable) {
            // Conexion correcta -> mostrar imagen del medio y deshabilitar los cables
            MostrarConexion(cable.colorCable);
            cableSeleccionado.PonerCorrecto();
            cable.PonerCorrecto();
            // Deshabilitar el boton para que no se puedan volver a tocar
            cableSeleccionado.GetComponent<Button>().interactable = false;
            cable.GetComponent<Button>().interactable = false;
            conexiones[cable.colorCable] = true;
        } else {
            // Colores distintos -> no conectar, volver a normal
            cableSeleccionado.PonerNormal();
            cable.PonerNormal();
        }

        cableSeleccionado = null;
        VerificarTodosConectados();
    }

    void MostrarConexion(string color) {
        string[] colores = { "Rojo", "Verde", "Azul", "Cyan" };
        for (int i = 0; i < colores.Length; i++)
            if (colores[i] == color)
                imagenesConexion[i].SetActive(true);
    }

    void VerificarTodosConectados() {
        string[] colores = { "Rojo", "Verde", "Azul", "Cyan" };
        foreach (string c in colores)
            if (!conexiones.ContainsKey(c) || !conexiones[c]) return;
        botonValidar.SetActive(true);
    }

    public void Validar() {
        Debug.Log("Puzzle completado!");
        StartCoroutine(MostrarExito());
    }

    IEnumerator MostrarExito() {
        yield return new WaitForSeconds(tiempoAntesDeMostrar);
        panelExito.SetActive(true);
         if (PuzzleManager.instancia != null)                            
        PuzzleManager.instancia.ReproducirSonidoExito();
        yield return new WaitForSeconds(tiempoVisible);
        panelExito.SetActive(false);
        panelCables.SetActive(false);
        if (interaccion != null) interaccion.NotificarPanelCerrado();
    }
}
