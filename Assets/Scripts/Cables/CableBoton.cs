using UnityEngine;
using UnityEngine.UI;

public enum LadoCable { Izquierda, Derecha }

public class CableBoton : MonoBehaviour {

    [Header("Configuracion")]
    public string colorCable;   // 'Rojo', 'Verde', 'Azul', 'Cyan'
    public LadoCable lado;      // Izquierda o Derecha
    public CablesManager manager;

    private Image imagen;

    void Awake() {
        imagen = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(AlClickear);
    }

    void AlClickear() {
        manager.ClickEnCable(this);
    }

    // Cambia el color de la imagen segun el estado
    public void PonerSeleccionado() { imagen.color = Color.white; }
    public void PonerNormal()       { imagen.color = new Color(0.6f, 0.6f, 0.6f); }
    public void PonerError()        { imagen.color = Color.red; }
    public void PonerCorrecto()     { imagen.color = Color.green; }
}
