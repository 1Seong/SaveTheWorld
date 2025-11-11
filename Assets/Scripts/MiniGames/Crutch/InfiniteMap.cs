using UnityEngine;

public class InfiniteMap : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Surface"))
        {
            collision.transform.parent.transform.Translate(new Vector3(19.1f, 0f, 0f));
        }
    }
}
