using UnityEngine;
public enum Team { Nuetral, Red, Blue }
public class PlayerMapAreas : MonoBehaviour
{
    
    public Team team;

    public Team GetTeam()
    {
        return team;
    }
}
