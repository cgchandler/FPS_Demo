using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;
    private GameObject[] _enemy;
    private Vector3[] _spawnVector;
    private int _level = 0;
    private int _maxLevel = 100;
    private int _liveEnemyCount = 0;

    public int Level
    {
        get { return _level; }
    }

    public int LiveEnemyCount
    {
        get { return _liveEnemyCount; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Will Use One Enemy per Level
        _enemy = new GameObject[_maxLevel];

        // Current number of live enemies
        _liveEnemyCount = _level;

        // Define 5 Different Spawn Points for Enemies
        _spawnVector = new Vector3[5] {
            new Vector3(0.0f, 1.5f, 10.0f),
            new Vector3(-10.0f, 1.5f, 10.0f),
            new Vector3(-20.0f, 1.5f, 10.0f),
            new Vector3(10.0f, 1.5f, 10.0f),
            new Vector3(20.0f, 1.5f, 10.0f)
            };

    }

    // Update is called once per frame
    void Update()
    {
        bool levelCompleted = true;
        int enemies = 0;
        for (int index = 0; index < _level; ++index)
        {
            if (_enemy[index] != null)
            {
                enemies++;
                levelCompleted = false;
                //break; Can't break early because we need to count all of the live enemies
            }
        }

        if (levelCompleted)
        {
            if (_level < _maxLevel)
                ++_level;

            Debug.Log(string.Format("Starting Level {0}", _level));

            enemies = _level;

            for (int index = 0; index < _level; ++index)
            {
                _enemy[index] = Instantiate(enemyPrefab) as GameObject;

                _enemy[index].transform.position = _spawnVector[Random.Range(0, 4)];
                Debug.Log(string.Format(
                    "Spawn Postion ({0}, {1}, {2})",
                    _enemy[index].transform.position.x,
                    _enemy[index].transform.position.y,
                    _enemy[index].transform.position.z)
                );
                float angle = Random.Range(0, 360);
                _enemy[index].transform.Rotate(0, angle, 0);
            }
        }

        _liveEnemyCount = enemies;  // update the current number of enemies

    }
}
