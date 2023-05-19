using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveVertical : MonoBehaviour
{
    float _speed = 0f;
    int _stageIndex;

    private void OnEnable()
    {
        GameStart.OnGameStart += OnGameStart;
        FindObjectOfType<PlayerState>().StageChangeHandler += OnStageChange;
    }

    private void OnDisable()
    {
        GameStart.OnGameStart -= OnGameStart;

        PlayerState state = FindObjectOfType<PlayerState>();
        if (state != null)
            state.StageChangeHandler -= OnStageChange;
    }
    private void OnGameStart()
    {
        _speed = GameManager.Instance.References.GameConfig.PlayerSpeed;
    }

    private void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.x += _speed * Time.deltaTime;
        transform.localPosition = pos;
    }
    private void OnStageChange(int index)
    {
        _stageIndex = index;
        _speed = GameManager.Instance.References.GameConfig.StageSpeeds[_stageIndex];
    }

    public void ObstacleHitMove()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(() => _speed, x => _speed = x, -5, 0.075f)).
        Append(DOTween.To(() => _speed, x => _speed = x, -50, 0.25f)).
        Append(DOTween.To(() => _speed, x => _speed = x, 0, 0.15f)).
        Append(DOTween.To(() => _speed, x => _speed = x, GameManager.Instance.References.GameConfig.StageSpeeds[_stageIndex], 0.125f));
    }
}
