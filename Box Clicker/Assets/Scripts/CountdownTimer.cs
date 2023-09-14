using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using System.Collections;

public class CountdownTimer : MonoBehaviourPunCallbacks
{
    public TMP_Text countdownText; // The TextMesh Pro Text component
    public GameObject gameStartObject; // The GameObject to enable after countdown

    public event Action OnCountdownComplete; // Event to notify when the countdown is complete

    private bool isCountingDown = false;

    public void StartCountdown()
    {
        // Check if the current client is the master client
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartCountdownRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void StartCountdownRPC()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        isCountingDown = true;
        int countdown = 5;

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();

            // Synchronize the countdown across all clients
            photonView.RPC("SyncCountdownRPC", RpcTarget.All, countdown);

            yield return new WaitForSeconds(1.0f);
            countdown--;
        }

        countdownText.text = "Start";
        yield return new WaitForSeconds(1.0f);

        // Notify that the countdown is complete
        OnCountdownComplete?.Invoke();

        // After 1 second, destroy the countdown text
        if (countdownText != null)
        {
            Destroy(countdownText.gameObject);
        }

        // Enable the game object
        if (gameStartObject != null)
        {
            gameStartObject.SetActive(true);
        }

        isCountingDown = false;

        // You can add more game logic here if needed.
    }

    [PunRPC]
    private void SyncCountdownRPC(int countdownValue)
    {
        countdownText.text = countdownValue.ToString();
    }

    // Ensure that the countdown is stopped when the game object is destroyed
    private void OnDestroy()
    {
        if (isCountingDown)
        {
            StopCoroutine(CountdownCoroutine());
        }
    }
}
