using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _original;
    [SerializeField] private Material _hit;
    [Tooltip("Detonation prefab")]
    public GameObject currentDetonator;
    [Tooltip("explosion effect Time")]
    [Range(0, 60)]
    public float explosionLife = 10;
    [Tooltip("Return to anchor time")]
    [Range(0, 60)]
    public int _restoreTime = 15;
    public float detailLevel = 1.0f;
    private bool newHit = false;
    public AudioClip clip;
    private float currentTime =0;

    [SerializeField] private int FrameRate = 1;

    void Start()
    {
        InvokeRepeating("CheckDistance", 0, 1 / FrameRate);
    }
    private void CheckDistance()
    {
        if (newHit)
        {
            currentTime += 1;

            if (currentTime > _restoreTime)
            {
                newHit = false;
                Debug.Log("currentTime: " + currentTime + ">" + _restoreTime);
                _meshRenderer.material = _original;
                gameObject.GetComponent<TargetSnap>().interactable = false;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
       // StopCoroutine(Hit());



         newHit = true;

        _meshRenderer.material = _hit;
        currentTime = 0;
      //  StartCoroutine(Hit());
        gameObject.GetComponent<TargetSnap>().interactable = true;
        if (other.gameObject.name == "kamehameha")
        {
            detonation(other);
            Magic._hitCount++;
            Magic.instance.UpdatePercent();

            Destroy(other.gameObject);
        }           
    }

    void detonation(Collider collision)
    {
        GameObject exp = (GameObject)Instantiate(collision.gameObject, collision.transform.position, Quaternion.identity);
        GameObject exp2 = (GameObject)Instantiate(currentDetonator, collision.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(clip, collision.transform.position);

        Destroy(exp, explosionLife / 4);
        Destroy(exp2, explosionLife);
    }
    /*
    IEnumerator Hit()
    {

        for (int i = 0; i < _restoreTime; i++)
        {
            if (newHit)
            {
                
                     yield break;

            }
            yield return new WaitForSeconds(1.0f);
        }
        if (newHit)
        {
            newHit = false;

        }
        else
        {
            _meshRenderer.material = _original;
            gameObject.GetComponent<TargetSnap>().interactable = false;
        }


    }
    */
}
