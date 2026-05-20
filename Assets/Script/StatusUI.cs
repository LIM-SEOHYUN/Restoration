using UnityEngine;
using UnityEngine.UI;
using TMPro; //삽입

public class StatusUI : MonoBehaviour
{
    public Slider hpBar;
    public TextMeshProUGUI hpText; //삽입
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHP(float currentHp, float maxHp)
    {
      hpBar.maxValue = maxHp;
      hpBar.value = currentHp;
      hpText.text = currentHp + " / " + maxHp;//현재체력/최대체력표시
    }
}
