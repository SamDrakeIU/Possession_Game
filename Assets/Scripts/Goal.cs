using UnityEngine;

public class Goal : MonoBehaviour
{
    public int GoalCheck = 0;//Variable for clear condition

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (GoalCheck >= 5)
        {
            objectRenderer.material.color = Color.red;
        }
    }
}
