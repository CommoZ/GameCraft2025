using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class PlatformLight2D : MonoBehaviour
{
    // =========================================================================
    // LIGHT SETTINGS
    // =========================================================================
    [Header("Light Settings")]
    [SerializeField] private Light2D platformLight;
    [SerializeField] private float litIntensity = 1.2f;
    [SerializeField] private float unlitIntensity = 0f;
    [SerializeField] private float fadeSpeed = 6f;

    // =========================================================================
    // TILEMAP SETTINGS
    // =========================================================================
    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileSwap[] tileSwaps;
    [SerializeField] private int tileRadius = 10; // radius in tiles

    private float targetIntensity;

    // =========================================================================
    // UNITY LIFECYCLE
    // =========================================================================
    void Awake()
    {
        if (platformLight == null)
            platformLight = GetComponentInChildren<Light2D>();

        if (tilemap == null)
            tilemap = GetComponent<Tilemap>();

        platformLight.intensity = unlitIntensity;
        targetIntensity = unlitIntensity;
    }

    void Update()
    {
        platformLight.intensity = Mathf.Lerp(
            platformLight.intensity,
            targetIntensity,
            fadeSpeed * Time.deltaTime
        );
    }

    // =========================================================================
    // COLLISION
    // =========================================================================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("PlayerWhole"))
            return;

        targetIntensity = litIntensity;

        Vector3Int centerCell = GetPlayerFeetCell(collision.transform);
        SwapTilesInRadius(centerCell);
    }

    // =========================================================================
    // TILE LOGIC
    // =========================================================================
    void SwapTilesInRadius(Vector3Int centerCell)
    {
        for (int x = -tileRadius; x <= tileRadius; x++)
        {
            for (int y = -tileRadius; y <= tileRadius; y++)
            {
                // Circular radius check
                if (x * x + y * y > tileRadius * tileRadius)
                    continue;

                Vector3Int cellPos = new Vector3Int(
                    centerCell.x + x,
                    centerCell.y + y,
                    centerCell.z
                );

                Sprite currentSprite = tilemap.GetSprite(cellPos);
                if (currentSprite == null)
                    continue;

                TrySwapTile(cellPos, currentSprite);
            }
        }
    }

    void TrySwapTile(Vector3Int cellPos, Sprite currentSprite)
    {
        foreach (var swap in tileSwaps)
        {
            if (currentSprite == swap.unlitSprite)
            {
                tilemap.SetTile(cellPos, swap.litTile);
                return;
            }
        }
    }

    // =========================================================================
    // HELPERS
    // =========================================================================
    Vector3Int GetPlayerFeetCell(Transform player)
    {
        Vector3 feetPos = player.position + Vector3.down * 0.5f;
        return tilemap.WorldToCell(feetPos);
    }
}

// =========================================================================
// DATA STRUCTS
// =========================================================================
[System.Serializable]
public struct TileSwap
{
    public Sprite unlitSprite;
    public TileBase litTile;
}
