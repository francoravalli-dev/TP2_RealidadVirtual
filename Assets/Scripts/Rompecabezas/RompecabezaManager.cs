using System.Collections;
using UnityEngine;


public class RompecabezaManager : MonoBehaviour
{
    [Header("Piezas (las 9)")]
    public PiezaRompecabeza[] piezas;

    [Header("Slots destino (las 9 posiciones del grid, en orden 0-8)")]
    public RectTransform[] slots;

    [Header("Paneles")]
    public GameObject panelCompletado;
    public GameObject panelRompecabeza;

    [Header("Wrapper para el feedback de imagen completa (brillo + escala)")]
    public RectTransform wrapperImagen;
    public UnityEngine.UI.Image overlayBrillo; 
    [Range(0f, 1f)] public float alphaMaxBrillo = 0.25f;

    [Header("Interaccion")]
    public InteraccionRompecabeza interaccionRompecabeza;

    [Header("Tiempos")]
    public float tiempoMensajeCompletado = 2f;
    public float duracionSwap = 0.25f;
    public float duracionFeedbackCompleto = 0.4f;

    [Header("Pieza que otorga este puzzle")]
    public Sprite spritePieza;

    private PiezaRompecabeza piezaSeleccionada = null;
    private bool bloqueado = false;
    private bool animandoSwap = false;

    void OnEnable()
    {
        IniciarPuzzle();
    }

    void IniciarPuzzle()
    {
        bloqueado = false;
        animandoSwap = false;
        piezaSeleccionada = null;
        if (panelCompletado != null) panelCompletado.SetActive(false);

      
        int[] indices = new int[piezas.Length];
        for (int i = 0; i < indices.Length; i++) indices[i] = i;

        bool resuelto;
        do
        {
            Barajar(indices);
            resuelto = true;
            for (int i = 0; i < piezas.Length; i++)
            {
                if (indices[i] != piezas[i].indiceCorrecto) { resuelto = false; break; }
            }
        } while (resuelto);

        for (int i = 0; i < piezas.Length; i++)
        {
            piezas[i].manager = this;
            piezas[i].indiceActual = indices[i];
            piezas[i].Deseleccionar();
            piezas[i].DesbloquearInteraccion();
            piezas[i].rect.anchoredPosition = slots[indices[i]].anchoredPosition;
        }
    }

    void Barajar(int[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = arr[i]; arr[i] = arr[j]; arr[j] = tmp;
        }
    }

    public void ClickEnPieza(PiezaRompecabeza pieza)
    {
        if (bloqueado || animandoSwap) return;

       
        if (piezaSeleccionada == pieza)
        {
            pieza.Deseleccionar();
            piezaSeleccionada = null;
            return;
        }

        if (piezaSeleccionada == null)
        {
            piezaSeleccionada = pieza;
            pieza.MarcarSeleccionada();
        }
        else
        {
            StartCoroutine(SwapPiezas(piezaSeleccionada, pieza));
            piezaSeleccionada = null;
        }
    }

    IEnumerator SwapPiezas(PiezaRompecabeza a, PiezaRompecabeza b)
    {
        animandoSwap = true;
        a.Deseleccionar();

        Vector2 posA = a.rect.anchoredPosition;
        Vector2 posB = b.rect.anchoredPosition;
        int slotA = a.indiceActual;
        int slotB = b.indiceActual;

        float t = 0f;
        while (t < duracionSwap)
        {
            t += Time.deltaTime;
            float pct = Mathf.Clamp01(t / duracionSwap);
            float suavizado = Mathf.SmoothStep(0f, 1f, pct);
            a.rect.anchoredPosition = Vector2.Lerp(posA, posB, suavizado);
            b.rect.anchoredPosition = Vector2.Lerp(posB, posA, suavizado);
            yield return null;
        }

        a.rect.anchoredPosition = posB;
        b.rect.anchoredPosition = posA;
        a.indiceActual = slotB;
        b.indiceActual = slotA;

        animandoSwap = false;

        if (ChequearCompleto())
        {
            foreach (PiezaRompecabeza p in piezas)
                p.BloquearInteraccion();

            StartCoroutine(PuzzleCompletado());
        }
    }

    bool ChequearCompleto()
    {
        foreach (PiezaRompecabeza p in piezas)
        {
            if (p.indiceActual != p.indiceCorrecto) return false;
        }
        return true;
    }

    IEnumerator PuzzleCompletado()
    {
        bloqueado = true;

        
        yield return StartCoroutine(FeedbackImagenCompleta());

        if (panelCompletado != null) panelCompletado.SetActive(true);
        yield return new WaitForSeconds(tiempoMensajeCompletado);
        if (panelCompletado != null) panelCompletado.SetActive(false);
        if (panelRompecabeza != null) panelRompecabeza.SetActive(false);

        if (PuzzleManager.instancia != null)
            PuzzleManager.instancia.CompletarPuzzleRompecabeza(spritePieza);

        if (interaccionRompecabeza != null)
            interaccionRompecabeza.NotificarPanelCerrado();
    }

    IEnumerator FeedbackImagenCompleta()
    {
        if (overlayBrillo == null) yield break;

        Color colorBase = overlayBrillo.color;
        colorBase.a = 0f;
        overlayBrillo.color = colorBase;

        float mitad = duracionFeedbackCompleto / 2f;
        float t = 0f;

        
        while (t < mitad)
        {
            t += Time.deltaTime;
            float pct = Mathf.Clamp01(t / mitad);
            Color c = overlayBrillo.color;
            c.a = Mathf.Lerp(0f, alphaMaxBrillo, pct);
            overlayBrillo.color = c;
            yield return null;
        }

        t = 0f;
        while (t < mitad)
        {
            t += Time.deltaTime;
            float pct = Mathf.Clamp01(t / mitad);
            Color c = overlayBrillo.color;
            c.a = Mathf.Lerp(alphaMaxBrillo, 0f, pct);
            overlayBrillo.color = c;
            yield return null;
        }

        Color final = overlayBrillo.color;
        final.a = 0f;
        overlayBrillo.color = final;
    }
}