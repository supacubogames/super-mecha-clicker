using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _textPos;

    [SerializeField] private float _speed = 0.5f; // Velocidad a la que el texto flotante se mueve hacia arriba
    [SerializeField] private float _fadeSpeed = 0.5f; // Velocidad a la que el texto flotante se desvanece (reduce su opacidad)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _textPos = gameObject.GetComponent<Transform>();
        _text = gameObject.GetComponent<TMP_Text>();
    }

    void Start()
    {
        Destroy(gameObject, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        _textPos.Translate(Vector3.up * _speed * Time.deltaTime);

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - _fadeSpeed * Time.deltaTime);

    }

    // Método para establecer el texto del texto flotante, que será llamado desde el GameManager cada 
    // vez que se agregue energía (ya sea por clics o por energía pasiva).
    public void SetText(string text)
    {
        _text.text = text;
    }
}
