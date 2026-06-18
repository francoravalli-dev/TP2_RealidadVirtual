using UnityEngine;
using UnityEngine.UI;

public class PasswordPuzzle : MonoBehaviour
{
    public string palabraCorrecta = "METEORO";
    
    private string respuestaJugador = "";

    public PuzzleManager puzzleManager; 

    public void OnLetterClicked(string letra)
    {
        respuestaJugador += letra;
        Debug.Log("Letras ingresadas: " + respuestaJugador);

        if (respuestaJugador.Length == palabraCorrecta.Length)
        {
            VerificarContrasena();
        }
    }

    private void VerificarContrasena()
    {
        if (respuestaJugador == palabraCorrecta)
        {
            Debug.Log("¡Contraseña correcta! Acceso desbloqueado.");
            
            if (puzzleManager != null)
            {
                puzzleManager.CompletarPuzzleContraseña(); 
            }
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Error: Contraseña incorrecta. Reiniciando secuencia...");
            
            respuestaJugador = "";
        }
    }
}