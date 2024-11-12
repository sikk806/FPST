using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time;
    public float damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time < 0)
        {
            GetComponent<Animator>().SetTrigger("Explosive");
            Destroy(gameObject, 2f);
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Health>().Damage(damage);
        }
    }
}
