using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GenatorMap : MonoBehaviour
{
    [SerializeField] Dictionary<Vector2, Vector4> allPiece = new Dictionary<Vector2, Vector4>();
    [SerializeField] Dictionary<Vector2, int> allPieceToType = new Dictionary<Vector2, int>();
    [SerializeField] GameObject[] piecesObj;
    [SerializeField] int connected;
    [SerializeField] int amountConnect;
    [SerializeField] GameObject rowInput;
    [SerializeField] GameObject colInput;

    private List<Vector2> unVisited = new List<Vector2>();
    private Stack<Vector2> stack = new Stack<Vector2>();
    private Vector2[] neightborPos = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private Vector2 currentPos;
    private int row;
    private int col;
    private Piece[,] pieces;
    private GameObject parentPieces;
    private int preAmountConnect;
    private static GenatorMap instance;

    public static GenatorMap Instance { get => instance; set => instance = value; }
    #region Singleton
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion
    void Start()
    {
        row = GameManager.Instance.Row;
        col = GameManager.Instance.Col;
    }
    public void CreateMap(int row, int col)
    {
        //if (rowInput.GetComponent<TMP_InputField>().text != "")
        //    row = int.Parse(rowInput.GetComponent<TMP_InputField>().text);
        //if (colInput.GetComponent<TMP_InputField>().text != "")
        //    col = int.Parse(colInput.GetComponent<TMP_InputField>().text);

        this.row = row;
        this.col = col;

        currentPos = Vector2.zero;
        allPiece.Clear();
        allPieceToType.Clear();
        unVisited.Clear();
        stack.Clear();
        Destroy(parentPieces);

        GameManager.Instance.IsEnd = false;
        GameManager.Instance.End(false);
        //add all piece
        for (int x = 0; x < this.row; x++)
        {
            for (int y = 0; y < this.col; y++)
            {
                Vector2 pos = new Vector2(y, x);
                allPiece.Add(pos, Vector4.zero);
                unVisited.Add(pos);
            }
        }
        unVisited.Remove(Vector2.zero);
        FindWay();
        TransferVector4ToInt();
        this.pieces = new Piece[this.row, this.col];
        SpawnArray();
        preAmountConnect = amountConnect;
        this.amountConnect = CalculatorConect() - preAmountConnect;
        this.connected = CalculatorConected();
    }
    void FindWay()
    {
        while (unVisited.Count > 0)
        {
            List<Vector2> neighbor = GetNeighbor(currentPos);
            if (neighbor.Count != 0)
            {
                RandomWay(neighbor);
            }
            else//make a new array or extent the current array
            {
                if (Random.Range(0, 5) == 1)//Make new
                {
                    stack.Clear();
                    unVisited.Remove(currentPos);
                    if (unVisited.Count != 0)
                    {
                        currentPos = unVisited[Random.Range(0, unVisited.Count - 1)];
                        unVisited.Remove(currentPos);
                    }
                }
                else // Extent
                {
                    while (stack.Count > 0)
                    {
                        currentPos = stack.Pop();//take and delete
                        neighbor = GetNeighbor(currentPos);
                        if (neighbor.Count != 0)
                        {
                            RandomWay(neighbor);
                        }
                    }
                }
            }
        }

    }

    private void RandomWay(List<Vector2> neighbor)
    {
        Vector2 nPos = neighbor[Random.Range(0, neighbor.Count)];
        Vector2 dir = nPos - currentPos;
        allPiece[currentPos] += SetDirection(dir);
        allPiece[nPos] += SetDirection(Vector2.zero - dir);
        stack.Push(currentPos);
        currentPos = nPos;
        unVisited.Remove(nPos);
    }
    Vector4 SetDirection(Vector2 dir)
    {
        Vector4 a = Vector4.zero;
        switch (dir)
        {
            case Vector2 v when v.Equals(Vector2.up):
                a = new Vector4(1, 0, 0, 0);
                break;
            case Vector2 v when v.Equals(Vector2.right):
                a = new Vector4(0, 1, 0, 0);
                break;
            case Vector2 v when v.Equals(Vector2.down):
                a = new Vector4(0, 0, 1, 0);
                break;
            case Vector2 v when v.Equals(Vector2.left):
                a = new Vector4(0, 0, 0, 1);
                break;
        }
        return a;
    }
    List<Vector2> GetNeighbor(Vector2 currenPos)
    {
        List<Vector2> neighborPiece = new List<Vector2>();
        foreach (Vector2 neighbor in neightborPos)
        {
            Vector2 n = currenPos + neighbor;
            if (unVisited.Contains(n))
                neighborPiece.Add(n);
        }
        return neighborPiece;
    }
    void SpawnArray()
    {
        parentPieces = Instantiate(piecesObj[5], Vector2.zero, Quaternion.identity);
        Destroy(parentPieces.GetComponent<Piece>());
        parentPieces.tag = "Untagged";
        parentPieces.name = "Pieces";
        foreach (var item in allPieceToType)
            SpawnPiece(item.Key, item.Value, parentPieces);
        ResetCamPosition();
    }
    public void ResetCamPosition()
    {
        GameManager.Instance.MainCamera.transform.position = new Vector3( (col-1) / 2f,(row-1) / 2f, -10);
        float cameraSize = col > row ? col : row * Screen.height / Screen.width * 0.5f;
        GameManager.Instance.MainCamera.orthographicSize = cameraSize>9?cameraSize:9;
    }
    void SpawnPiece(Vector2 pos, int typeOfPiece, GameObject parent)
    {
        GameObject clone = Instantiate(this.piecesObj[typeOfPiece], pos, Quaternion.identity);
        clone.transform.parent = parent.transform;
        this.pieces[(int)pos.y, (int)pos.x] = clone.GetComponent<Piece>();
    }
    int CalculatorConect()
    {
        int amount = 0;
        foreach (var p in GameObject.FindGameObjectsWithTag("Piece"))
        {
            foreach (var p2 in p.GetComponent<Piece>().Values)
            {
                amount += p2;
            }
        }
        return amount / 2;
    }
    public int CalculatorConected()
    {
        int amount = 0;
        for (int i = 0; i < this.row; i++)
        {
            for (int j = 0; j < this.col; j++)
            {
                //left to right
                if (j != this.col - 1)
                    if (this.pieces[i, j].Values[1] == 1 && this.pieces[i, j + 1].Values[3] == 1)
                        amount++;
                //bottom to top
                if (i != this.row - 1)
                    if (this.pieces[i, j].Values[0] == 1 && this.pieces[i + 1, j].Values[2] == 1)
                        amount++;
            }
        }
        return connected = amount;
    }
    public enum TypeOfPiece : int
    {
        THREE_WAY = 0,
        TWO_WAY = 1,//SHPE LIKE "L"
        ONLY_WAY = 2,
        FOUR_WAY = 3,
        STRAIGHT_WAY = 4,//LIKE "I"
        NO_WAY = 5
    }
    void TransferVector4ToInt()
    {
        foreach (var item in allPiece)
        {
            int totalValue = (int)(item.Value.x + item.Value.y + item.Value.z + item.Value.w);
            switch (totalValue)
            {
                case 0:
                    allPieceToType.Add(item.Key, (int)TypeOfPiece.NO_WAY);
                    break;
                case 1:
                    allPieceToType.Add(item.Key, (int)TypeOfPiece.ONLY_WAY);
                    break;
                case 2:
                    float[] value = new float[4] { item.Value.x, item.Value.y, item.Value.z, item.Value.w };
                    bool isStranightWay = false;
                    for (int i = 0; i < 2; i++)
                    {
                        if (value[i] == value[i + 2])
                        {
                            isStranightWay = true;
                            break;
                        }
                    }
                    if (isStranightWay)
                        allPieceToType.Add(item.Key, (int)TypeOfPiece.STRAIGHT_WAY);
                    else
                        allPieceToType.Add(item.Key, (int)TypeOfPiece.TWO_WAY);
                    break;
                case 3:
                    allPieceToType.Add(item.Key, (int)TypeOfPiece.THREE_WAY);
                    break;
                case 4:
                    allPieceToType.Add(item.Key, (int)TypeOfPiece.FOUR_WAY);
                    break;

            }
        }
    }
    public bool EndGame()
    {
        if (connected == amountConnect)
            return true;
        return false;
    }

}
