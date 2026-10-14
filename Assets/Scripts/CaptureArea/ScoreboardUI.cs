using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardUI : MonoBehaviour
{
    
    [SerializeField] public ConquestManager conquestManager;

    private Image progressbarRedImg;
    private Image progressbarBlueImg;
    public float progressbarRedFill;
    public float progressbarBlueFill;
    [SerializeField] private float fillSpeed = 1.5f;

    private void Awake()
    {
        progressbarRedImg = transform.Find("progressbarRedImg").GetComponent<Image>();
        progressbarBlueImg = transform.Find("progressbarBlueImg").GetComponent<Image>();
    }

    private void Start()
    {
        
            

        
    }

    private void Update()
    {
        if (conquestManager != null)
        {


            float redTargetFill = Mathf.Clamp01(conquestManager.RedTeamScore / conquestManager.targetScore);
            float blueTargetFill = Mathf.Clamp01(conquestManager.BlueTeamScore / conquestManager.targetScore);


            progressbarRedImg.fillAmount = Mathf.MoveTowards(
                    progressbarRedImg.fillAmount,
                    redTargetFill,
                    fillSpeed * Time.deltaTime
                );

            progressbarBlueImg.fillAmount = Mathf.MoveTowards(
                progressbarBlueImg.fillAmount,
                blueTargetFill,
                fillSpeed * Time.deltaTime
            );

        }
    }


    private void ConquestManager_OnScoreUpdated(object sender, System.EventArgs e)
    {
        conquestManager = sender as ConquestManager;


        
    }
}
