using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;
using System;
using System.Linq;

public class MapArea : MonoBehaviour
{

    //class that handles the capture area and capture state. Uses a set of 5 states to informFlag ownership, ConquestManager(scoring) and UI elements.
    //It uses a list of "key" type classes that are unique to each player object called PlayerMapAreas, which informs MapArea of the present player's team (Red/Blue) and uses the List's count to decide which state to flip to.
    //0 players present = nuetral, 1 player + red team = redcapturing -> CapturedByRed, 1p + blue team = blue capturing - > CapturedByBlue, 2 players present, contested, which isnt an actual state but probably should be.

    //One annoying bug I faced meant that flags were uncapturable if a player died on one while the other player wasnt also present. It took 2 days to fix and then it turned out to just be if list == 0 {capstate.nuetral

    public event EventHandler OnCaptured;
    public event EventHandler OnPlayerEnter;
    public event EventHandler OnPlayerExit;
    public Team team;
    public List<PlayerHealth> playerHealthList;
    public enum CapState
    {
        Nuetral,
        CapturedbyRed,
        RedCapturing,
        CapturedbyBlue,
        BlueCapturing

    }
    private List<CaptureAreaCollider> capAreaColliderList;
    public CapState capState;
    private float CapProgress;
    private PlayerMapAreas playerMapArea;
    public List<PlayerMapAreas> playerMapAreas;

    private void Awake()
    {
        capAreaColliderList = new List<CaptureAreaCollider>();

        foreach (Transform child in transform)
        {

            CaptureAreaCollider captureAreaCollider = child.GetComponent<CaptureAreaCollider>();
            if (captureAreaCollider != null)
            {
                capAreaColliderList.Add(captureAreaCollider);
                captureAreaCollider.OnPlayerEnter += CaptureAreaCollider_OnPlayerEnter;
                captureAreaCollider.OnPlayerExit += CaptureAreaCollider_OnPlayerExit;
            }

        }
        capState = CapState.Nuetral;


    }

    private void OnEnable()
    {
        foreach (PlayerHealth playerHealth in playerHealthList)
            playerHealth.OnPlayerDeath += PlayerHealth_OnDeath;
    }

    private void OnDisable()
    {
        foreach (PlayerHealth playerHealth in playerHealthList)
            playerHealth.OnPlayerDeath -= PlayerHealth_OnDeath;
    }

    private void PlayerHealth_OnDeath(GameObject source) //hoping this fixes the issue of flags not being recapturable if players die while on a captured flag
    {
        // Remove the dead player from all capture area colliders
        foreach (CaptureAreaCollider captureAreaCollider in capAreaColliderList)
        {
            captureAreaCollider.RemovePlayerOnDeath(source);
        }

        // Check if there are any remaining players in the capture area
        bool hasPlayerInside = false;
        foreach (CaptureAreaCollider captureAreaCollider in capAreaColliderList)
        {
            if (captureAreaCollider.GetPlayerMapAreasList().Count > 0)
            {
                hasPlayerInside = true;
                break;
            }
        }

        if (!hasPlayerInside)
        {
            OnPlayerExit?.Invoke(this, EventArgs.Empty);
        }
    }


    private void CaptureAreaCollider_OnPlayerExit(object sender, EventArgs e)
    {
        bool hasPlayerInside = false;   
        foreach (CaptureAreaCollider captureAreaCollider in capAreaColliderList)
        {
            if (captureAreaCollider.GetPlayerMapAreasList().Count > 0)
            {
                
                hasPlayerInside = true;
                
            }
        }

        if (!hasPlayerInside)
        {
            OnPlayerExit?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CaptureAreaCollider_OnPlayerEnter(object sender, EventArgs e)
    {
        OnPlayerEnter?.Invoke(this, EventArgs.Empty);
    }

    private void Update() //switches flag owner based on player's team using capture area collider
    {

        //This Update class is a bit of a nightmare, I'm sure there was a quicker and cleaner way to get the logic functioning properly as it does now, but some was built before adding two players, so it got more complicated with having to checkfor contests and ownsership states
        //The states here inform the UI elements which state to be in = the capture progress disc, the minimap area colouring, the message box

        //I spent a few days on this class alone, every fix brought out 2 or 3 new bugs
        //OLD COMMENT KEPT HERE ->//Must add flag recapturing, allow for flag to reset to nuetral. Flags and scoreboard fills cannot update while holding no flags. if a red flag has a blue player on it, it should become blue, stop being red, and only affect blue score.
        

        //structure: MapArea holds CaptureAreaColliders. CaptureAreaColliders PlayerMapAreas player component to detect collision and check the player's team. ConquestManager calls MapArea to AddScore() for either team depending on how many flags they have. XYZ point per flag every second. 
        //structure: It's less important but I would also like the MapAreasUI class to set the image color based on the owner of the flags, only works for blue for now, it was attempted earleir today and would just break when using a switch case and maparea.team.

        List<PlayerMapAreas> playerMapAreasInsideList = new List<PlayerMapAreas>();

        foreach (CaptureAreaCollider captureAreaCollider in capAreaColliderList)
        {
            foreach (PlayerMapAreas playerMapAreas in captureAreaCollider.GetPlayerMapAreasList())
            {
                if (!playerMapAreasInsideList.Contains(playerMapAreas))
                {
                    playerMapAreasInsideList.Add(playerMapAreas);
                }
            }
        }
        if(playerMapAreasInsideList.Count == 2 && team == Team.Nuetral)
        {
            capState = CapState.Nuetral;
        }

        if (playerMapAreasInsideList.Count == 0 && team == Team.Red) 
        {
            capState = CapState.CapturedbyRed;
        }
        if (playerMapAreasInsideList.Count == 0 && team == Team.Blue)
        {
            capState = CapState.CapturedbyBlue;
        }

        if (CapProgress < 0)
        {
            CapProgress = 0;
        }
        float progressSpeed = 0.25f;

        switch (capState)
        {
            case CapState.Nuetral:
                CapProgress += playerMapAreasInsideList.Count * progressSpeed * Time.deltaTime;

                // Determine the dominant team in the area, if both teams are present on the same point, the flag cap progress resets. 
                //my first solution was a LINQ query using .all, but both Neil in class & online sources advise against LINQ inside an Update() method for performance. 
                if (playerMapAreasInsideList.Count > 0)
                {
                    Team dominantTeam = playerMapAreasInsideList[0].GetTeam(); //checks each player's team off the playermapareas list
                    bool allSameTeam = true;

                    for (int i = 1; i < playerMapAreasInsideList.Count; i++) //for loop checks teams of players to satisfy the allSameTeam bool.
                    {
                        if (playerMapAreasInsideList[i].GetTeam() != dominantTeam)
                        {
                            allSameTeam = false;
                            break;
                        }
                    }

                    if (allSameTeam) //if all players are on the same team, the flag can be captured, otherwise it resets.
                    {
                        switch (dominantTeam)
                        {
                            case Team.Blue:
                                if (CapProgress >= 1f)
                                {
                                    capState = CapState.CapturedbyBlue;

                                    OnCaptured?.Invoke(this, EventArgs.Empty);

                                    Debug.Log("Area Captured by blue team");
                                }
                                else
                                {
                                    capState = CapState.BlueCapturing;
                                }
                                break;

                            case Team.Red:
                                if (CapProgress >= 1f)
                                {
                                    capState = CapState.CapturedbyRed;

                                    OnCaptured?.Invoke(this, EventArgs.Empty);

                                    Debug.Log("Area Captured by red team");
                                }
                                else
                                {
                                    capState = CapState.RedCapturing;
                                }
                                break;
                        }
                    }
                    else
                    {
                        // both teams present, tick down
                        CapProgress -= playerMapAreasInsideList.Count * progressSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    CapProgress = 0f;
                }

                break;

            case CapState.BlueCapturing:
                if (playerMapAreasInsideList.Count > 0)
                {
                    Team dominantTeam = playerMapAreasInsideList[0].GetTeam();
                    bool allSameTeam = true;

                    for (int i = 1; i < playerMapAreasInsideList.Count; i++)
                    {
                        if (playerMapAreasInsideList[i].GetTeam() != dominantTeam)
                        {
                            allSameTeam = false;
                            break;
                        }
                    }

                    if (allSameTeam && dominantTeam == Team.Blue)
                    {
                        CapProgress += playerMapAreasInsideList.Count * progressSpeed * Time.deltaTime;

                        if (CapProgress >= 1f)
                        {
                            capState = CapState.CapturedbyBlue;

                            OnCaptured?.Invoke(this, EventArgs.Empty);

                            Debug.Log("Area Captured by blue team");
                        }
                    }
                    else
                    {
                        CapProgress = 0f;
                    }
                }
                else
                {
                    CapProgress = 0f;
                }

                break;

            case CapState.RedCapturing:
                if (playerMapAreasInsideList.Count > 0)
                {
                    Team dominantTeam = playerMapAreasInsideList[0].GetTeam();
                    bool allSameTeam = true;

                    for (int i = 1; i < playerMapAreasInsideList.Count; i++)
                    {
                        if (playerMapAreasInsideList[i].GetTeam() != dominantTeam)
                        {
                            allSameTeam = false;
                            break;
                        }
                    }


                    if (allSameTeam && dominantTeam == Team.Red)
                    {
                        CapProgress += playerMapAreasInsideList.Count * progressSpeed * Time.deltaTime;

                        if (CapProgress >= 1f)
                        {
                            capState = CapState.CapturedbyRed;

                            OnCaptured?.Invoke(this, EventArgs.Empty);

                            Debug.Log("Area Captured by red team");
                        }
                    }
                    else
                    {
                        CapProgress = 0f;
                    }
                }
                else
                {
                    CapProgress = 0f;
                }

                break;

            case CapState.CapturedbyBlue:
                //repeating the nuetral flag logic
                playerMapAreasInsideList = new List<PlayerMapAreas>();


                foreach (CaptureAreaCollider captureAreaCollider in capAreaColliderList)
                {
                    foreach (PlayerMapAreas playerMapAreas in captureAreaCollider.GetPlayerMapAreasList())
                    {
                        if (!playerMapAreasInsideList.Contains(playerMapAreas))
                        {
                            playerMapAreasInsideList.Add(playerMapAreas);
                        }
                    }
                } //collider logic to ocunt players

                if(playerMapAreasInsideList.Count > 0)
                {
                    Team dominantTeam = playerMapAreasInsideList[0].GetTeam();
                    bool allSameTeam = true;
                    bool redTeamPresent = false;

                    for (int i = 1; i < playerMapAreasInsideList.Count; i++)
                    {
                        if (playerMapAreasInsideList[i].GetTeam() != dominantTeam)
                        {
                            allSameTeam = false;
                        }
                        if (playerMapAreasInsideList[i].GetTeam() == Team.Red)
                        {
                            redTeamPresent = true;
                        }       
                    }

                    if (dominantTeam == Team.Red)
                    {
                        redTeamPresent = true;
                    }

                    if (redTeamPresent && allSameTeam && dominantTeam == Team.Red)
                    {
                        CapProgress -= progressSpeed * Time.deltaTime; //counts capture down and then sets owner to red
                        if (CapProgress <= 0f)
                        {
                            capState = CapState.RedCapturing; //ssetting this to skip neutral, I dont want players to have to wait long on a flag in a 2player game
                            CapProgress = 0f;
                            OnCaptured?.Invoke(this, EventArgs.Empty);
                            Debug.Log($"{gameObject.name} Captured by red team");
                        }
                    }
                    else
                    {
                        CapProgress = 1f; //keeps capture if  teams are mixed or point is empty
                    }
                }
                else
                {
                    CapProgress = 1f;
                }

                FlagOwnership();
                break;

            case CapState.CapturedbyRed:
                //repeating the nuetral flag logic
                playerMapAreasInsideList = new List<PlayerMapAreas>();


                foreach (CaptureAreaCollider captureAreaCollider in capAreaColliderList)
                {
                    foreach (PlayerMapAreas playerMapAreas in captureAreaCollider.GetPlayerMapAreasList())
                    {
                        if (!playerMapAreasInsideList.Contains(playerMapAreas))
                        {
                            playerMapAreasInsideList.Add(playerMapAreas);
                        }
                    }
                }

                if (playerMapAreasInsideList.Count > 0)
                {
                    Team dominantTeam = playerMapAreasInsideList[0].GetTeam();
                    bool allSameTeam = true;
                    bool BlueTeamPresent = false;

                    for (int i = 0; i < playerMapAreasInsideList.Count; i++)
                    {
                        if (playerMapAreasInsideList[i].GetTeam() != dominantTeam)
                        {
                            allSameTeam = false;
                        }
                        if (playerMapAreasInsideList[i].GetTeam() == Team.Blue)
                        {
                            BlueTeamPresent = true;
                        }
                    }

                    if (dominantTeam == Team.Blue)
                    {
                        BlueTeamPresent = true;
                    }

                    if (BlueTeamPresent && allSameTeam && dominantTeam == Team.Blue)
                    {
                        CapProgress -= progressSpeed * Time.deltaTime;
                        if (CapProgress <= 0f)
                        {
                            capState = CapState.BlueCapturing; //ssetting this to skip neutral, I dont want players to have to wait long on a flag in a 2player game
                            CapProgress = 0f;
                            OnCaptured?.Invoke(this, EventArgs.Empty);
                            Debug.Log($"{gameObject.name} Captured by Blue team");
                        }
                    }
                    else
                    {
                        CapProgress = 1f; //keeps capture if  teams are mixed or point is empty
                    }
                }
                else
                {
                    CapProgress = 1f;
                }

                FlagOwnership();
                break;
        }
    }


    public Team FlagOwnership()
    {
        if (capState == CapState.CapturedbyBlue)
        {
            team = Team.Blue;   
           gameObject.GetComponentInChildren<CaptureAreaCollider>().team = Team.Blue;
        }
        else if (capState == CapState.CapturedbyRed)
        {
            team = Team.Red;
            gameObject.GetComponentInChildren<CaptureAreaCollider>().team = Team.Red;
        }
        return team;
    }

    public void MapArea_OnCaptured()
    {
        gameObject.GetComponentInChildren<CaptureAreaCollider>().team = playerMapArea.GetTeam();
    }
    public float GetCapProgress()
    {
        return CapProgress;
    }

    public CapState GetCapState()
    {
        return capState;
    }

    

}

