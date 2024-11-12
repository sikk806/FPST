using UnityEngine;
using UnityEngine.UI;

public class ProgressBarColor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Image image = GetComponent<Image>();

        image.color = Color.HSVToRGB(image.fillAmount / 3, 1.0f, 1.0f);   
    }
}
