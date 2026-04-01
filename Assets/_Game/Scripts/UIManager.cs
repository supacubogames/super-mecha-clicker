using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{

    // 1. SINGLETON (Para acceder fácilmente desde otras clases)
    public static UIManager Instance;

    // Método Awake se llama cuando la instancia del script se carga
    private void Awake()
    {
        // Si no hay una instancia de la clase, asigna esta instancia. Si ya existe una, destruye el objeto duplicado.
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [SerializeField]
    private TextMeshProUGUI _energyText;

    // 1. Nos suscribimos al evento cuando el objeto se enciende
    private void OnEnable()
    {
        // Si la instancia del GameManager existe, inicializamos la UI con el valor actual de la energía,
        // a través del evento que ofrece el GameManager (el cual ya tiene un valor asignado que viene desde el GameManager,
        // en el campo Energy).
        if (GameManager.Instance != null)
        {
            // Nos suscribimos al evento OnEnergyChanged del GameManager.
            // La logica es: cada vez que la energía cambie, se llamará al método UpdateEnergyUI.
            // Quien realiza el cambio de energía es el GameManager, por eso accedemos a él. 
            // Y UpdateEnergyUI es el método que queremos que se llame cuando el evento ocurra.
            GameManager.Instance.OnEnergyChanged += UpdateEnergyUI;

            // Seteamos el valor inicial de la UI al valor actual de la energía en el GameManager, através del evento.
            // En la firma del evento, se espera un float, que es el valor actual de la energía.
            UpdateEnergyUI(GameManager.Instance.Energy);
        }
    }

    // 2. Nos desuscribimos del evento cuando el objeto se apaga
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnergyChanged -= UpdateEnergyUI;
        }
    }

    // 3. La reacción (El metodo que recibe el float del evento). 
    // Dicho de forma sencilla, lo que queremos hacer cuando la energía cambie.
    // El metodo recibe el valor actual de la energía como parámetro, que se obtiene del evento. 
    // El evento, a través del GameManager, nos pasa ese valor, que viene en el float currentEnergy.
    private void UpdateEnergyUI(double currentEnergy)
    {
        //Aqui se actualiza el texto en la UI. El formato "F0" redondea el número a 0 decimales.
        _energyText.text = "<sprite index=0> " + EnergyAmountFormatter(currentEnergy);
    }

    //Metodo para formatear la cantidad de energía en un formato más legible, utilizando sufijos para grandes números (K para miles, M para millones, etc.).
    public string EnergyAmountFormatter(double energy)
    {
        if(energy >= 1000000000000000000000000000000000000000000d)
        {
            return (energy / 1000000000000000000000000000000000000000000000d).ToString("F2") + "Vg";
        }
        else if(energy >= 1000000000000000000000000000000000000f)
        {
            return (energy / 1000000000000000000000000000000000000f).ToString("F2") + "U";
        }
        else if(energy >= 1000000000000000000000000000000f)
        {
            return (energy / 1000000000000000000000000000000f).ToString("F2") + "No";
        }  
        else if(energy >= 1000000000000000000000000000f)
        {
            return (energy / 1000000000000000000000000000f).ToString("F2") + "Oc";
        }
        else if(energy >= 1000000000000000000000000f)
        {
            return (energy / 1000000000000000000000000f).ToString("F2") + "Sp";
        }
        else if(energy >= 1000000000000000000000f)
        {
            return (energy / 1000000000000000000000f).ToString("F2") + "Sx";
        }   
        else if(energy >= 1000000000000000000f)
        {
            return (energy / 1000000000000000000f).ToString("F2") + "Qi";
        }

        else if(energy >= 1000000000000000f)
        {
            return (energy / 1000000000000000f).ToString("F2") + "Qa";
        }
        else if(energy >= 1000000000000f)
        {
            return (energy / 1000000000000f).ToString("F2") + "T";
        }
        else if(energy >= 1000000000f)
        {
            return (energy / 1000000000f).ToString("F2") + "B";
        }
        else if(energy >= 1000000f)
        {
            return (energy / 1000000f).ToString("F2") + "M";
        }
        else if(energy >= 1000f)
        {
            return (energy / 1000f).ToString("F2") + "K";
        }
        else
        {
            return energy.ToString("F0");
        }
    }
}
