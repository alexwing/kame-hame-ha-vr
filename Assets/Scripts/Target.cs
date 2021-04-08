using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _original;
    [SerializeField] private Material _hit;

    private void OnTriggerEnter(Collider other)
    {
       // if (other.gameObject.layer == LayerMask.NameToLayer("Effect"))
        {
            StartCoroutine(Hit());

            Magic._hitCount++;
            Magic.instance.UpdatePercent();

            Destroy(other.gameObject);
        }           
    }

    IEnumerator Hit()
    {
        _meshRenderer.material = _hit;

        yield return new WaitForSeconds(1f);

        _meshRenderer.material = _original;
    }

}
