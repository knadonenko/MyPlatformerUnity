﻿using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private int _frameRate;
    [SerializeField] private AnimationClip[] _clips;

    private SpriteRenderer _renderer;
    private float _secondsPerFrame;
    private int _currentFrame;
    private float _nextFrameTime;
    private int _currentClip;

    private bool _isPlaying = true;

    private void OnEnable()
    {
        _nextFrameTime = Time.time;
        enabled = _isPlaying;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = _isPlaying;
    }

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _secondsPerFrame = 1f / _frameRate;
        // _nextFrameTime = Time.time + _secondsPerFrame;
        StartAnimation();
    }

    private void StartAnimation()
    {
        _nextFrameTime = Time.time;
        enabled = _isPlaying = true;
        _currentFrame = 0;
    }

    private void Update()
    {
        if (_nextFrameTime > Time.time || !_isPlaying) return;
        var clip = _clips[_currentClip];
        if (_currentFrame >= clip.Sprites.Length)
        {
            if (clip.Loop)
            {
                _currentFrame = 0;
            }
            else
            {
                enabled = _isPlaying = clip.AllowNext;
                clip.OnComplete?.Invoke();
                if (clip.AllowNext)
                {
                    _currentFrame = 0;
                    _currentClip = (int) Mathf.Repeat(_currentClip + 1, _clips.Length);
                }

            }
            return;
        }

        _renderer.sprite = clip.Sprites[_currentFrame];
        _nextFrameTime += _secondsPerFrame;
        _currentFrame++;
    }

    public void SetClip(string clipName)
    {
        for (var i = 0; i < _clips.Length; i++)
        {
            if (!_clips[i].IsEqualName(clipName)) continue;
            _currentClip = i;
            StartAnimation();
            return;
        }
    }
    
}

[Serializable]
public class AnimationClip
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _allowNext;
    [SerializeField] private UnityEvent _onComplete;

    public bool IsEqualName(string name) => _name == name;
    
    public string Name => _name;
    public Sprite[] Sprites => _sprites;
    public bool AllowNext => _allowNext;
    public UnityEvent OnComplete => _onComplete;
    public bool Loop => _loop;
}
