using UnityEngine;
using System.Collections;

public class shootDetect : MonoBehaviour {
	
	public TerrainData tData ;
	public GameObject currentDetonator;
    public GameObject currentDetonatorTerrain;
    public float explosionLife = 10;
    public float lifetime = 10;
    public float detailLevel = 1.0f;
	public int terrainDestructSize = 3;
	public float terrainDestructHeight = 3;
    //public float shootDistanceToDestroy = 500.0f;
    public Texture2D brush;

    public bool type;


    public AudioClip clip;

	void Start () {
        textureFix(); 


        Destroy(gameObject, lifetime);
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

	void Update(){


      
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
	
	void OnTriggerEnter (Collider collision ) {


        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.name != "Nave" && collision.gameObject.name != "Terrain")
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), collision);
        }
        /* if (collision.gameObject.name != "OVRPlayerController" && collision.gameObject.name != "R Cube" && collision.gameObject.name != "L Cube") {

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
        if (collision.gameObject.name == "Nave") {
            Debug.Log("colision " + collision.gameObject.name);
            Destroy(collision.gameObject);
            detonation();
        }
		if (collision.gameObject.name == "Terrain") {
            Debug.Log("colision " + collision.gameObject.name);
            destroyTerrain(collision,type);
            detonationTerrain();
        }
		
	}


	void destroyTerrain (Collider collision,bool type){
		
		int xRes = tData.heightmapWidth;
		int yRes = tData.heightmapHeight;
		
		//shotSoundSource.PlayOneShot(shotSound,1.0f);
	//	Debug.Log("xRes " +  xRes);
	//	Debug.Log("yRes " +  yRes);
     //   Debug.Log(collision.gameObject.GetComponent<TerrainCollider>().bounds.size.x.GetType());
		
		
		int x = (int) Mathf.Lerp(0, xRes, Mathf.InverseLerp(collision.gameObject.transform.position.x, (collision.gameObject.GetComponent<TerrainCollider>().bounds.size.x/2), transform.position.x ));
		int y = (int) Mathf.Lerp(0, yRes, Mathf.InverseLerp(collision.gameObject.transform.position.z, (collision.gameObject.GetComponent<TerrainCollider>().bounds.size.x/2), transform.position.z));

        /*if ((int)tData.size.x > x+terrainDestructSize) {
			x = (int)tData.size.x - terrainDestructSize;
		}
		if ( (int)tData.size.z > z+terrainDestructSize) {
			z = (int)tData.size.z - terrainDestructSize;
		}*/

        int size = brush.width;
        // size = 4;
        int despX = size / 2;
        int despY = -size/2;
        float[,] areaT;
		try {
			areaT = tData.GetHeights(x+ despX, y+ despY, size, size);
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
                    Color texPixel = brush.GetPixel(i, j);
                    if (type)
                    {
                        areaT[i, j] += texPixel.grayscale / 3.5f;
                    }
                    else
                    {
                        areaT[i, j] -= texPixel.grayscale / 3.5f;
                    }
                        
                    //areaT[i, j] = 0;

                }
			}
            tData.SetHeights(x+ despX, y+ despY, areaT);
        }
        catch(System.Exception e){
            Debug.Log(e.Message.ToString());
			//areaT = tData.GetHeights(x, z, 1, 1);
		//	areaT [x,z] -=  terrainDestructHeight /100 ;
		}
		
		
	}


    void detonationTerrain()
    {

      //  Component dTemp = currentDetonatorTerrain.GetComponent("Detonator");

        //float offsetSize = dTemp.size/3;

        GameObject exp = (GameObject)Instantiate(currentDetonatorTerrain, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(clip, transform.position);
       // dTemp = exp.GetComponent("Detonator");
        //dTemp.detail = detailLevel;

        Destroy(exp, explosionLife);



    }
    void detonation (){
		
		//Component dTemp = currentDetonator.GetComponent("Detonator");
		
		//float offsetSize = dTemp.size/3;
		
		GameObject exp = (GameObject) Instantiate(currentDetonator, transform.position, Quaternion.identity);
		AudioSource.PlayClipAtPoint(clip, transform.position);
		//dTemp = exp.GetComponent("Detonator");
		//dTemp.detail = detailLevel;

		Destroy(exp, explosionLife);
        
	
		
	}
}
