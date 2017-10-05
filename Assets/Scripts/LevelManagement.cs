using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManagement : MonoBehaviour {

    public GameObject Spawner;
    public GameObject DeathMachineMK1;
    public GameObject DeathMachineMK2;
    public GameObject DeathMachineMK3;
    private float timeFloat;
    private bool phase1 = true;
    private bool phase2 = true;
    private bool phase3 = true;
    private bool phase4 = true;

    float counter = 0;
    float counter2 = 0;


    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        timeFloat = GameObject.Find("HUD").GetComponent<Timer>().timeFloat;

        // Press R to restart
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Level progression
        if (phase1 && timeFloat >= 10.0f)
        {
            GameObject wokeSpawner = Instantiate(Spawner, transform.position + new Vector3(40,6,0), transform.rotation) as GameObject;
            GameObject wokeSpawner2 = Instantiate(Spawner, transform.position + new Vector3(-40,6,0), transform.rotation) as GameObject;
            phase1 = false;
        }
        if (phase2 && timeFloat >= 20.0f)
        {
            GameObject wokeDeathMachine = Instantiate(DeathMachineMK1, transform.position, transform.rotation * Quaternion.Euler(180, 0, 0)) as GameObject;
            phase2 = false;
        }

        if (timeFloat >= 35.0f && phase4) {
            counter += 1 * Time.deltaTime;
            if (counter >= 8) { 
                GameObject wokeSpawner = Instantiate(Spawner, transform.position + new Vector3(40, 6, 0), transform.rotation) as GameObject;
                GameObject wokeSpawner1 = Instantiate(Spawner, transform.position + new Vector3(-40, 6, 0), transform.rotation) as GameObject;
                GameObject wokeSpawner2 = Instantiate(Spawner, transform.position + new Vector3(0, 6, 40), transform.rotation) as GameObject;
                GameObject wokeSpawner3 = Instantiate(Spawner, transform.position + new Vector3(0, 6, -40), transform.rotation) as GameObject;
                counter = 0;
            }
            if (phase3)
            {
                GameObject wokeDeathMachine = Instantiate(DeathMachineMK1, transform.position, transform.rotation * Quaternion.Euler(180, 0, 0)) as GameObject;
                Vector3 randomPos = new Vector3(Random.Range(-40, 40), 6, Random.Range(-40, 40));
                GameObject wokeSpawner = Instantiate(Spawner, transform.position + randomPos, transform.rotation) as GameObject;
                phase3 = false;
            }
        }

        if (phase4 && timeFloat >= 50.0f)
        {
            GameObject wokeDeathMachine = Instantiate(DeathMachineMK2, transform.position, transform.rotation) as GameObject;
            GameObject wokeSpawner = Instantiate(Spawner, transform.position + new Vector3(0, 6, 40), transform.rotation) as GameObject;
            GameObject wokeSpawner1 = Instantiate(Spawner, transform.position + new Vector3(0, 6, -40), transform.rotation) as GameObject;
            phase4 = false;
        }

        //Current endgame
        if (timeFloat >= 55.0f)
        {
            counter +=  1 * Time.deltaTime;
            if (counter >= 3)
            {
                Vector3 randomPos = new Vector3(Random.Range(-40, 40), 6, Random.Range(-40, 40));
                GameObject wokeSpawner = Instantiate(Spawner, transform.position + randomPos, transform.rotation) as GameObject;
                counter = 0;
            }
            counter2 += 1 * Time.deltaTime;
            if (counter2 >= 10)
            {   
                GameObject wokeDeathMachine = Instantiate(DeathMachineMK2, transform.position, transform.rotation) as GameObject;
                counter2 = 0;
            }
        }
    }
}
