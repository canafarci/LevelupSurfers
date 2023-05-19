using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveVerticalFixed : MonoBehaviour
{
    [SerializeField] float _speed = 0f;
    private void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.x += _speed * Time.deltaTime;
        transform.localPosition = pos;
    }

    public void StopMove()
    {
        DOTween.To(() => _speed, x => _speed = x, 0, 1);
    }
}
