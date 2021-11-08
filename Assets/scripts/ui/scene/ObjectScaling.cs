using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;



public class ObjectScaling : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isDragging;
    private float _currentScale;
    public float minScale, maxScale;
    private float _temp = 0;
    private float _scalingRate = 2;

    private void Start()
    {
        _currentScale = transform.localScale.x;
        StartCoroutine(ResetCollider());
    }

    IEnumerator ResetCollider()
    {
        Destroy(this.gameObject.GetComponent("BoxCollider"));
        yield return 0;
        this.gameObject.AddComponent<BoxCollider>();
        Debug.Log("Collider renewed.");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("scaling received anything.");
        if (Input.touchCount == 1)
        {
            _isDragging = true;
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }

    public void OnMouseUpAsButton()
    {
        Debug.Log("Tapped to close zoom view.");
        transform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isDragging)
            if (Input.touchCount == 2)
            {
                transform.localScale = new Vector2(_currentScale, _currentScale);
                float distance = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                if (_temp > distance)
                {
                    if (_currentScale < minScale)
                        return;
                    _currentScale -= (Time.deltaTime) * _scalingRate;
                }

                else if (_temp < distance)
                {
                    if (_currentScale > maxScale)
                        return;
                    _currentScale += (Time.deltaTime) * _scalingRate;
                }

                _temp = distance;
            }
        else if(Input.touchCount == 1)
            {
                //Panning

            }
    }
}
