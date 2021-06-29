using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using UnityEngine.Tilemaps;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    public Tilemap tileMap;

    public List<TileBase> tileBases;

    // 현재 전투의 모든 타일
    public Model.Tile[,] field;

    // 필드 데이터
    public int[,] Field1
    {
        get => new int[16, 16]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            {1,1,1,0,2,0,0,2,0,0,0,0,0,1,1,1 },
            {1,1,1,0,0,0,3,0,0,0,0,0,0,1,1,1 },
            {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            {1,1,1,0,0,0,0,4,0,0,0,0,0,1,1,1 },
            {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1 },
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
        };
    }
    private int[,] Field2
    {
        get => new int[16, 16]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        };
    }

    public void InitField(int[,] fildData)
    {
        field = new Model.Tile[fildData.GetLength(0), fildData.GetLength(1)];

        for (int y = 0; y < fildData.GetLength(0); y++)
        {
            for (int x = 0; x < fildData.GetLength(1); x++)
            {
                field[y, x] = new Model.Tile();
                field[y, x].category = (Model.Tile.Category)fildData[y, x];
            }
        }

        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), tileBases[(int) field[y, x].category]);
            }
        }
    }

    private void Awake()
    {
        instance = this;
        tileMap = GetComponentInChildren<Tilemap>();
    }
}
