using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Estado de Puzzles")]
    public bool puzzleContrasenaCompletado = false;
    public bool puzzleSimonCompletado = false;
    public bool puzzleColoresCompletado = false;

    [Header("Inventario")]
    public int piezasSudoku = 0;
    public int totalPiezasNecesarias = 2;

    public void CompletarPuzzleContraseña()
    {
        if (!puzzleContrasenaCompletado)
        {
            puzzleContrasenaCompletado = true;
            Debug.Log("PuzzleManager: ¡Puzzle de Contraseña superado!");

            EntregarPiezaSudoku();
            VerificarProgresoGlobal();
        }
    }

    public void CompletarPuzzleSimon()
    {
        if (!puzzleSimonCompletado)
        {
            puzzleSimonCompletado = true;
            Debug.Log("PuzzleManager: ¡Simón Dice superado!");

            EntregarPiezaSudoku();
            VerificarProgresoGlobal();
        }
    }

    private void EntregarPiezaSudoku()
    {
        piezasSudoku++;
        Debug.Log("Piezas de Sudoku obtenidas: " + piezasSudoku + "/" + totalPiezasNecesarias);
    }

    private void VerificarProgresoGlobal()
    {
        if (puzzleContrasenaCompletado && puzzleSimonCompletado)
        {
            Debug.Log("¡Todos los puzzles previos completados! Habilitando acceso al Sudoku Final...");
        }
    }
}