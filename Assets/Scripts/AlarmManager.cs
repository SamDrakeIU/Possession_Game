using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
{
    private float alarmLevel;

    public PossessScript player;
    public Slider awarenessMeter;
    public int possessedAlarmRate;
    public int ghostAlarmRate;
    public EnemyAI[] enemies;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alarmLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int alarmRate = 1;
        bool aware = false;
        foreach (EnemyAI enemy in enemies)
        {
            if(enemy.currentState == EnemyAI.AIState.Chase)
            {
                aware = true;
            }
        }
        if(player.possessing)
        {
            alarmRate = possessedAlarmRate;
        }
        else
        {
            alarmRate = ghostAlarmRate;
        }
        if(aware)
        {
            alarmLevel += alarmRate * Time.deltaTime;
        }
        else if(alarmLevel > 0)
        {
            alarmLevel -= 3 * Time.deltaTime;
        }
        awarenessMeter.value = Mathf.Clamp(alarmLevel, 0, 50);
        if(alarmLevel > 50)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
