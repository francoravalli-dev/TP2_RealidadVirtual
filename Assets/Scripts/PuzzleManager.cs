using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instancia;

    [Header("Estado de Puzzles")]
    public bool puzzleContrasenaCompletado = false;
    public bool puzzleOrdenCompletado = false;

    [Header("Panel de pieza conseguida")]
    public PanelPieza panelPieza;

    void Awake()
    {
        if (instancia != null) { Destroy(gameObject); return; }
        instancia = this;
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

    private void VerificarProgresoGlobal()
    {
        if (puzzleContrasenaCompletado && puzzleOrdenCompletado)
            Debug.Log("¡Puzzles previos completos! Habilitando acceso al Sudoku..");
    }
}