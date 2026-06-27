using UnityEngine;

public class PuertaSudoku : MonoBehaviour
{
    [Header("Las dos hojas de la puerta")]
    public GameObject hojaDerecha;  // circle.002
    public GameObject hojaIzquierda; // circle.003

    private bool abierta = false;

    void Update()
    {
        if (abierta || PuzzleManager.instancia == null) return;

        if (PuzzleManager.instancia.puzzleContrasenaCompletado &&
            PuzzleManager.instancia.puzzleOrdenCompletado)
        {
            AbrirPuerta();
        }
    }

    void AbrirPuerta()
    {
        abierta = true;
        if (hojaDerecha != null) hojaDerecha.SetActive(false);
        if (hojaIzquierda != null) hojaIzquierda.SetActive(false);
    }
}