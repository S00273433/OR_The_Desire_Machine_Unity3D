using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class CaptureProgressUI_blue : MonoBehaviour
{

    [SerializeField] private List<MapArea> mapAreasList;

    
    private Image progressImgBlue;
    [SerializeField] PlayerMapAreas playerMapAreaBlue;
    [SerializeField] private PlayerHealth pHealthBlue;


    // private Text txtCapturing;

    private void Awake()
    {
        progressImgBlue = transform.Find("ProgressImgBlue").GetComponent<Image>();
        //txtCapturing = transform.Find("txtCapturing").GetComponent<Text>();
    }

    private void Start()
    {
        foreach (MapArea mapArea in mapAreasList)
        {
            mapArea.OnPlayerEnter += MapArea_OnPlayerEnter;
            mapArea.OnPlayerExit += MapArea_OnPlayerExit;
            pHealthBlue.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
            
        }

        Hide();

    }

    private void Update()
    {
        if (playerMapAreaBlue.GetTeam() == Team.Blue)
        {
            foreach (MapArea mapArea in mapAreasList)
            {
                if (mapArea != null)
                {
                    /*if (mapArea.GetCapState() == MapArea.CapState.BlueCapturing)
                    {
                        Show();
                        progressImgBlue.fillAmount = mapArea.GetCapProgress();
                    }
                    else if (mapArea.GetCapState() == MapArea.CapState.CapturedbyBlue)
                    {
                        progressImgBlue.fillAmount = 1f;
                    }
                    else if (mapArea.GetCapState() == MapArea.CapState.RedCapturing)
                    {
                        Hide();
                    }
                    else if (mapArea.GetCapState() == MapArea.CapState.Nuetral)
                    {
                        Hide();
                    }*/

                    switch (mapArea.GetCapState())
                    {
                        case MapArea.CapState.BlueCapturing:
                            Show();
                            progressImgBlue.fillAmount = mapArea.GetCapProgress();
                            break;

                        case MapArea.CapState.CapturedbyBlue:

                            progressImgBlue.fillAmount = 1f;
                            break;

                        case MapArea.CapState.RedCapturing:
                        case MapArea.CapState.Nuetral:
                            float currentProgress = mapArea.GetCapProgress();
                            progressImgBlue.fillAmount -= currentProgress * Time.deltaTime;
                            break;
                    }
                }
            }
        }
        else Hide();

    }
    private void MapArea_OnPlayerEnter(object sender, System.EventArgs e)
    {
        foreach (MapArea mapArea in mapAreasList)
        {
            if (playerMapAreaBlue.GetTeam() == Team.Blue)
            {
                Show();
            }

            else Hide();
        }

    }


    private void MapArea_OnPlayerExit(object sender, System.EventArgs e)
    {
        if (playerMapAreaBlue.GetTeam() == Team.Blue)
        {
            
            Hide();
        }
    }

    private void PlayerHealth_OnPlayerDeath(GameObject player)
    {
        if (playerMapAreaBlue.GetTeam() == Team.Blue)
        {
            Hide();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
