using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Transform textPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textPos = gameObject.GetComponent<Transform>();
        text = gameObject.GetComponent<TMP_Text>();

        Destroy(gameObject, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        textPos.Translate(Vector3.up * 0.5f * Time.deltaTime);

        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.5f * Time.deltaTime);
  
    }
}
