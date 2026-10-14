using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CaptureProgressUI_red : MonoBehaviour
{

    [SerializeField] private List<MapArea> mapAreasList;

   
    private Image progressImgRed;
    [SerializeField] private PlayerMapAreas playerMapAreaRed;
    [SerializeField] private PlayerHealth pHealthRed;

    // private Text txtCapturing;

    private void Awake()
    {
        progressImgRed = transform.Find("ProgressImgRed").GetComponent<Image>();
        
    }

    private void Start()
    {
        foreach (MapArea mapArea in mapAreasList)
        {
            mapArea.OnPlayerEnter += MapArea_OnPlayerEnter;
            mapArea.OnPlayerExit += MapArea_OnPlayerExit;
            pHealthRed.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        }

        Hide();

    }

    

    private void Update()
    {
        if (playerMapAreaRed.GetTeam() == Team.Red)
        {
            foreach (MapArea mapArea in mapAreasList)
            {
                if (mapArea != null)
                {
                    switch (mapArea.GetCapState())
                    {
                        case MapArea.CapState.RedCapturing:
                            Show();
                            progressImgRed.fillAmount = mapArea.GetCapProgress();
                            break;

                        case MapArea.CapState.CapturedbyRed:

                            progressImgRed.fillAmount = 1f;
                            break;

                        case MapArea.CapState.BlueCapturing:
                        case MapArea.CapState.Nuetral:
                            float currentProgress = mapArea.GetCapProgress();
                            progressImgRed.fillAmount -= currentProgress * Time.deltaTime;
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
            if (playerMapAreaRed.GetTeam() == Team.Red)
            {
                Show();
            }

            else Hide();
        }
    }

    
    private void MapArea_OnPlayerExit(object sender, System.EventArgs e)
    {
        if (playerMapAreaRed.GetTeam() == Team.Red)
        {
            Hide();
        }
    }
    private void PlayerHealth_OnPlayerDeath(GameObject player)
    {
        Hide();
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
