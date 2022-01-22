using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(GenatorMap))]
[RequireComponent(typeof(CameraZoom))]


public class GameManager : MonoBehaviour
{
    [SerializeField] int row;
    [SerializeField] int col;
    [SerializeField] float smooth;
    [SerializeField] Animator finishAni;
    [SerializeField] TextMeshProUGUI currentLevel;
    [SerializeField] ParticleSystem finishEffect;

    private Camera mainCamera;
    private bool isEnd;
    private static GameManager instance;

    public float Smooth { get => smooth; set => smooth = value; }
    public int Row { get => row; set => row = value; }
    public int Col { get => col; set => col = value; }
    public bool IsEnd { get => isEnd; set => isEnd = value; }
    public static GameManager Instance { get => instance; set => instance = value; }
    public Camera MainCamera { get => mainCamera; set => mainCamera = value; }
    public bool IsNext { get; set; }
    #region Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }
    #endregion
    void Start()
    {
        SetLevel(1);
        mainCamera = Camera.main;
        isEnd = false;
        GenatorMap.Instance.CreateMap(row, col);
    }
    private void Update()
    {
        EnlessMode();
    }
    public void End(bool In_Out)
    {
        finishAni.SetBool("In-Out", In_Out);
        if (In_Out)
            finishEffect.Play();
    }
    void EnlessMode()
    {

        if (isEnd && IsNext)
        {
            if (Mathf.Abs(row - col) > 3)
            {
                if (row > col)
                    col++;
                else
                    row++;
            }
            else
            {
                int randomMode = Random.Range(0, 6);
                if (randomMode == 0)
                {
                    int temp = row;
                    row = col;
                    col = temp;
                }
                else if (randomMode % 2 == 0)
                    col++;
                else if (randomMode % 2 == 1)
                    row++;
            }
            GenatorMap.Instance.CreateMap(row, col);
            SetLevel(PlayerPrefs.GetInt("CurrentLevels") + 1);
            IsNext = false;
        }
    }
    void SetLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevels", level);
        currentLevel.text = "#"+PlayerPrefs.GetInt("CurrentLevels").ToString();
    }
}
