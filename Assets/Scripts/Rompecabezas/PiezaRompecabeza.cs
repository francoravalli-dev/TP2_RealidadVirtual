using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Va en cada una de las 9 piezas. Necesita Image (el sprite placeholder)
// y un hijo llamado "Borde" con una Image blanca (desactivada por defecto)
// que se prende/apaga para marcar seleccion.
public class PiezaRompecabeza : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Identidad de la pieza")]
    public int indiceCorrecto; // 0-8, posicion donde deberia terminar
    [HideInInspector] public int indiceActual; // 0-8, posicion donde esta ahora

    [Header("Referencias visuales")]
    public GameObject borde; // hijo con Image blanca para marcar seleccion
    public RectTransform rect;

    [Header("Hover")]
    public float escalaHover = 1.1f;
    public float velocidadEscala = 10f;

    [HideInInspector] public bool seleccionada = false;

    [HideInInspector] public RompecabezaManager manager; // asignado por el manager en IniciarPuzzle
    private Vector3 escalaNormal;
    private float escalaObjetivo;
    private int indiceHermanoOriginal;
    private bool interactuable = true;

    void Awake()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        escalaNormal = rect.localScale;
        escalaObjetivo = 1f;
        if (manager == null) manager = GetComponentInParent<RompecabezaManager>();
        if (borde != null) borde.SetActive(false);
        indiceHermanoOriginal = transform.GetSiblingIndex();
    }

    void Update()
    {
        // Lerp suave de escala para el hover
        Vector3 objetivo = escalaNormal * escalaObjetivo;
        rect.localScale = Vector3.Lerp(rect.localScale, objetivo, Time.deltaTime * velocidadEscala);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactuable) return;
        escalaObjetivo = escalaHover;
        transform.SetAsLastSibling(); // la trae al frente para que no la tapen las vecinas
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!interactuable) return;
        escalaObjetivo = 1f;
        if (!seleccionada) RestaurarOrden();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactuable) return;
        if (manager != null) manager.ClickEnPieza(this);
    }

    public void MarcarSeleccionada()
    {
        seleccionada = true;
        if (borde != null) borde.SetActive(true);
        transform.SetAsLastSibling();
    }

    public void Deseleccionar()
    {
        seleccionada = false;
        if (borde != null) borde.SetActive(false);
        RestaurarOrden();
    }

    // Se llama cuando el puzzle se completa: apaga hover/click y deja la pieza quieta en su lugar
    public void BloquearInteraccion()
    {
        interactuable = false;
        escalaObjetivo = 1f;
        RestaurarOrden();
    }

    // Se llama al reiniciar el puzzle (por si se reabre despues de haber sido resuelto)
    public void DesbloquearInteraccion()
    {
        interactuable = true;
    }

    void RestaurarOrden()
    {
        // Guarda la posicion relativa original entre hermanos, sin romper el orden general
        indiceHermanoOriginal = Mathf.Clamp(indiceHermanoOriginal, 0, transform.parent.childCount - 1);
        transform.SetSiblingIndex(indiceHermanoOriginal);
    }
}