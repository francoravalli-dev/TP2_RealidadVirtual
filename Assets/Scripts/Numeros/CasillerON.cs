using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CasillerON : MonoBehaviour, IDropHandler {
[Header("Configuracion")]
public int valorEsperado; // 1 para Casillero_ON_1, 2 para _2, etc.
public OrdenNumerosManager manager;
[Header("Sprites de fondo")]
public Sprite fondoVacio; // gris
public Sprite fondoOcupado; // blanco
public Sprite fondoError; // rojo
public Sprite fondoCorrecto;// verde
[HideInInspector]
public DraggableNumON numeroActual = null;
public void OnDrop(PointerEventData e) {
DraggableNumON numero = e.pointerDrag?.GetComponent<DraggableNumON>();
if (numero == null) return;
// Si ya había un número, lo devuelve a ZonaNumeros
if (numeroActual != null) {
numeroActual.transform.SetParent(manager.zonaNumeros);
numeroActual = null;
}
// Pone el número en el casillero
numero.transform.SetParent(transform);
numero.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
numeroActual = numero;
GetComponent<Image>().sprite = fondoOcupado;
manager.VerificarCasilleros();
}
public void MarcarError() { GetComponent<Image>().sprite = fondoError; }
public void MarcarCorrecto() { GetComponent<Image>().sprite = fondoCorrecto; }
public void ResetearVacio() {
GetComponent<Image>().sprite = fondoVacio;
if (numeroActual != null) {
numeroActual.transform.SetParent(manager.zonaNumeros);
numeroActual = null;
}
}
}