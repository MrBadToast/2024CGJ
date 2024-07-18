using UnityEngine;

public class HitTarget : MonoBehaviour
{
    public LayerMask hitMask;
    public float tollerance;

    public bool PowerbarDetected()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.01f, 1f), 0f, Vector2.up, 0f, hitMask);
        if (hit)
        {
            if (Mathf.Abs(hit.rigidbody.transform.localPosition.x) < tollerance)
            {
                Destroy(hit.rigidbody.gameObject);
                return true;
            }
            else
            {
                Destroy(hit.rigidbody.gameObject);
                return false;
            }
        }
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
