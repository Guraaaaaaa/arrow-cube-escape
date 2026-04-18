using DG.Tweening;
using UnityEngine;

public class shape : MonoBehaviour
{
    [SerializeField] private Transform _innerShape, _outerShape;
    [SerializeField] private float _cycleLenght = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DOMove(new Vector3(10, 0, 0), _cycleLenght).SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
