using UnityEngine;

public class EnterZone : MonoBehaviour
{
    public bool inZone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inZone = false;
    }

    void OnTriggerEnter(Collider other)
    {
        inZone = true;
    }
}
