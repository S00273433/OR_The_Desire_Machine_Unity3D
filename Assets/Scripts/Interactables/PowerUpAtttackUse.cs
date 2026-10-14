using UnityEngine;

public class PowerUpAttackUse : MonoBehaviour
{
    [SerializeField] private PlayerAttack PlayerAttackRed;
    [SerializeField] private PlayerAttack PlayerAttackBlue;
    [SerializeField] private AttackUp attackUp;
    [SerializeField] private float RespawnTime = 45f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerOne")) //I use p1 and p2 tags to assign the object to the correct player. It should have been fine with Player but it seemed to work better this way
        {
            PlayerAttackRed.SetAttackUp(true);
            ToggleActive();
        }
        if (other.CompareTag("PlayerTwo"))
        {
            PlayerAttackBlue.SetAttackUp(true);
            ToggleActive();
        }
    }

    private void ToggleActive()  //turns mesh and collider on/off
    {
        Collider c = GetComponent<Collider>();
        c.enabled = !c.enabled;
        
        if (!c.enabled)
        Invoke("ToggleActive", RespawnTime);

        MeshRenderer[] m = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in m)
        {
            r.enabled = !r.enabled;
        }
    }


}