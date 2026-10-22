using UnityEngine;

public class Snap : MonoBehaviour
{
    public GameObject piece;

    private CharacterController playcontroller;
    private Rigidbody rb;

    [Header("Target Reference")]
    // Put object with Goal script
    [SerializeField] private Goal goal;

    [Header("Target Location")]//Where the player should be
    [SerializeField] private float TargetPlayerx = 0;
    [SerializeField] private float TargetPlayerz = 0;

    [Header("Static Orientation")]//orientation of static object
    [SerializeField] private float FinalPosx = 0;
    [SerializeField] private float FinalPosy = 0;
    [SerializeField] private float FinalPosz = 0;
    [SerializeField] private float FinalRotx = 0;
    [SerializeField] private float FinalRoty = 0;
    [SerializeField] private float FinalRotz = 0;


    
    void Start()
    {
        playcontroller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    public void DeleteAllOtherScripts()
    {
        
        MonoBehaviour[] otherScripts = GetComponents<MonoBehaviour>();

        
        foreach (MonoBehaviour targetScript in otherScripts)
        {
            if (targetScript != this)
            {
                Destroy(targetScript);
            }
        }
    }

    public void MakeStatic()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Destroy(rb);
            Debug.Log("Test area 1");
        }

        if (playcontroller != null)
        {
            playcontroller.enabled = false;
            Debug.Log("Test area 2");
        }

        transform.position = new Vector3(FinalPosx, FinalPosy, FinalPosz);
        transform.rotation = Quaternion.Euler(FinalRotx, FinalRoty, FinalRotz);
        Debug.Log("Test area 3");

        if (goal != null)
        {
            goal.GoalCheck++;
            Debug.Log("Test area 4");
        }
        DeleteAllOtherScripts();
        Debug.Log("Test area 5");

        Debug.Log(gameObject.name + " is now static!");
        Debug.Log("Test area 6");
      
        Destroy(this);
        Debug.Log("Test area 7");
    }




    void Update()
    {
        float playerX = transform.position.x;
        float playerZ = transform.position.z;

        if ((playerX >= TargetPlayerx - 2 && playerX <= TargetPlayerx + 2) && (playerZ >= TargetPlayerz - 2 && playerZ <= TargetPlayerz + 2)) 
        {
            Debug.Log("Location found");
            MakeStatic();

        }
    }
}
