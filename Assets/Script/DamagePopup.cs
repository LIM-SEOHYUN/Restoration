using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float moveUpSpeed = 1f;
    public float fadeSpeed = 2f;

    private Color textColor;




    void Start()
    {
        textColor = text.color;
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;
        textColor.a -= fadeSpeed * Time.deltaTime;
        text.color = textColor;

        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(int damage, bool isCritical)
    {
        text.text = damage.ToString();

        if (isCritical)
        {
            text.color = Color.red; //Ä¡¸íÅ¸ ±Û¾¾
            text.fontSize += 10;
        }
        else
        {
            text.color = Color.white;
        }
    }
}
