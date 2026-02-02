using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject HeathBarRoot;
    [SerializeField] private Boss boss;

    [SerializeField] private Image bar;

    [SerializeField] private GameObject WinScreen;
    
    private float fillAnim = 0;

    private float WinDelay = 10;
    
    void Start()
    {
        WinScreen.SetActive(false);
    }

    void Update()
    {
        if (boss.PlayerInRange())
        {
            HeathBarRoot.SetActive(true);

            float fillPercent = boss.GetHealth() / boss.GetMaxHealth();
            
            if (fillAnim < fillPercent)
            {
                fillAnim += Time.deltaTime * .2f;
                fillPercent = fillAnim;
            }

            bar.fillAmount = fillPercent;

        }
        else
        {
            HeathBarRoot.SetActive(false);
        }

        if (boss.GetHealth() < 0)
        {
            WinDelay -= Time.deltaTime;
            if (WinDelay < 0)
            {
                WinScreen.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SceneManager.LoadScene("SampleScene");
                }
            }
        }
        
    }
}
