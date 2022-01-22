using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] int[] values;
    [SerializeField] float smooth;

    private int angle;
    public int[] Values { get => values; set => values = value; }
    private void Start()
    {
        smooth = GameManager.Instance.Smooth;
        int randomRotate = Random.Range(1, 4);
        for (int i = 0; i < randomRotate; i++)
        {
            Rotate();
            RotateValue();
        }
    }
    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), smooth);
    }
    private void OnMouseDown()
    {
        if (!GameManager.Instance.IsEnd)
        {
            Rotate();
            RotateValue();
        }
        GenatorMap.Instance.CalculatorConected();
        GameManager.Instance.IsEnd = GenatorMap.Instance.EndGame();
        if (GameManager.Instance.IsEnd)
            GameManager.Instance.End(true);
    }
    private void Rotate()
    {
        angle += 90;
    }
    private void RotateValue()
    {
        int firstValue = values[0];
        for (int i = 0; i < 3; i++)
        {
            values[i] = values[i + 1];
        }
        values[3] = firstValue;
    }
}
