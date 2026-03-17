using UnityEngine;

public class ClickerButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        GameManager.Instance.AddEnergy(1);

        // Cambiamos la escala del botón para dar una sensación de "presionado".
        gameObject.transform.localScale = new Vector3(1.5f, 1.5f);
    }

    void OnMouseUp()
    {
        // Volvemos a la escala original del botón cuando se suelta el mouse.
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }

}
