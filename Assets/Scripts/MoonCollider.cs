using System.Collections;
using UnityEngine;

public class MoonCollider : MonoBehaviour
{
    private Collider _other;

    private void OnTriggerEnter(Collider other)
    {
       // if (other.gameObject.layer == LayerMask.NameToLayer("Effect"))
        {
            _other = other;
            StartCoroutine(Hit());
 
            Magic._hitCount++;
            Magic.instance.UpdatePercent();

       
        }           
    }

    IEnumerator Hit()
    {
        if (_other)
        {

            //  _meshRenderer.material = _hit;
            _other.gameObject.transform.localScale = _other.gameObject.transform.localScale * 11.5f;
            yield return new WaitForSeconds(1f);
            Destroy(_other.gameObject);
        }
        yield return new WaitForSeconds(1f);

        //  _meshRenderer.material = _original;
    }

}
