using UnityEngine;

public class DiceDisplay : MonoBehaviour
{
    [SerializeField] public string diceName;
    public float rotationSpeed = 60f;

    void Update()
    {
        // Rotate the object around the Y-axis continuously
        if (diceName == "D8")
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        else
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
