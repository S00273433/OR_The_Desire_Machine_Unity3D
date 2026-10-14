using UnityEngine;

public class PowerupBox : MonoBehaviour
{
    [SerializeField] private Transform BoxTransform;
    [SerializeField] private float RotationSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        BoxTransform.Rotate(Vector3.left, RotationSpeed * Time.deltaTime);
        BoxTransform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }
}
