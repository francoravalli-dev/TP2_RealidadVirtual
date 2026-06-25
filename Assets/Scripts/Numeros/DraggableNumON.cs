using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DraggableNumON : MonoBehaviour,
IBeginDragHandler, IDragHandler, IEndDragHandler {
[Header("Configuracion")]
public int miValor; // poner el número que es (1 al 8)
private RectTransform rectTransform;
private CanvasGroup canvasGroup;
private Vector3 posicionOriginal;
private Transform padreOriginal;
void Awake() {
rectTransform = GetComponent<RectTransform>();
canvasGroup = GetComponent<CanvasGroup>();
}
public void OnBeginDrag(PointerEventData e) {
posicionOriginal = rectTransform.position;
padreOriginal = transform.parent;
transform.SetParent(transform.root); // se pone encima de todo
canvasGroup.blocksRaycasts = false; // deja pasar el click al casillero
}
public void OnDrag(PointerEventData e) {
rectTransform.position = e.position;
}
public void OnEndDrag(PointerEventData e) {
canvasGroup.blocksRaycasts = true;
// Si no cayó en ningún casillero → vuelve a su lugar
if (transform.parent == transform.root) {
transform.SetParent(padreOriginal);
rectTransform.position = posicionOriginal;
}
}
}