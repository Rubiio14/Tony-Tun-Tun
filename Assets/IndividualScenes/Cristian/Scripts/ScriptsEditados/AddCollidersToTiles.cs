using UnityEngine;
using UnityEngine.Tilemaps;

public class AddCollidersToTiles : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (!tilemap.HasTile(localPlace)) continue;

            var tile = tilemap.GetTile(localPlace);
            var tileGameObject = tilemap.GetInstantiatedObject(localPlace);

            if (tileGameObject != null && tileGameObject.GetComponent<Collider2D>() == null)
            {
                tileGameObject.AddComponent<BoxCollider2D>();
            }
        }
    }
}

