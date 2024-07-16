using UnityEngine;

public class HitTarget : MonoBehaviour
{
    public LayerMask hitMask;

    public bool PowerbarDetected()
    {
        if (Physics2D.BoxCast(transform.position, new Vector2(0.01f, 1f), 0f, Vector2.up, 0f, hitMask))
            return true;
        else
            return false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (Physics2D.BoxCast(transform.position, new Vector2(0.01f, 1f), 0f, Vector2.up, 0f, hitMask))
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireCube(transform.position,new Vector3(0.01f,1f,0f));
    }
}
