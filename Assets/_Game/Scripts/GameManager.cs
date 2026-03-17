using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 1. SINGLETON (Para acceder fácilmente desde otras clases)
    public static GameManager Instance;

    // Método Awake se llama cuando la instancia del script se carga
    private void Awake()
    {
        // Si no hay una instancia de la clase, asigna esta instancia. Si ya existe una, destruye el objeto duplicado.
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 2. CAMPOS (Variables y propiedades)
    [SerializeField]
    // Variable privada para almacenar la energía del jugador
    private float _energy;

    [SerializeField]
    private float _multiplier = 1f; // Multiplicador de clics, empieza en 1 (sin bonus)

    [SerializeField]
    private bool _isEnabledIdleMultiplier; //Verifica si el upgrade de multiplicador de energía pasiva está habilitado o no

    // 3. EVENTOS (Las señales de radio que otras clases pueden escuchar)

    // Evento que se dispara cuando la energía cambia. Esto permite que otras clases se enteren de los cambios en la energía.
    // En particular, este evento espera un metodo que reciba un float como parametro, que representará 
    // el nuevo valor de la energía después del cambio.
    public event Action<float> OnEnergyChanged;

    // 4. PROPIEDADES (Los guardias de las variables)
    // Propiedad para acceder y modificar la energía
    public float Energy
    {
        // El getter devuelve el valor actual de la energía, mientras que el setter permite modificar la energía pero con una protección para evitar valores negativos.
        get { return _energy; }
        set
        {
            if (value < 0)
            {
                _energy = 0;
            }
            else
            {
                _energy = value;
            }
        }
    }

    // 5. MÉTODOS (Las acciones que puede realizar la clase/ la logic de negocio)

    // Metodo que cambia la energía del jugador. 
    // El metodo cumple con la firma del evento, ya que recibe un float (el nuevo valor de energía) y no devuelve nada (void).
    public void AddEnergy(float amount)
    {
        // A. Modificamos el valor (usando la Property para que proteja de negativos)
        Energy += amount * _multiplier;

        // B. Gritamos el cambio a los oyentes
        // El signo "?" revisa si alguien está escuchando. Si nadie escucha, no hace nada (evita errores).
        // EL Invoke(Energy) es lo que realmente dispara el evento, y le pasa el valor actual de la energía a los oyentes.
        // En este caso, el oyente es el UIManager, que se suscribió al evento y tiene un método que se llama UpdateEnergyUI, 
        // el cual recibe el valor de energía para actualizar la UI. El event constantemente revisa si el valor
        // de energía ha cambiado, y si es así, llama a UpdateEnergyUI con el nuevo valor de energía.
        OnEnergyChanged?.Invoke(Energy);

        // C. (Opcional) Un log para nosotros mismos
        Debug.Log($"[GameManager] Energía actual: {Energy}");
    }

    // Metodo para aplicar el upgrade de multiplicador de clics. Recibe el monto a agregar al multiplicador y el costo del upgrade.
    // Devuelve un booleano para indicar si el upgrade se aplicó correctamente (true) o no (false, por falta de energía).
    public bool AddMultiplier(float amountToAdd, float amountToCharge)
    {
        if (_energy >= amountToCharge)
        {
            _energy -= amountToCharge;
            _multiplier *= amountToAdd;

            // Después de modificar la energía, también debemos notificar a los oyentes del cambio, 
            // ya que la energía se ha reducido debido al costo del upgrade.
            // El "?" asegura que solo se intente invocar el evento si hay oyentes suscritos, evitando errores si no hay ninguno.
            OnEnergyChanged?.Invoke(Energy);

            // Devuelve true para indicar que el upgrade se aplicó correctamente.
            return true;
        }
        // Si no hay suficiente energía para pagar el costo del upgrade, no se aplica y se devuelve false.
        return false;
    }

    [SerializeField] float _idleEnergyMultiplierAmount = 1f; // El monto a agregar al multiplicador de energía pasiva (idle energy) por cada upgrade comprado.
    public bool AddIdleMutliplier(float amountToAdd, float amountToCharge)
    {
        // Este método es similar a AddMultiplier, pero se utiliza para aplicar el upgrade de multiplicador de energía pasiva (idle energy).
        // Se verifica si el jugador tiene suficiente energía para pagar el costo del upgrade.
        if (_energy >= amountToCharge)
        {
            // Si hay suficiente energía, se resta el costo del upgrade de la energía actual.
            _energy -= amountToCharge;

            if (!_isEnabledIdleMultiplier)
            {
                // Si el upgrade de multiplicador de energía pasiva no está habilitado, lo habilitamos y establecemos el monto a agregar al multiplicador.
                _isEnabledIdleMultiplier = true;

                // Iniciamos la corrutina que se encargará de agregar energía pasiva cada segundo, multiplicada por el monto del upgrade.
                StartCoroutine(IdleEnergyCoroutine());
            }
            else
            {
                // Si el upgrade de multiplicador de energía pasiva ya está habilitado, simplemente aumentamos el monto a agregar al multiplicador.
                _idleEnergyMultiplierAmount *= 2f;
            }

            // Después de modificar la energía, también debemos notificar a los oyentes del cambio, 
            // ya que la energía se ha reducido debido al costo del upgrade.
            // El "?" asegura que solo se intente invocar el evento si hay oyentes suscritos, evitando errores si no hay ninguno.
            OnEnergyChanged?.Invoke(Energy);
            Debug.Log("Idle Multiplier Upgrade Applied! " + amountToAdd);

            // Devuelve true para indicar que el upgrade se aplicó correctamente.
            return true;

        }
        else
        {
            // Devuelve false para indicar que el upgrade no se aplicó debido a la falta de energía.
            return false;
        }
    }

    private IEnumerator IdleEnergyCoroutine()
    {
        while (_isEnabledIdleMultiplier)
        {
            yield return new WaitForSeconds(1f); // Espera 1 segundo
            Energy += _idleEnergyMultiplierAmount; // Agrega 1 de energía cada segundo
            OnEnergyChanged?.Invoke(Energy); // Notifica a los oyentes del cambio de energía    
        }
    }
}



