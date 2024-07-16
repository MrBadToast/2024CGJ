using UnityEngine;

public class PowerBar : StaticSerializedMonoBehaviour<PowerBar>
{
    public GameObject powerBarPrefab;
    public Transform powerBarTFParent;
    public HitTarget hitTarget;
    public float powerBarSpeed = 1.0f;
    public float powerBarSpawnpoint = 70f;

    public bool DebugSpawn = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (DebugSpawn)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("BarSpawnedByDebug");
                SpawnBar();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(hitTarget.PowerbarDetected())
                    Debug.Log(" O CorrectHit");
                else
                    Debug.Log(" X Missed");
            }
        }
    }

    public void SpawnBar()
    {
        GameObject spawned = Instantiate(powerBarPrefab, powerBarTFParent);
        spawned.transform.localPosition = new Vector3(powerBarSpawnpoint, 0f, 0f);
    }


}
