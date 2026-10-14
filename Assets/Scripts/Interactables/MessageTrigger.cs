using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    [SerializeField] private MessageTriggerData MessageData;
    [SerializeField] private TMP_Text MessageText;

    [SerializeField] private List<string> PlayerTags = new List<string>();
    [SerializeField] private List<PlayerHealth> playerHealths = new List<PlayerHealth>();
    [SerializeField] private List<PlayerMapAreas> playerMapAreas = new List<PlayerMapAreas>();
    

    [SerializeField] private MapArea mapArea;

    private void OnEnable()
    {
        foreach (PlayerHealth playerHealth in playerHealths)
            playerHealth.OnPlayerDeath += PlayerHealth_OnDeath;

        // Subscribe to flag state changes
        if (mapArea != null)
        {
            mapArea.OnCaptured += MapArea_OnCaptured;
            
        }
    }

    private void OnDisable()
    {
        foreach (PlayerHealth playerHealth in playerHealths)
            playerHealth.OnPlayerDeath -= PlayerHealth_OnDeath;

        // Unsubscribe from flag state changes
        if (mapArea != null)
        {
            mapArea.OnCaptured -= MapArea_OnCaptured;
            
        }
    }

    private void PlayerHealth_OnDeath(GameObject source)
    {
        string playerTag = source.tag;
        PlayerTags.RemoveAll(tag => tag == playerTag);
        UpdateMessage();
    }

    private void OnTriggerEnter(Collider TriggerCollider)
    {
        string pTag = TriggerCollider.gameObject.tag;

        // Only add valid player tags
        if (pTag == "PlayerOne" || pTag == "PlayerTwo")
        {
            PlayerTags.Add(pTag);
            UpdateMessage();
        }
    }

    private void OnTriggerExit(Collider TriggerCollider)
    {
        string pTag = TriggerCollider.gameObject.tag;
        PlayerTags.Remove(pTag);
        UpdateMessage();
    }
    
    private void MapArea_OnCaptured(object sender, EventArgs e)
    {
        foreach (PlayerMapAreas area in playerMapAreas)
        {
            MessageText.SetText($"{MessageData.Text} captured by {area.team} team");
        }
    }
    private void UpdateMessage()
    {

        if (PlayerTags.Contains("PlayerOne") && !PlayerTags.Contains("PlayerTwo"))
        {
            MessageText.color = Color.red;
            MessageText.SetText($"red team capturing {MessageData.Text}");
        }
        else if (PlayerTags.Contains("PlayerTwo") && !PlayerTags.Contains("PlayerOne"))
        {
            MessageText.color = Color.blue;
            MessageText.SetText($"blue team capturing {MessageData.Text}");
        }
        else if (PlayerTags.Contains("PlayerOne") && PlayerTags.Contains("PlayerTwo"))
        {
            MessageText.color = Color.magenta;
            MessageText.SetText($"both teams contesting {MessageData.Text}");
        }
        else
        {
            // No players on flag - clear text
            MessageText.SetText("");
        }
    }
}
