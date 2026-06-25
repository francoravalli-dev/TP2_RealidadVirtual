using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PasswordPuzzle : MonoBehaviour
{
    [Header("Palabra correcta")]
    public string palabraCorrecta = "METEORO";

    [Header("Botones de letras")]
    public Button[] botonesLetras;

    [Header("Textos de cada botón (mismo orden que 'botonesLetras')")]
    public string[] letrasDeBoton;

    [Header("Display de la palabra en progreso")]
    public TextMeshProUGUI textoProgreso; 
    [Header("Botón Validar")]
    public GameObject botonValidar;

    [Header("Sprites de fondo para botones de letra")]
    public Sprite fondoNormalLetra;
    public Sprite fondoUsada; 
    [Header("Panel de éxito")]
    public GameObject panelMensajeExito;
    public GameObject panelContrasena; 

    [Header("Interacción")]
    public InteraccionContrasena interaccionContrasena;

    [Header("Tiempos del mensaje de éxito")]
    public float tiempoAntesDeMostrar = 0.5f;
    public float tiempoVisible = 2f;

    [Header("PuzzleManager")]
    public PuzzleManager puzzleManager;

    private string respuestaJugador = "";
    private List<int> indicesUsados = new List<int>(); 

    void Start()
    {
        botonValidar.SetActive(false);

        for (int i = 0; i < botonesLetras.Length; i++)
        {
            int idx = i;
            botonesLetras[idx].onClick.AddListener(() => ClickEnLetra(idx));
        }

        ActualizarDisplay();
    }


    void ClickEnLetra(int indice)
    {
        if (indicesUsados.Contains(indice)) return;

        if (respuestaJugador.Length >= palabraCorrecta.Length) return;

        respuestaJugador += letrasDeBoton[indice];
        indicesUsados.Add(indice);

        botonesLetras[indice].GetComponent<Image>().sprite = fondoUsada;
        botonesLetras[indice].interactable = false;

        ActualizarDisplay();

        if (respuestaJugador.Length == palabraCorrecta.Length)
        {
            botonValidar.SetActive(true);
        }
    }

    void ActualizarDisplay()
    {
        if (textoProgreso == null) return;

        string display = "";
        for (int i = 0; i < palabraCorrecta.Length; i++)
        {
            if (i < respuestaJugador.Length)
                display += respuestaJugador[i] + " ";
            else
                display += "_ ";
        }
        textoProgreso.text = display.TrimEnd();
    }


    public void Validar()
    {
        if (respuestaJugador == palabraCorrecta)
        {
            Debug.Log("✅ ¡Contraseña correcta! Acceso desbloqueado.");
            StartCoroutine(MostrarMensajeExito());
        }
        else
        {
            Debug.Log("❌ Contraseña incorrecta. Reiniciando...");
            ReiniciarMinijuego();
        }
    }


    void ReiniciarMinijuego()
    {
        respuestaJugador = "";
        indicesUsados.Clear();

        for (int i = 0; i < botonesLetras.Length; i++)
        {
            botonesLetras[i].GetComponent<Image>().sprite = fondoNormalLetra;
            botonesLetras[i].interactable = true;
        }

        botonValidar.SetActive(false);
        ActualizarDisplay();
    }


    IEnumerator MostrarMensajeExito()
    {
        botonValidar.SetActive(false);

        yield return new WaitForSeconds(tiempoAntesDeMostrar);

        panelMensajeExito.SetActive(true);

        yield return new WaitForSeconds(tiempoVisible);

        panelMensajeExito.SetActive(false);
        panelContrasena.SetActive(false);

        if (puzzleManager != null)
            puzzleManager.CompletarPuzzleContraseña();

        if (interaccionContrasena != null)
            interaccionContrasena.NotificarPanelCerrado();
    }
}
