using UnityEngine;

public class TargetTerrain : MonoBehaviour
{

    public TerrainData tData;
    public GameObject currentDetonatorTerrain;
    public float explosionLife = 10;


    [Header("Terrain Destrucion")]
    [Tooltip("Height of terrain destruction")]
    [Range(0f, 10f)]
    public float terrainDestructHeight = 0.20f;
    [Tooltip("Ramdom explosion particles system")]
    [Range(0f, 1f)]
    public float ramdomExplosion = 1.0f;

    //public float shootDistanceToDestroy = 500.0f;
    [Tooltip("Ramdom explosion particles system")]
    public Texture2D brush;
    [SerializeField] private Texture2D[] Listbrush;


    [Tooltip("offsetX")]
    [Range(-20, 20)]
    public int offsetx = 1;


    [Tooltip("offsetY")]
    [Range(-20, 20)]
    public int offsety = 1;

    public bool type;


    public AudioClip clip;

    void Start()
    {


    }

    void textureFix()
    {
        Color32[] pixelBlock = null;
        try
        {
            pixelBlock = brush.GetPixels32();
        }
        catch (UnityException _e)
        {
            brush.filterMode = FilterMode.Point;
            RenderTexture rt = RenderTexture.GetTemporary(brush.width, brush.height);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(brush, rt);
            Texture2D img2 = new Texture2D(brush.width, brush.height);
            img2.ReadPixels(new Rect(0, 0, brush.width, brush.height), 0, 0);
            img2.Apply();
            RenderTexture.active = null;
            brush = img2;
            pixelBlock = brush.GetPixels32();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "kamehameha")
        {



            destroyTerrain(collision, type);
            detonationTerrain(collision);
            Destroy(collision.gameObject);
        }

    }



    void destroyTerrain(Collider collision, bool type)
    {


        float normalizedValue = Mathf.InverseLerp(0, 100, (int)collision.GetComponent<KameHameHa>().Size);
        int brushSize = (int) Mathf.Lerp(0, Listbrush.Length-1, normalizedValue);

        brush = Listbrush[brushSize];
        textureFix();

        Terrain terr = GetComponent<Terrain>();
        // get the normalized position of this game object relative to the terrain
        Vector3 tempCoord = (collision.transform.position - terr.gameObject.transform.position);
        Vector3 coord;

        int hmWidth = terr.terrainData.heightmapResolution;
        int hmHeight = terr.terrainData.heightmapResolution;



        //int hmWidth = Mathf.RoundToInt(collision.GetComponent<KameHameHa>().Size)*10; 
        //    int hmHeight = Mathf.RoundToInt(collision.GetComponent<KameHameHa>().Size)*10;
        //float ExplosionVelocity = collision.GetComponent<KameHameHa>().Velocity;

        float normalizedValueVelocity = Mathf.InverseLerp(0, 10, (int)collision.GetComponent<KameHameHa>().Velocity);
        float ExplosionVelocity = Mathf.Lerp(terrainDestructHeight,0.1f , normalizedValueVelocity);


        Debug.Log($"ExplosionVelocity {ExplosionVelocity}");

        coord.x = tempCoord.x / terr.terrainData.size.x;
        coord.y = tempCoord.y / terr.terrainData.size.y;
        coord.z = tempCoord.z / terr.terrainData.size.z;

        int size = brush.width;
        int offset = Mathf.RoundToInt( size / 2);

        int x = (int)(coord.x * hmWidth) - offset;
        int y = (int)(coord.z * hmHeight) - offset ;

       // int x = (int)(coord.x * hmWidth) + offsetx;
     //   int y = (int)(coord.z * hmHeight) + offsety;


        float[,] areaT;
        try
        {
            areaT = tData.GetHeights(x, y, size, size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Color texPixel = brush.GetPixel(i, j);
                    if (type)
                    {
                        areaT[i, j] += texPixel.grayscale *   ExplosionVelocity ;
                    }
                    else
                    {
                        areaT[i, j] -= texPixel.grayscale *   ExplosionVelocity ;
                    }

                    //areaT[i, j] = 0;
                }
            }
            tData.SetHeights(x, y, areaT);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message.ToString());
        }

    }


    void detonationTerrain(Collider collision)
    {

        Destroy(Instantiate(currentDetonatorTerrain, collision.transform.position, Quaternion.identity), explosionLife);



        float normalizedValue = Mathf.InverseLerp(0, 100, (int)collision.GetComponent<KameHameHa>().Size);
        int explosionSize = (int)Mathf.Lerp(0, 10, normalizedValue);


        //float ExplosionVelocity = collision.GetComponent<KameHameHa>().Velocity;
       // Debug.Log("Size" + collision.GetComponent<KameHameHa>().Size + " -- " + explosionSize + " -- " + ExplosionVelocity);

        for (int i = 0; i < explosionSize; i++)
        {
            Destroy(Instantiate(currentDetonatorTerrain, Utils.RandomNearPosition(collision.transform, ramdomExplosion, 0f, ramdomExplosion, true).position, Quaternion.identity), explosionLife);
        }
        AudioSource.PlayClipAtPoint(clip, collision.transform.position);

    }



    }

