#pragma warning disable 0649

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameControl : MonoBehaviour
{
    public static event Action HandlePulled = delegate { };
    public static event Action<int> CoinEarned = delegate { };
    public static event Action<int> PlaySound = delegate { };

    [SerializeField] private Text totalCoins_text;
    [SerializeField] private Text currentBet_text;
    [SerializeField] private Text highScore_text;

    public static int totalCoins;
    public static int currentBet;
    public static int highScore;

    [SerializeField] private Row[] rows;
    [SerializeField] private Transform handle;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject Coin;

    private int prizeValue;
    private bool resultsChecked = false;

    AudioSource pullHandleSound;
    private static bool leverIsStayingStill;

    private void Start()
    {
        leverIsStayingStill = true;
        Time.timeScale = 1;
        pullHandleSound = handle.GetComponent<AudioSource>();

        //PlayerPrefs.SetInt("highScore", 0);
        
        Screen.SetResolution(800, 600, false);
        totalCoins = (PlayerPrefs.GetInt("totalCoins") == 0 && PlayerPrefs.GetInt("currentBet") == 0) ? Constants.STARTING_COINS : PlayerPrefs.GetInt("totalCoins");
        currentBet = PlayerPrefs.GetInt("currentBet");
        highScore = (PlayerPrefs.GetInt("highScore") == 0) ? Constants.STARTING_COINS : PlayerPrefs.GetInt("highScore");
    }

    // Update is called once per frame
    void Update()
    {
        if (!rows[0].rowStopped || !rows[1].rowStopped || !rows[2].rowStopped)
        {
            prizeValue = 0;
            resultsChecked = false;
        }
        if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped && !resultsChecked)
        {
            CheckResults();

            if (totalCoins <= 0 && currentBet <= 0)
            {
                Time.timeScale = 0;
                GameOverUI.SetActive(true);
            }
        }
        totalCoins_text.text = "Total coins: " + totalCoins;
        currentBet_text.text = "Current bet: " + currentBet;
        highScore_text.text  = "High score:  " + highScore;

        PlayerPrefs.SetInt("totalCoins", totalCoins);
        PlayerPrefs.SetInt("currentBet", currentBet);
        PlayerPrefs.SetInt("highScore", highScore);
    }

    private void OnMouseDown()
    {
        if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped)
        {
            if (Time.timeScale != 0 && leverIsStayingStill)
            {
                StartCoroutine("PullHandle");
                pullHandleSound.PlayOneShot(pullHandleSound.clip);
            }
        }
    }

    private IEnumerator PullHandle()
    {
        leverIsStayingStill = false;

        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0f, 0f, i);
            yield return new WaitForSeconds(0.1f);
        }
        if (currentBet > 0)
        {
            HandlePulled();
        }
        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0f, 0f, -i);
            yield return new WaitForSeconds(0.1f);
        }
        leverIsStayingStill = true;
    }

    private void CheckResults()
    {
        if (rows[0].stoppedSlot >= 0 && rows[1].stoppedSlot >= 0 && rows[2].stoppedSlot >= 0)
        {
            if (rows[0].stoppedSlot == rows[1].stoppedSlot &&
            rows[1].stoppedSlot == rows[2].stoppedSlot &&
            rows[0].stoppedSlot == rows[2].stoppedSlot
            && rows[0].stoppedSlot != -1)
            {
                prizeValue = 4 * currentBet;
                StartCoroutine(PlayEffects(5));
            }
            else if (rows[0].stoppedSlot == rows[1].stoppedSlot ||
                rows[1].stoppedSlot == rows[2].stoppedSlot ||
                rows[0].stoppedSlot == rows[2].stoppedSlot)
            {
                StartCoroutine(PlayEffects(5));
                prizeValue = 2 * currentBet;
            }
            else
            {
                PlaySound(1);
            }
            currentBet = 0;
            totalCoins += prizeValue;

            if (totalCoins > highScore)
            {
                highScore = totalCoins;
            }
        }
        resultsChecked = true;
    }

    private IEnumerator PlayEffects(int count)
    {
        PlaySound(0);
        yield return new WaitForSeconds(1f);
        CoinEarned(count);
    }
}
