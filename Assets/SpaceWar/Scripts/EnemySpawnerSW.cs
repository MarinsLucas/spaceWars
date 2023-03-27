using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerSW : MonoBehaviour
{
    public Transform[] positions;
    public SWWaveData[] data;

    [Header("Prefabs")]
    [SerializeField] GameObject kamikasePrefab;
    [SerializeField] GameObject normalPrefab;

    byte qKamikase;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.childCount==0)
        {
            //Debug.Log(Random.Range(0, data.Length-1));
            SpawnWave(data[Random.Range((int)0, data.Length-1)]);
        }
    }

    void SpawnWave(SWWaveData info)
    {
        qKamikase = info.quantKamikase;
        for(byte line = 0; line < info.quantLines; line++)
        {
            for(byte i =0; i<positions.Length;i++)
            {
                int percentage = Random.Range(0, 100);
                if(percentage>50 && qKamikase > 0)
                {
                    GameObject kamikase = Instantiate(kamikasePrefab, transform);
                    kamikase.transform.position = positions[i].transform.position + new Vector3(0f, 2*line, 0f);
                    qKamikase--; 
                }
                else
                {
                    GameObject normal = Instantiate(normalPrefab, transform);
                    normal.transform.position =positions[i].transform.position + new Vector3(0f, 2*line, 0f);
                }
            }
        }
        
    }
}
