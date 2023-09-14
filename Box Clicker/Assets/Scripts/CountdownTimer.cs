using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text countdownText; // The TextMesh Pro Text component
    public GameObject gameStartObject; // The GameObject to enable after countdown

    public event Action OnCountdownComplete; // Event to notify when the countdown is complete

    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        int countdown = 5;

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1.0f);
            countdown--;
        }

        countdownText.text = "Start";
        yield return new WaitForSeconds(1.0f);

        // Notify that the countdown is complete
        OnCountdownComplete?.Invoke();

        // After 1 second, destroy the countdown text
        Destroy(countdownText.gameObject);

        // Enable the game object
        gameStartObject.SetActive(true);

        // You can add more game logic here if needed.
    }
}