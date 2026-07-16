using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instancia;

    [Header("Estado de Puzzles")]
    public bool puzzleContrasenaCompletado = false;
    public bool puzzleOrdenCompletado = false;
    public bool puzzleRompecabezaCompletado = false;

    [Header("Panel de pieza conseguida")]
    public PanelPieza panelPieza;

    [Header("Sonido de puzzle completado")] 
    public AudioSource audioSource;          
    public AudioClip sonidoExito;   

    void Awake()
    {
        if (instancia != null) { Destroy(gameObject); return; }
        instancia = this;
    }
 public void ReproducirSonidoExito() // NUEVO
    {
        if (audioSource != null && sonidoExito != null)
            audioSource.PlayOneShot(sonidoExito);
    }
    public void CompletarPuzzleContraseña(Sprite spritePieza)
    {
        if (puzzleContrasenaCompletado) return;

        puzzleContrasenaCompletado = true;
        Debug.Log("PuzzleManager: ¡Puzzle de Contraseña superado!");

        if (panelPieza != null) panelPieza.MostrarPieza(spritePieza);
        VerificarProgresoGlobal();
    }

    public void CompletarPuzzleOrden(Sprite spritePieza)
    {
        if (puzzleOrdenCompletado) return;

        puzzleOrdenCompletado = true;
        Debug.Log("PuzzleManager: ¡Puzzle de Orden superado!");

        if (panelPieza != null) panelPieza.MostrarPieza(spritePieza);
        VerificarProgresoGlobal();
    }

    public void CompletarPuzzleRompecabeza(Sprite spritePieza)
    {
    if (puzzleRompecabezaCompletado) return;

    puzzleRompecabezaCompletado = true;
    Debug.Log("PuzzleManager: ¡Puzzle de Rompecabeza superado!");

    if (panelPieza != null) panelPieza.MostrarPieza(spritePieza);
    VerificarProgresoGlobal();
    }

    private void VerificarProgresoGlobal()
    {
        if (puzzleContrasenaCompletado && puzzleOrdenCompletado && puzzleRompecabezaCompletado)
            Debug.Log("¡Puzzles previos completos! Habilitando acceso al Sudoku..");
    }

}