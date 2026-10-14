using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ConquestManager : MonoBehaviour
{
    
     public float targetScore = 500;
     public float scoreInterval = 1f;
     public float scorePerFlag = 1;
     private float gameTimer;  

    public MapArea[] flagList;

    public float BlueTeamScore, RedTeamScore;
    public TMP_Text txtBlueTeamScore, txtRedTeamScore;

    public EventHandler ScoreUpdated;

    public enum FlagTeam {Nuetral, Red, Blue} // remember to add a field like this to player classes

    private void Update()
    {
        gameTimer += Time.deltaTime;
        if (gameTimer > scoreInterval)
        {
            AddScore();
            gameTimer = 0;
        }

        UpdateUI();
        CheckWin();

        
    }

    private void AddScore()
    {
        float redTeamFlags = 0;
        float blueTeamFlags = 0;

        foreach (MapArea flag in flagList)
        {
            if (flag.team == Team.Red) redTeamFlags++;
            else if (flag.team == Team.Blue) blueTeamFlags++;
        }

        if (redTeamFlags != 0)
        {
            RedTeamScore += redTeamFlags * scorePerFlag;
        }

        if (blueTeamFlags != 0)
        { 
        BlueTeamScore += blueTeamFlags * scorePerFlag;
        }

        ScoreUpdated?.Invoke(this, new EventArgs());

        Debug.Log($"red score {RedTeamScore}");
        Debug.Log($" blue score {BlueTeamScore}");

    }

    public float GetScoreRed()
    {

        return
            RedTeamScore;
    }

    public float GetScoreBlue()
    {
        return
           BlueTeamScore;
    }

    private void UpdateUI()
    {
        
        txtBlueTeamScore.SetText($"{BlueTeamScore}");
        txtRedTeamScore.SetText($"{RedTeamScore}"); 

    }

    private void CheckWin()
    {
        if (RedTeamScore >= targetScore)
        {
            SceneManager.LoadSceneAsync("GameOver");
        }
        else if (BlueTeamScore >= targetScore)
        {
            SceneManager.LoadSceneAsync("GameOver");
        }
    }


}
