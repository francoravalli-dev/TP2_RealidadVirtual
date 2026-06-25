using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class OrdenNumerosManager : MonoBehaviour {
[Header("Casilleros (arrastrar los 8)")]
public CasillerON[] casilleros;
[Header("Zona donde están los números sueltos")]
public Transform zonaNumeros;
[Header("Botón comprobar")]
public GameObject botonComprobar;
[Header("Panel de éxito")]
public GameObject panelExito;
public GameObject panelOrdenNumeros; // este mismo panel
[Header("Interacción")]
public InteraccionOrdenNumeros interaccion;
[Header("Tiempos")]
public float tiempoAntesDeMostrar = 0.5f;
public float tiempoVisible = 2f;
void Start() {
botonComprobar.SetActive(false);
}
// Se llama cada vez que se suelta un número en un casillero
public void VerificarCasilleros() {
foreach (CasillerON c in casilleros)
if (c.numeroActual == null) {
botonComprobar.SetActive(false);
return;
}
botonComprobar.SetActive(true); // todos llenos
}
// Se llama al apretar el botón Comprobar
public void Comprobar() {
bool todoCorrecto = true;
foreach (CasillerON c in casilleros) {
if (c.numeroActual != null && c.numeroActual.miValor == c.valorEsperado)
c.MarcarCorrecto();
else {
c.MarcarError();
todoCorrecto = false;
}
}
if (todoCorrecto) {
Debug.Log("Puzzle completado!");
StartCoroutine(MostrarExito());
} else {
Debug.Log("Hay errores — casilleros rojos");
}
}
IEnumerator MostrarExito() {
yield return new WaitForSeconds(tiempoAntesDeMostrar);
panelExito.SetActive(true);
yield return new WaitForSeconds(tiempoVisible);
panelExito.SetActive(false);
panelOrdenNumeros.SetActive(false);
if (interaccion != null) interaccion.NotificarPanelCerrado();
}
}