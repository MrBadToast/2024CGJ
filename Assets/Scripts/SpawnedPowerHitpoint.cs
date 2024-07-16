using UnityEngine;

public class SpawnedPowerHitpoint : MonoBehaviour
{
    public float speed = 5f;
    public float destroyPoint = -70f;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * speed * Time.fixedDeltaTime);

        if(transform.localPosition.x < destroyPoint)
            Destroy(gameObject);
    }
}
