using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _original;
    [SerializeField] private Material _hit;
    public GameObject currentDetonatorTerrain;
    public float explosionLife = 10;
    public float detailLevel = 1.0f;


    public AudioClip clip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "kamehameha")
        {
            StartCoroutine(Hit());

            detonationTerrain(other);
            Magic._hitCount++;
            Magic.instance.UpdatePercent();

            Destroy(other.gameObject);
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

        Destroy(exp, explosionLife / 4);
        Destroy(exp2, explosionLife);




    }

    IEnumerator Hit()
    {
        _meshRenderer.material = _hit;

        yield return new WaitForSeconds(explosionLife/4.5f);

        _meshRenderer.material = _original;
    }

}
