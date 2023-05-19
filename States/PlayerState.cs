using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerState : MonoBehaviour
{
    public event Action<int, int> LevelChangeHandler;
    public event Action<int> StageChangeHandler;
    public int CurrentLevel { get { return _currentLevel; } }
    [SerializeField] int _currentLevel = 1;
    Coroutine _levelChangeRoutine;
    StateChangeDotween _scaleChangeTween;
    private void Awake() => _scaleChangeTween = GetComponent<StateChangeDotween>();
    public void ChangeCurrentYear(int targetLevel)
    {
        if (targetLevel <= 1)
            targetLevel = 1;

        OnYearChange(_currentLevel, targetLevel);
    }

    void OnYearChange(int startYear, int targetYear)
    {
        LevelChangeHandler?.Invoke(startYear, targetYear);

        _scaleChangeTween.Tween();

        _currentLevel = targetYear;

        //0, 400, 1000, 1500, 2000, 2500 f.e.
        List<int> stageChangeLevels = GameManager.Instance.References.GameConfig.StageChangeLevels.ToList();


        //! LINQ METHOD
        // int closestLevelDownYear = lvlChangeYears.TakeWhile(p => p <= startYear).Last();
        // int closestLevelUpYear = lvlChangeYears.SkipWhile(p => p <= startYear).First();

        //! BINARY SEARCH METHOD
        //* Return value: The zero-based index of item in the sorted List, if item is found; otherwise, 
        //* a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        //* if there is no larger element, the bitwise complement of Count."

        int binaryIndex = stageChangeLevels.BinarySearch(startYear);
        int closestStageDownLevel = binaryIndex < 0 ? stageChangeLevels[~binaryIndex - 1] : stageChangeLevels[binaryIndex];
        int closestStageUpLevel = binaryIndex < 0 ? stageChangeLevels[~binaryIndex] : stageChangeLevels[binaryIndex + 1];

        print(closestStageDownLevel + "  " + closestStageUpLevel);

        if (targetYear >= closestStageUpLevel && startYear != closestStageUpLevel)
            StageChange(stageChangeLevels.IndexOf(closestStageUpLevel));
        else if (targetYear < closestStageDownLevel)
            StageChange(stageChangeLevels.IndexOf(closestStageDownLevel) - 1);
    }

    private void StageChange(int index)
    {
        StageChangeHandler?.Invoke(index);
        BranchedSetActive[] array = GetComponentsInChildren<BranchedSetActive>(true);
        array.FirstOrDefault(x => x.Level == index).SetItemActive();
        array.Where(x => x.Level != index).ToList().ForEach(x => x.SetItemInactive());

        GameObject fx = Instantiate(GameManager.Instance.References.GameConfig.StageUpFX, transform);
        Destroy(fx, 2f);
    }

    // private void Start() => StartCoroutine(OnYearChange(0, 400));
}
