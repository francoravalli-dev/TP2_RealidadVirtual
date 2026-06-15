using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SudokuManager : MonoBehaviour
{
    [Header("Fichas (las de abajo)")]
    public Button[] fichas;

    [Header("Tipo de icono de cada ficha (mismo orden que 'fichas')")]
    public TipoIcono[] tipoDeFicha;

    [Header("Celdas del tablero")]
    public Celda[] celdas;

    [Header("Botón Comprobar")]
    public GameObject botonComprobar;

    [Header("Sprites de fondo para fichas")]
    public Sprite fondoNormalFicha;
    public Sprite fondoSeleccionado;

    [Header("Panel de éxito")]
    public GameObject panelMensajeExito;
    public GameObject panelSudoku; // el panel completo del sudoku, para cerrarlo al final

    [Header("Interacción")]
    public InteraccionSudoku interaccionSudoku;
    
    [Header("Tiempos del mensaje de éxito")]
    public float tiempoAntesDeMostrar = 0.5f;
    public float tiempoVisible = 2f;

    private Button fichaSeleccionada = null;
    private int indiceFichaSeleccionada = -1;

    void Start()
    {
        botonComprobar.SetActive(false);

        for (int i = 0; i < fichas.Length; i++)
        {
            int idx = i;
            fichas[idx].onClick.AddListener(() => SeleccionarFicha(idx));
        }

        foreach (Celda celda in celdas)
        {
            Celda c = celda;
            c.GetComponent<Button>().onClick.AddListener(() => ClickEnCelda(c));
        }
    }

    Image GetIconoDeFicha(Button ficha)
    {
        if (ficha.transform.childCount > 0)
            return ficha.transform.GetChild(0).GetComponent<Image>();

        return null;
    }

    void SeleccionarFicha(int indice)
    {
        if (indiceFichaSeleccionada == indice)
        {
            fichas[indice].GetComponent<Image>().sprite = fondoNormalFicha;
            fichaSeleccionada = null;
            indiceFichaSeleccionada = -1;
            return;
        }

        if (fichaSeleccionada != null)
            fichaSeleccionada.GetComponent<Image>().sprite = fondoNormalFicha;

        fichaSeleccionada = fichas[indice];
        indiceFichaSeleccionada = indice;
        fichaSeleccionada.GetComponent<Image>().sprite = fondoSeleccionado;
    }

    void ClickEnCelda(Celda celda)
    {
        if (celda.esFija) return;

        // CASO 1: celda ocupada -> revertir (vuelve la ficha a la bandeja)
        if (celda.IDActual != "")
        {
            int idx = int.Parse(celda.IDActual);
            fichas[idx].gameObject.SetActive(true);
            fichas[idx].GetComponent<Image>().sprite = fondoNormalFicha;

            celda.icono.sprite = null;
            celda.icono.gameObject.SetActive(false);
            celda.IDActual = "";
            celda.ResetearAVacia();

            VerificarFichasColocadas();
            return;
        }

        // CASO 2: celda vacía con ficha seleccionada -> colocarla
        if (fichaSeleccionada != null)
        {
            Image iconoFicha = GetIconoDeFicha(fichaSeleccionada);
            celda.icono.sprite = iconoFicha != null ? iconoFicha.sprite : null;
            celda.icono.gameObject.SetActive(true);
            celda.IDActual = indiceFichaSeleccionada.ToString();
            celda.ResetearAOcupada();

            fichaSeleccionada.gameObject.SetActive(false);
            fichaSeleccionada = null;
            indiceFichaSeleccionada = -1;

            VerificarFichasColocadas();
            return;
        }

        // CASO 3: celda vacía, nada seleccionado -> no hace nada
    }

    void VerificarFichasColocadas()
    {
        foreach (Button ficha in fichas)
            if (ficha.gameObject.activeSelf)
            {
                botonComprobar.SetActive(false);
                return;
            }

        botonComprobar.SetActive(true);
    }

    public void Comprobar()
    {
        bool todoCorrecto = true;

        foreach (Celda celda in celdas)
        {
            if (celda.esFija) continue;

            int idx = int.Parse(celda.IDActual);
            TipoIcono tipoColocado = tipoDeFicha[idx];

            if (tipoColocado == celda.IDCorrecto)
                celda.MarcarCorrecto();
            else
            {
                celda.MarcarError();
                todoCorrecto = false;
            }
        }

        if (todoCorrecto)
        {
            Debug.Log("✅ Puzzle completado!");
            StartCoroutine(MostrarMensajeExito());
        }
        else
        {
            Debug.Log("❌ Hay errores, revisá las celdas rojas");
        }
    }

    IEnumerator MostrarMensajeExito()
    {
        yield return new WaitForSeconds(tiempoAntesDeMostrar);

        panelMensajeExito.SetActive(true);

        yield return new WaitForSeconds(tiempoVisible);

        panelMensajeExito.SetActive(false);
        panelSudoku.SetActive(false);

        if (interaccionSudoku != null)
            interaccionSudoku.NotificarPanelCerrado();
    }
}