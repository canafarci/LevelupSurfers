using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StateChangeDotween : MonoBehaviour, ITweener
{
    Vector3 _scale;
    float _duration = 0.175f;
    Queue<Sequence> _sequenceStack = new Queue<Sequence>();
    private void Start() => StartCoroutine(EmptyStack());

    private void Awake() => _scale = transform.localScale;
    public void Tween()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(_scale * 1.2f, _duration / 2f)).
        Append(transform.DOScale(_scale, _duration / 2f));

        sequence.Pause();
        _sequenceStack.Enqueue(sequence);
    }

    IEnumerator EmptyStack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            while (_sequenceStack.Count > 0)
            {
                Sequence sequence = _sequenceStack.Dequeue();
                sequence.Play();
                yield return sequence.WaitForCompletion();
            }
        }
    }
}
