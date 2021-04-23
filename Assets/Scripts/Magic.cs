
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


    [SerializeField] private Transform _kames;

    [Header("Hand")]
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;
    private Vector3 _leftHandLastValid;
    private Vector3 _rightHandLastValid;

    [Tooltip("Hand distance to init Kame Hame Ha.")]
    [Range(0f, 5f)]
    public float _handDistance = 0.1f;

    [Header("Kame Hame Ha")]
    [Tooltip("Destroy distance from camera")]
    [Range(0f, 10000f)]
    public float _destroyDistance = 1000f;    
    
    [Tooltip("Max size of Kame Hame Ha.")]
    [Range(0f, 8f)]
    public float _kameHameMaxSize = 2f;

    [Tooltip("Vertical position of Kame Hame Ha.")]
    [Range(-3f, 3f)]
    public float _kameHameHaPosition = 0.5f;
    [Tooltip("Hands shoot intensity of Kame Hame Ha.")]
    [Range(0f, 5f)]
    public float _kameHameHaShootMagnitude = 1f;
    [Tooltip("Velocity of Kame Hame Ha.")]
    [Range(0, 30000)]
    public int _kameHameHaShootVelocity = 50;

    [Tooltip("Velocity multiplier.")]
    [Range(0, 1500)]
    public int _kameHameHaShootMaxMultiplier =10;


    [Tooltip("Velocity limit.")]
    [Range(0, 1000000)]
    public int _kameHameHaShootlimit = 10000;


    [Header("Effect")]
    [SerializeField] private Transform[] _magicArray;
    private Transform _currentEffect;

    [Header("Score")]
    [SerializeField] private TMPro.TextMeshPro _aimPercentText;
    [SerializeField] private TMPro.TextMeshPro _hitsText;
    [SerializeField] private TMPro.TextMeshPro _DistanceText;
    private float distance;
    private Rigidbody _magicRd;
    private int index;
    private Rigidbody _leftHandRd;
    private Rigidbody _rightHandRd;
    private List<ParticleSystem> _magicParticleList;
    [Header("Sound")]
    public AudioSource AudioSourceKame;
    public AudioClip Create;
    public AudioClip Launch;


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


        // Measure the distance between both palms
        distance = Vector3.Distance(_leftHand.position, _rightHand.position);
        _DistanceText.text = "Distance: " + String.Format("{0:0.00}", distance);

        // limit kame hame size
        if (distance > _kameHameMaxSize)
        {
            distance = _kameHameMaxSize;
        }
      
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

        // kame hame firing
        ShotMagic();
    }


    private void CreateEffect()
    {
       
        // Generated after determining the effect at random
        index = UnityEngine.Random.Range(0, _magicArray.Length);
        _currentEffect = Instantiate(_magicArray[index], _kames);
        _currentEffect.name = "kamehameha";
        _currentEffect.transform.parent = _kames;
        _currentEffect.GetComponent<KameHameHa>().Distance = _destroyDistance;

        _magicRd = _currentEffect.GetComponent<Rigidbody>();

        AudioSourceKame.clip = Create;
        AudioSourceKame.loop = true;
        AudioSourceKame.Play();

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

        AudioSourceKame.pitch = distance;

        Vector3 middlePosition = _leftHand.position - ((_leftHand.position - _rightHand.position) / 2);

        _currentEffect.localPosition = new Vector3(middlePosition.x, middlePosition.y + _kameHameHaPosition, middlePosition.z);

        // Pull out multiple particles from the list and scale them
        for (int i = 0; i < _magicParticleList.Count; i++)
            _magicParticleList[i].transform.localScale = new Vector3(distance, distance, distance) * 0.1f;

        // When the strength of both hands is greater than 2 and the distance is greater than 0.2, magic is fired.
        var midway = _leftHandRd.transform.up + _rightHandRd.transform.up;
        float speed = Vector3.Dot(midway, _rightHandRd.transform.up);



        if (speed > _kameHameHaShootMagnitude)
        {
            speed = (speed - _kameHameHaShootMagnitude) * _kameHameHaShootVelocity * _kameHameHaShootMaxMultiplier;
            if (speed < _kameHameHaShootMagnitude * _kameHameHaShootVelocity /2)
            {
                speed = _kameHameHaShootMagnitude * _kameHameHaShootVelocity /2;

            }
            if (speed > _kameHameHaShootlimit)
            {
                speed = _kameHameHaShootlimit;
            }
            AudioSourceKame.Stop();
            AudioSourceKame.clip = Launch;
            AudioSourceKame.loop = false;
            float normalizedValue = Mathf.InverseLerp(_kameHameHaShootVelocity,_kameHameHaShootlimit, speed);
            float pitch = Mathf.Lerp(0.5f, 2.5f, normalizedValue);

            AudioSourceKame.pitch = pitch;
            _currentEffect.GetComponent<KameHameHa>().Velocity = _kameHameHaShootlimit / speed;
            AudioSourceKame.Play();
            _currentEffect.GetComponent<KameHameHa>().Size = distance  * 100 / _kameHameMaxSize;

            _hitsText.text = "Speed: " + String.Format("{0:0.00}", speed);
            _magicRd.AddForce(midway  * speed); 
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

        _aimPercentText.text = "Hits: " + percent.ToString("F1") + "%";
        //   _hitsText.text = "Hits: " + _hitCount ;
    }

    public void closed()
    {


        _aimPercentText.text = "Closed";
        //   _hitsText.text = "Hits: " + _hitCount ;
    }

    public void opened()
    {


        _aimPercentText.text = "Open";
        //   _hitsText.text = "Hits: " + _hitCount ;
    }
}
