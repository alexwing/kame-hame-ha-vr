using UnityEngine;
using System.Collections;

public class terrainReset : MonoBehaviour {

	
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
			0, 0, Terrain.terrainData.heightmapWidth, Terrain.terrainData.heightmapHeight);

        this.Terrain.terrainData.SetHeights(0, 0, this.originalHeights);
    }
}
