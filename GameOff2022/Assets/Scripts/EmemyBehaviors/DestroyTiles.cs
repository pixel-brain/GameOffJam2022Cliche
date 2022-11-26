using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTiles : MonoBehaviour
{

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("DestructableTiles"))
        {
            Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.1f;
                hitPosition.y = hit.point.y - 0.1f;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
        }
    }
}
