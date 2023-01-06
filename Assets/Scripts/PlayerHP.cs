using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public int HP;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHPGauge();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoDamage(int damage)
    {
        // TODO: �ǰ� ����
        HP -= damage;
        UpdateHPGauge();
        if(HP <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void UpdateHPGauge()
    {
        _slider.value = HP;
    }
}
