
/**
 * Script to create and shoot magic
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magic : MonoBehaviour
{
    public static Magic instance;

    [Header("Hand")]
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;
    private Vector3 _leftHandLastValid;
    private Vector3 _rightHandLastValid;

    private Rigidbody _leftHandRd;
    private Rigidbody _rightHandRd;
    [Tooltip("Hand distance to init Kame Hame Ha.")]
    [Range(0f, 5f)]
    public float _handDistance = 0.1f;

    [Header("Kame Hame Ha")]
    [Tooltip("Max size of Kame Hame Ha.")]
    [Range(0f, 8f)]
    public float _kameHameMaxSize = 2f;

    [Tooltip("Vertical position of Kame Hame Ha.")]
    [Range(-3f, 3f)]
    public float _kameHameHaPosition = 0.5f;
    [Tooltip("Hands shoot intensity of Kame Hame Ha.")]
    [Range(0f, 50f)]
    public float _kameHameHaShootMagnitude = 4f;
    [Tooltip("Velocity of Kame Hame Ha.")]
    [Range(0, 3000)]
    public int _kameHameHaShootVelocity = 50;
    private float distance;

    [Header("Effect")]
    [SerializeField] private Transform[] _magicArray;
    private Transform _currentEffect;
    private Rigidbody _magicRd;
    private int index;
   // private bool _activeMagic;
    private List<ParticleSystem> _magicParticleList;

    [Header("Score")]
    [SerializeField] private TMPro.TextMeshPro _aimPercentText;
    [SerializeField] private TMPro.TextMeshPro _hitsText;
    [SerializeField] private TMPro.TextMeshPro _DistanceText;
    private float _shotCount;
    public static float _hitCount;


    void Start()
    {
        instance = this;

        _leftHandRd = _leftHand.GetComponent<Rigidbody>();
        _rightHandRd = _rightHand.GetComponent<Rigidbody>();

        _magicParticleList = new List<ParticleSystem>();

        _shotCount = 0;
        _hitCount = 0;
    }

    private void FixedUpdate()
    {


   //     var speed = Vector3.Dot(_leftHandRd.velocity, _leftHandRd.transform.up * -1);
  //      _hitsText.text = "speed: " + speed;
        // Measure the distance between both palms
        distance = Vector3.Distance(_leftHand.position, _rightHand.position);
        _DistanceText.text = "Distance: " + String.Format("{0:0.00}", distance);

        if (distance> _kameHameMaxSize)
        {
            distance = _kameHameMaxSize;
        }

        // Debug.Log(distance);
        // The distance is less than 0.1 and no magic is generated.
        if (distance < _handDistance && _currentEffect == null)
        {
            CreateEffect();
        }

        // If the magic has not been created, or there is no currently created effect
        if (_currentEffect == null)
        {
            return;
        }

        // Magic firing
        ShotMagic();
    }


    private void CreateEffect()
    {
        // reset
        //_activeMagic = true;

        // Generated after determining the effect at random
        index = UnityEngine.Random.Range(0, _magicArray.Length);
        _currentEffect = Instantiate(_magicArray[index]);
        _currentEffect.name = "kamehameha";
        _currentEffect.GetComponent<DestoryEffect>().SetMagic(instance);

        _magicRd = _currentEffect.GetComponent<Rigidbody>();

        _magicParticleList.Clear();

        // Included in a list consisting of several particles
        for (int i = 0; i < _currentEffect.childCount; i++)
            _magicParticleList.Add(_currentEffect.GetChild(i).GetComponent<ParticleSystem>());
    }

    private void ShotMagic()
    {
        // If the distance of the bottom of the hand is greater than 0.5, it just fires forward
        /* if (distance > 0.5f)
         {
             _activeMagic = false;
             _magicRd.AddForce(Vector3.forward * 2, ForceMode.Impulse);
             _shotCount++;
             return;
         }*/

        // Set the effect position to the center of both hands
        _currentEffect.parent = transform;

    //    Debug.Log("hads active" + _leftHandRd.gameObject.activeSelf + "+"+ _rightHand.gameObject.activeSelf);
        Debug.Log("hads position" + _leftHand.position + "+"+ _rightHand.position);

        if (!_leftHandRd.gameObject.activeInHierarchy)
        {
            _leftHand.position = _leftHandLastValid;
        }
        else
        {
            _leftHandLastValid = _leftHand.position;
        }

        if (!_rightHandRd.gameObject.activeInHierarchy)
        {
            _rightHand.position = _rightHandLastValid;
        }
        else
        {
            _rightHandLastValid = _rightHand.position;
        }


        Vector3 middlePosition = _leftHand.position - (( _leftHand.position - _rightHand.position)/2);

        _currentEffect.position = new Vector3(middlePosition.x, middlePosition.y + _kameHameHaPosition, middlePosition.z) ;

        // Pull out multiple particles from the list and scale them
        for (int i = 0; i < _magicParticleList.Count; i++)
            _magicParticleList[i].transform.localScale = new Vector3(distance, distance, distance) * 0.1f;

        // When the strength of both hands is greater than 2 and the distance is greater than 0.2, magic is fired.
        var midway = _leftHandRd.transform.up + _rightHandRd.transform.up;
        var speed = Vector3.Dot(midway, _leftHandRd.transform.up );
        _hitsText.text = "Speed: " + String.Format("{0:0.00}", speed);
    
        if (speed > _kameHameHaShootMagnitude)
        {
            _magicRd.AddForce(midway * speed *_kameHameHaShootVelocity );
            _magicRd = null;
            _currentEffect = null;
            _shotCount++;
        }
    }

    /// <summary>
    /// Accuracy update
    /// </summary>
    public void UpdatePercent()
    {
        var percent = _hitCount / _shotCount * 100;

        _aimPercentText.text = "Accuracy: " + percent.ToString("F1") + "%";
     //   _hitsText.text = "Hits: " + _hitCount ;
    }
}
