using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp = 10;
    public float maxHp = 10;
    public float invincibleTime;
    public Image hpProgressBar;

    float lastDamageTime;
    IHealthListener healthListener;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthListener = GetComponent<IHealthListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hpProgressBar != null)
        {
            hpProgressBar.fillAmount = hp / maxHp;
        }
    }

    public void Damage(float damage)
    {
        if (hp > 0 && lastDamageTime + invincibleTime < Time.time)
        {
            hp -= damage;

            lastDamageTime = Time.time;

            if (hp <= 0)
            {
                if (healthListener != null)
                {
                    healthListener.OnDie();
                }
            }
            else
            {
                Debug.Log("아펑");
            }
        }
    }

    public interface IHealthListener
    {
        void OnDie();
    }
}
