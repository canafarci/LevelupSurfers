using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class YearText : MonoBehaviour
{
    TextMeshProUGUI _text;
    int _currentLevel = 1;

    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void OnEnable() => FindObjectOfType<PlayerState>().LevelChangeHandler += OnLevelChange;
    private void OnDisable()
    {
        PlayerState state = FindObjectOfType<PlayerState>();
        if (state != null)
            state.LevelChangeHandler -= OnLevelChange;
    }
    void OnLevelChange(int startYear, int targetYear) => StartCoroutine(YearChangeRoutine(targetYear));

    IEnumerator YearChangeRoutine(int targetYear)
    {
        if (targetYear > _currentLevel)
            _text.DOColor(Color.green, .5f);
        else
            _text.DOColor(Color.red, .5f);

        _text.transform.DOScale(Vector3.one * 1.2f, .25f);

        Tween tween = DOTween.To(() => _currentLevel, x => _currentLevel = x, targetYear, 1);

        while (tween.IsActive())
        {
            _text.text = _currentLevel.ToString();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _text.transform.DOScale(Vector3.one, .25f);
        _text.DOColor(Color.white, .15f);
        yield break;
    }

}
