using UnityEngine;
using System.Collections;

public class TerrainShootDetect : MonoBehaviour
{

    public TerrainData tData;
    public GameObject currentDetonatorTerrain;
    public float explosionLife = 10;
    public float detailLevel = 1.0f;

    [Header("Terrain Destrucion")]
    [Tooltip("Size of terrain destruction")]
    [Range(0f,10f)]
    public int terrainDestructSize = 3;
    [Tooltip("Height of terrain destruction")]
    [Range(0f, 100)]
    public int terrainDestructHeight = 10;
    //public float shootDistanceToDestroy = 500.0f;
    public Texture2D brush;

    public bool type;


    public AudioClip clip;

    void Start()
    {
        textureFix();

        //  Destroy(gameObject, lifetime);
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

    void Update()
    {



        //	Debug.Log("shootDistanceToDestroy "+ shootDistanceToDestroy +" x " +  transform.position.x );
        //	Debug.ClearDeveloperConsole ();
        /*
            if(transform.position.x > shootDistanceToDestroy || transform.position.x < -shootDistanceToDestroy 
               || transform.position.z > shootDistanceToDestroy || transform.position.z < -shootDistanceToDestroy
               || transform.position.y > shootDistanceToDestroy || transform.position.y < -shootDistanceToDestroy)
            if (transform.position.x > shootDistanceToDestroy )
            {
                Destroy(this.gameObject);
            }*/
    }

    void OnTriggerEnter(Collider collision)
    {


        Debug.Log(collision.gameObject.name);
        /*
       if (collision.gameObject.name != "Nave" && collision.gameObject.name != "Terrain")
       {
           Physics.IgnoreCollision(this.GetComponent<Collider>(), collision);
       }
       if (collision.gameObject.name != "OVRPlayerController" && collision.gameObject.name != "R Cube" && collision.gameObject.name != "L Cube") {

            //if (collision.gameObject.transform.parent.gameObject.name != "limit") {
            detonation();
            //}
            //Debug.Log("detonacion a " + collision.gameObject.transform.parent.gameObject.name);
            Destroy(this.gameObject);

        }
        if (collision.gameObject.name == "Barril") {


            //Object clone;
            //clone = Instantiate(collision.gameObject, collision.gameObject.transform.position, collision.gameObject.transform.rotation) as Object;
            ////clone.velocity = transform.TransformDirection(Vector3.forward * 100);
            if(collision.gameObject.GetComponent<Rigidbody>()){
                collision.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward  *1100);
            }else{
                Destroy(collision.gameObject);
            }

        }*/
        Debug.Log("colision " + collision.gameObject.name);
        if (collision.gameObject.name == "kamehameha")
        {
            destroyTerrain(collision, type);
            detonationTerrain(collision);
            Destroy(collision);
        }

    }





    void destroyTerrain(Collider collision, bool type)
    {

        Terrain terr = GetComponent<Terrain>();
        // get the normalized position of this game object relative to the terrain
        Vector3 tempCoord = (collision.transform.position - terr.gameObject.transform.position);
        Vector3 coord;

        int hmWidth = terr.terrainData.heightmapResolution;
        int hmHeight = terr.terrainData.heightmapResolution;

        coord.x = tempCoord.x / terr.terrainData.size.x;
        coord.y = tempCoord.y / terr.terrainData.size.y;
        coord.z = tempCoord.z / terr.terrainData.size.z;

        int size = brush.width;
        int offset = size / 2;

        int x = (int)(coord.x * hmWidth) - offset;
        int y = (int)(coord.z * hmHeight) + offset;


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
                        areaT[i, j] += texPixel.grayscale *  terrainDestructHeight /100;
                    }
                    else
                    {
                        areaT[i, j] -= texPixel.grayscale *  terrainDestructHeight /100;
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

        //  Component dTemp = currentDetonatorTerrain.GetComponent("Detonator");

        //float offsetSize = dTemp.size/3;

        GameObject exp = (GameObject)Instantiate(collision.gameObject, collision.transform.position, Quaternion.identity);
        GameObject exp2 = (GameObject)Instantiate(currentDetonatorTerrain, collision.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(clip, collision.transform.position);
        // dTemp = exp.GetComponent("Detonator");
        //dTemp.detail = detailLevel;

        Destroy(exp, explosionLife/3);
        Destroy(exp2, explosionLife);




    }
    /*   void detonation (){

           //Component dTemp = currentDetonator.GetComponent("Detonator");

           //float offsetSize = dTemp.size/3;

           GameObject exp = (GameObject) Instantiate(currentDetonator, transform.position, Quaternion.identity);
           AudioSource.PlayClipAtPoint(clip, transform.position);
           //dTemp = exp.GetComponent("Detonator");
           //dTemp.detail = detailLevel;

           Destroy(exp, explosionLife);



       }*/
}
