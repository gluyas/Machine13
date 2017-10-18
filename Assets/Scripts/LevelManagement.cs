using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManagement : MonoBehaviour {

    public GameObject Spawner;
    public GameObject DeathMachineMK1;
    public GameObject DeathMachineMK2;
    public GameObject DeathMachineMK3;
    
    public float SpawnerSpawnRadius = 20;
    public float SpawnerSpawnBaseHeight = -10;
    
    private float _timeFloat;
    private bool _phase1 = true;
    private bool _phase2 = true;
    private bool _phase3 = true;
    private bool _phase4 = true;
    

    float _counter = 0;
    float _counter2 = 0;



    // Use this for initialization
    void Start () {
        MakeSpawner();
    }
	
    
	// Update is called once per frame
	void Update () {

        _timeFloat = GameObject.Find("HUD").GetComponent<Timer>().timeFloat;
	    
        // Press R to restart
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Level progression
        if (_phase1 && _timeFloat >= 10.0f)
        {
            MakeSpawner(); MakeSpawner();
            _phase1 = false;
        }
        if (_phase2 && _timeFloat >= 20.0f)
        {
            GameObject wokeDeathMachine = Instantiate(DeathMachineMK1, transform.position, transform.rotation * Quaternion.Euler(180, 0, 0)) as GameObject;
            _phase2 = false;
        }

        if (_timeFloat >= 35.0f && _phase4) {
            _counter += 1 * Time.deltaTime;
            if (_counter >= 8) { 
                GameObject wokeSpawner = Instantiate(Spawner, transform.position + new Vector3(40, 6, 0), transform.rotation) as GameObject;
                MakeSpawner(); MakeSpawner(); MakeSpawner();
                _counter = 0;
            }
            if (_phase3)
            {
                GameObject wokeDeathMachine = Instantiate(DeathMachineMK1, transform.position, transform.rotation * Quaternion.Euler(180, 0, 0)) as GameObject;
                Vector3 randomPos = new Vector3(Random.Range(-40, 40), 6, Random.Range(-40, 40));
                MakeSpawner();
                _phase3 = false;
            }
        }

        if (_phase4 && _timeFloat >= 50.0f)
        {
            GameObject wokeDeathMachine = Instantiate(DeathMachineMK2, transform.position, transform.rotation) as GameObject;
            MakeSpawner(); MakeSpawner();
            _phase4 = false;
        }

        //Current endgame
        if (_timeFloat >= 60.0f)
        {
            _counter +=  1 * Time.deltaTime;
            if (_counter >= 3)
            {
                Vector3 randomPos = new Vector3(Random.Range(-40, 40), 6, Random.Range(-40, 40));
                MakeSpawner();
                _counter = 0;
            }
            _counter2 += 1 * Time.deltaTime;
            if (_counter2 >= 10)
            {   
                GameObject wokeDeathMachine = Instantiate(DeathMachineMK2, transform.position, transform.rotation) as GameObject;
                _counter2 = 0;
            }
        }
    }

    private void MakeSpawner()
    {
        var pos = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Vector3.forward * SpawnerSpawnRadius;
        pos.y = SpawnerSpawnBaseHeight;
        Instantiate(Spawner).GetComponent<Spawner>().Init(pos);
    }
}
