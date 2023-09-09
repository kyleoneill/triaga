using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

enum RupeeType {
    Green,
    Blue,
    Red
}

public class RupeeController : MonoBehaviour
{
    [SerializeField] private Sprite GreenSprite;
    [SerializeField] private Sprite BlueSprite;
    [SerializeField] private Sprite RedSprite;

    [SerializeField] private AnimatorController GreenAnimator;
    [SerializeField] private AnimatorController BlueAnimator;
    [SerializeField] private AnimatorController RedAnimator;

    private SpriteRenderer _sr;
    private Animator _animator;
    private RupeeType _rupeeType;
    private int _value;
    
    private void Awake()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        SetRupeeType();
        switch (_rupeeType)
        {
            case RupeeType.Green:
                _sr.sprite = GreenSprite;
                _animator.runtimeAnimatorController = GreenAnimator;
                break;
            case RupeeType.Blue:
                _sr.sprite = BlueSprite;
                _animator.runtimeAnimatorController = BlueAnimator;
                break;
            case RupeeType.Red:
                _sr.sprite = RedSprite;
                _animator.runtimeAnimatorController = RedAnimator;
                break;
        }
    }

    private void SetRupeeType()
    {
        float randValue = Random.Range(0f, 1f);
        if (randValue > 0.9f)
        {
            _rupeeType = RupeeType.Red;
        }
        else if (randValue > 0.7f)
        {
            _rupeeType = RupeeType.Blue;
        }
        else
        {
            _rupeeType = RupeeType.Green;
        }
    }

    public int GetRupeeValue()
    {
        switch (_rupeeType)
        {
            case RupeeType.Green:
                return 1;
            case RupeeType.Blue:
                return 5;
            case RupeeType.Red:
                return 20;
            default:
                return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
