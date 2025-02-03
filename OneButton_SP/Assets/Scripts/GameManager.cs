using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public float maxTime;
    public float limitedTime;
    public TMP_Text Timer;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxTime = 0f;
        limitedTime = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer.text = limitedTime.ToString();

        limitedTime -= Time.deltaTime;
        if (limitedTime <= maxTime)
        {
            limitedTime = 30f;
            SceneManager.LoadScene("WinScene");
        }



    }
}
