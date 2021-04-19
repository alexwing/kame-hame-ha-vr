/**
 * Reset terrain from TerrainData Backup
 */

using UnityEngine;

public class TerrainReset : MonoBehaviour {
	
	public Terrain Terrain;
    public TerrainData terrainData;

    private float[,] originalHeights;
	
	private void OnDestroy()
	{
		this.Terrain.terrainData.SetHeights(0, 0, this.originalHeights);
	}
	
	private void Start()
	{
		this.originalHeights = terrainData.GetHeights(
			0, 0, Terrain.terrainData.heightmapResolution, Terrain.terrainData.heightmapResolution);

        this.Terrain.terrainData.SetHeights(0, 0, this.originalHeights);
    }
}
