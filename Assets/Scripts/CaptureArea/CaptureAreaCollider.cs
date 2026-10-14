using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;


public class CaptureAreaCollider : MonoBehaviour
{
    //handkes the collider for the capture area, and keeps track of players inside the area

    public event EventHandler OnPlayerEnter;
    public event EventHandler OnPlayerExit;
    
    public Team team;

    private List<PlayerMapAreas> playerMapAreasList = new List<PlayerMapAreas>();

    private void OnTriggerEnter(Collider collider)
    {
          if (collider.TryGetComponent<PlayerMapAreas>(out PlayerMapAreas playerMapAreas))
            {
            playerMapAreasList.Add(playerMapAreas);
            OnPlayerEnter?.Invoke(this, EventArgs.Empty);

            
            
            //Debug.Log("Player entered capture area");
        }
    }
    public void RemovePlayerOnDeath(GameObject deadPlayer)
    {
        //removes player from capture area list on death, called by player health on death event
        for (int i = playerMapAreasList.Count - 1; i >= 0; i--)
        {
            PlayerMapAreas playerMapArea = playerMapAreasList[i];
            if (playerMapArea != null && playerMapArea.gameObject == deadPlayer)
            {
                playerMapAreasList.RemoveAt(i);
                OnPlayerExit?.Invoke(this, EventArgs.Empty);
                break; // Exit after removing the player
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<PlayerMapAreas>(out PlayerMapAreas playerMapAreas))
        {
            
            playerMapAreasList.Remove(playerMapAreas);
            OnPlayerExit?.Invoke(this, EventArgs.Empty);
            //  Debug.Log("Player exited capture area");
        }
    }

    public List<PlayerMapAreas> GetPlayerMapAreasList()
    {
        return playerMapAreasList;
    }

}



