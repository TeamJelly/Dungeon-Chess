using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using static Model.Managers.FieldManager;
using Model.Tiles;
using Model.Managers;
using System.Text;

public class Chunk
{
    Dictionary<int, List<FieldData>> FieldDataDictionary {
        get 
        {
            if (fieldDataDictionary == null)
            {
                fieldDataDictionary = new Dictionary<int, List<FieldData>>()
                {

                };
            }

            return fieldDataDictionary;
        }
    }
    Dictionary<int, List<FieldData>> fieldDataDictionary;

    FieldData[] fieldDataSet = new FieldData[16]
    {
        // b0000 : 다막힘
        new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL WL WL WL\n"
                ),
        // b0001 : 위
        new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL WL WL WL\n"
                ),
        // b0010 : 오른
        new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR FR\n" +
                    "WL WL WL WL\n"
                ),
        // b0011 : 위, 오른
        new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR FR\n" +
                    "WL WL WL WL\n"
                ),
        // b0100 : 아래
        new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n"
                ),
        // b0101 : 위, 아래
        new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n"
                ),
        // b0110 : 오른, 아래
        new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR WL\n"
                ),
        // b0111 : 위, 오른, 아래
        new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR WL\n"
                ),
        // b1000 : 왼
        new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "FR FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "WL WL WL WL\n"
                ),
        // b1001 : 위, 왼
        new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "WL WL WL WL\n"
                ),
        // b1010 : 오른, 왼
        new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "FR FR FR FR\n" +
                    "FR FR FR FR\n" +
                    "WL WL WL WL\n"
                ),
        // b1011 : 위, 오른, 왼
        new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "FR FR FR FR\n" +
                    "FR FR FR FR\n" +
                    "WL WL WL WL\n"
                ),
        // b1100 : 아래, 왼
        new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "FR FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "WL FR FR WL\n"
                ),
        // b1101 : 위, 아래, 왼
        new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "WL FR FR WL\n"
                ),
        // b1110 : 오른, 아래, 왼
        new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "FR FR FR FR\n" +
                    "FR FR FR FR\n" +
                    "WL FR FR WL\n"
                ),
        // b1111 : 위, 오른, 아래, 왼
        new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "FR FR FR FR\n" +
                    "FR FR FR FR\n" +
                    "WL FR FR WL\n"
                )
    };

    class Tile4x4
    {
        public Tile[,] field;
        public FieldData fieldData;
        
        public Tile4x4(int x, int y)
        {
            pos = new Vector2Int(x, y);
        }
        
        public void SetFieldData(FieldData fieldData)
        {
            field = new Tile[4, 4];
            this.fieldData = fieldData;
        }
        
        public Tile GetField(Vector2Int p)
        {
            return field[p.y, p.x];
        }

        public bool visited = false;
        public List<Vector2Int> availableDirections = new List<Vector2Int>();
        public Vector2Int pos;

        /// <summary>
        /// b 0    0     0    0
        /// b(왼)(아래)(오른)(위)
        /// </summary>
        public int path_ID = 0;

        /// <summary>
        /// 실제 ID
        /// </summary>
        public int wall_ID = 0;
    }

    List<Vector2Int> directions = new List<Vector2Int>()
    {
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
    };

    // Start is called before the first frame update

    int size = 3;
    public void Start()
    {
        GenerateMap();
    }

    void InitTileBoxies(Tile4x4[,] tileBoxies)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Tile4x4 tile = new Tile4x4(x, y);

                int idx = 0;
                foreach (var direction in directions)
                {
                    Vector2Int v = tile.pos + direction;
                    if (!(v.x < 0 || v.x >= size || v.y < 0 || v.y >= size))
                    {
                        tile.availableDirections.Add(direction);

                        idx += (int)Mathf.Pow(2, directions.IndexOf(direction));
                    }
                }
                tileBoxies[y, x] = tile;
            }
        }
    }

    void GenerateMazeData(Tile4x4[,] tileBoxies)
    {
        Vector2Int nextPos;
        Tile4x4 currentTile = tileBoxies[size - 1, 0];

        // 입구와 연결되도록 길을 뚫어준다.
        currentTile.path_ID += 4;

        Stack<Tile4x4> visited_Tiles = new Stack<Tile4x4>();

        int visited_count = 1;
        int all_tile_count = size * size;

        // 미로 생성 알고리즘. 완료될때까지 반복
        while (true)
        {
            currentTile.visited = true;
            for (int i = currentTile.availableDirections.Count - 1; i >= 0; i--)
            {
                var pos = currentTile.pos + currentTile.availableDirections[i];
                if (tileBoxies[pos.y, pos.x].visited)
                    currentTile.availableDirections.RemoveAt(i);
            }

            //더 이상 갈 방향이 없을 때
            if (currentTile.availableDirections.Count == 0)
            {
                //모든 타일이 할당 되었을 때
                if (visited_count == all_tile_count)
                    break; // end

                //이전 타일로 돌아감
                else
                {
                    currentTile = visited_Tiles.Pop();
                }
            }

            //갈 수 있는 방향 중 하나를 선택.
            else
            {
                //방향 선택하고 현재 타일에서 해당 방향 제거

                Vector2Int direction = currentTile.availableDirections[Random.Range(0, currentTile.availableDirections.Count)];
                nextPos = currentTile.pos + direction;
                currentTile.availableDirections.Remove(direction);
                currentTile.path_ID += (int)Mathf.Pow(2, directions.IndexOf(direction));

                //stack에 push 후 타일 이동.
                visited_Tiles.Push(currentTile);
                visited_count++;

                //이전에 있던 타일로 가는 길 제거
                currentTile = tileBoxies[nextPos.y, nextPos.x];
                currentTile.path_ID += (int)Mathf.Pow(2, directions.IndexOf(-direction));
            }
        }
    }

    void MakeRoughWall(Tile4x4[,] tileBoxies)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Tile4x4 tile = tileBoxies[y, x];

                foreach (var direction in directions)
                {
                    // 길이다 > 벽을 세우면 안됨
                    // 길이 아니다. 
                    //      그 방향에 타일이 없다.                      > 벽 있어도 되고 없어도 됨
                    //      그 방향에 있는 타일이 벽을 세워놨다.        > 벽 있어도 되고 없어도 됨
                    //      그 방향에 있는 타일이 아직 벽을 안세워놨다. > 벽을 세워야 함

                    // 길이다 > 벽을 세우면 안됨
                    if ((tile.path_ID & (int)Mathf.Pow(2, directions.IndexOf(direction))) != 0)
                        continue;
                    // 길이 아니다. 
                    else
                    {
                        Vector2Int v = tile.pos + direction;

                        //      그 방향에 타일이 없다.                      > 벽 있어도 되고 없어도 됨
                        if (v.x < 0 || v.x >= size || v.y < 0 || v.y >= size)
                        {
                            tile.wall_ID += Random.Range(0, 2) * (int)Mathf.Pow(2, directions.IndexOf(direction));
                            continue;
                        }

                        Tile4x4 next = tileBoxies[v.y, v.x];
                        //      그 방향에 있는 타일이 벽을 세워놨다.        > 벽 있어도 되고 없어도 됨
                        if ((next.wall_ID & (int)Mathf.Pow(2, directions.IndexOf(-direction))) != 0)
                        {
                            tile.wall_ID += Random.Range(0, 2) * (int)Mathf.Pow(2, directions.IndexOf(direction));
                        }
                        //      그 방향에 있는 타일이 아직 벽을 안세워놨다. > 벽을 세워야 함
                        else
                        {
                            tile.wall_ID += (int)Mathf.Pow(2, directions.IndexOf(direction));
                        }
                    }
                }
            }
        }
    }

    public FieldData GenerateMap()
    {

        Tile4x4[,] tileBoxies1 = new Tile4x4[size, size];
        Tile4x4[,] tileBoxies2 = new Tile4x4[size, size];

        //타일 생성
        InitTileBoxies(tileBoxies1);
        InitTileBoxies(tileBoxies2);

        //미로 데이터 생성
        GenerateMazeData(tileBoxies1);
        GenerateMazeData(tileBoxies2);

        //두개의 미로 데이터를 이용해서 길 두번 뚫기.
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Tile4x4 tile1 = tileBoxies1[y, x];
                Tile4x4 tile2 = tileBoxies2[y, x];
                tile1.path_ID |= tile2.path_ID;
            }
        }

        //생성된 미로 데이터를 기반으로 벽의 다양성을 부여.
        MakeRoughWall(tileBoxies1);

        //생성된 미로 데이터를 기반으로 필드 채우기
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Tile4x4 tile = tileBoxies1[i, j];

                /// 여기서 파일 불러와서 아이디 적용.
                
                tile.SetFieldData(fieldDataSet[15 - tile.wall_ID]);
            }
        }

        return GetFieldData(tileBoxies1);
    }

    FieldData GetFieldData(Tile4x4[,] tileBoxies)
    {
        FieldData result = new FieldData(size * 4, size * 4);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                result = FieldManager.instance.Merge2FieldData(result, tileBoxies[i, j].fieldData, new Vector2Int(j * 4, i * 4));
            }
        }
        FieldData platform = new FieldData(size * 4, 2, 'W', 'L');

        StringBuilder sb = new StringBuilder(platform.fieldStrData);
        sb[0] = 'U';
        sb[1] = 'S';
        sb[2] = 'U';
        sb[3] = 'S';
        sb[4] = 'U';
        sb[5] = 'S';
        sb[6] = 'U';
        sb[7] = 'S';

        platform.fieldStrData = sb.ToString();
        result = FieldManager.instance.Merge2FieldData(result, platform, new Vector2Int(0, result.height));
        return result;
    }
}
