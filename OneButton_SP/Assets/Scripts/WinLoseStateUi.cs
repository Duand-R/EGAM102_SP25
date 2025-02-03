using UnityEngine;

public class WinLoseStateUi : MonoBehaviour
{
    public GameObject winHandle;
    public GameObject loseHandle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideAllUI();
    }

    private void HideAllUI()
    {
        winHandle.SetActive(false);
        loseHandle.SetActive(false);
    }

    public void OnGameLose()
    {
        if (loseHandle == null)
        {
            Debug.LogError("erro,check loseHandle");
            return;
        }

        HideAllUI();
        loseHandle.SetActive(true);
        Debug.Log("loseing screen active" + loseHandle.activeSelf);
    }

}
