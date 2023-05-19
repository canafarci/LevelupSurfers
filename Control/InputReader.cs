using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputReader : MonoBehaviour
{
    public Vector3 HitVector { get; private set; }
    public bool IsDragging { get { return _isDragging; } }
    [SerializeField] LayerMask _moveLayer;
    bool _isDragging;
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            _isDragging = true;

        else if (Input.GetMouseButtonUp(0))
            _isDragging = false;

        if (_isDragging)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _moveLayer))
            {
                HitVector = hit.point;
            }
        }
    }

}
