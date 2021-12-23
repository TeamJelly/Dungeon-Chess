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
    FieldData[] fieldDataSet = new FieldData[]
    {
         new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL WL WL WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL WL WL WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR FR\n" +
                    "WL WL WL WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR FR\n" +
                    "WL WL WL WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n" +
                    "WL FR FR WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR FR\n" +
                    "WL FR FR WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "FR FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "WL WL WL WL\n"
                ),
                  new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "WL WL WL WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "FR FR FR FR\n" +
                    "FR FR FR FR\n" +
                    "WL WL WL WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "FR FR FR FR\n" +
                    "FR FR FR FR\n" +
                    "WL WL WL WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "FR FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "WL FR FR WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "FR FR FR WL\n" +
                    "WL FR FR WL\n"
                ),
         new FieldData(
                    4, 4,
                    "WL WL WL WL\n" +
                    "FR FR FR FR\n" +
                    "FR FR FR FR\n" +
                    "WL FR FR WL\n"
                ),
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
            /*char[] chars = fieldData.fieldStrData.ToCharArray();
            int index = 0;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    while (chars[index] == ' ' || chars[index] == '\n')
                        index++;
                    char c1 = chars[index];

                    index++;

                    while (chars[index] == ' ' || chars[index] == '\n')
                        index++;
                    char c2 = chars[index];

                    if ($"{c1}{c2}" == "FR")        // Floor
                        field[y, x] = new Floor();
                    else if ($"{c1}{c2}" == "WL")   // Wall
                        field[y, x] = new Wall();
                    else if ($"{c1}{c2}" == "US")   // UpStair
                        field[y, x] = new UpStair();
                    else if ($"{c1}{c2}" == "DS")   // DownStair
                        field[y, x] = new DownStair();
                    else if ($"{c1}{c2}" == "TN")   // Thorn
                        field[y, x] = new Thorn();
                    else if ($"{c1}{c2}" == "VO")   // Void
                        field[y, x] = new Model.Tiles.Hole();
                    else if ($"{c1}{c2}" == "SL")   // Sell
                        field[y, x] = new Sell();
                    else if ($"{c1}{c2}" == "HL")   // Heal
                        field[y, x] = new Heal();
                    else if ($"{c1}{c2}" == "PW")   // Power
                        field[y, x] = new Power();
                    else if ($"{c1}{c2}" == "LK")   // Locked
                        field[y, x] = new Locked();
                    else if ($"{c1}{c2}" == "UL")   // UnLocked
                        field[y, x] = new UnLocked();
                    else
                        field[y, x] = new Floor();

                    field[y, x].position = new Vector2Int(x + 4 * pos.x, y + 4 * pos.y);
                    index++;
                }
            }*/
        }
        public Tile GetField(Vector2Int p)
        {
            return field[p.y, p.x];
        }
        public bool visited = false;
        public List<Vector2Int> availableDirections = new List<Vector2Int>();

        public Vector2Int pos;
        public int path_ID = 0;
        public int wall_ID = 0;


    }

    public bool isClear = false;
    public Image[] buttons;
    public Sprite[] sprites;
    Tile4x4[,] TileBoxies;
    // Start is called before the first frame update

    int size = 3;
    public void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        List<Vector2Int> directions = new List<Vector2Int>();
        directions.Add(new Vector2Int(0, -1));
        directions.Add(new Vector2Int(1, 0));
        directions.Add(new Vector2Int(0, 1));
        directions.Add(new Vector2Int(-1, 0));

        TileBoxies = new Tile4x4[size, size];
        //타일 생성
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Tile4x4 tile = new Tile4x4(x, y);

                Debug.Log("tile" + tile.pos);

                int idx = 0;
                foreach (var direction in directions)
                {
                    Vector2Int v = tile.pos + direction;
                    if (!(v.x < 0 || v.x >= size || v.y < 0 || v.y >= size))
                    {
                        tile.availableDirections.Add(direction);

                        idx += (int)Mathf.Pow(2, directions.IndexOf(direction));
                        Debug.Log(direction);
                    }
                }


                TileBoxies[y, x] = tile;
            }
        }

        Vector2Int nextPos;
        Tile4x4 currentTile = TileBoxies[size - 1, size / 2];

        currentTile.path_ID += 4;

        Stack<Tile4x4> visited_Tiles = new Stack<Tile4x4>();

        int visited_count = 1;
        int all_tile_count = size * size;

        // 완료될때까지 반복
        while (true)
        {
            currentTile.visited = true;
            for (int i = currentTile.availableDirections.Count - 1; i >= 0; i--)
            {
                var pos = currentTile.pos + currentTile.availableDirections[i];
                if (TileBoxies[pos.y, pos.x].visited)
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
                Debug.Log(currentTile.pos + "," + direction + "," + currentTile.path_ID);
                Debug.Log(currentTile.availableDirections.ToArray());
                currentTile = TileBoxies[nextPos.y, nextPos.x];
                currentTile.path_ID += (int)Mathf.Pow(2, directions.IndexOf(-direction));
                Debug.Log("reverse direction add" + currentTile.pos + ", " + currentTile.path_ID + "," + directions.IndexOf(-direction));
            }
        }

        Debug.Log("--------------");
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Tile4x4 tile = TileBoxies[y, x];

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

                        Debug.Log(v);
                        Tile4x4 next = TileBoxies[v.y, v.x];
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

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {

                Tile4x4 tile = TileBoxies[i, j];

                tile.SetFieldData(fieldDataSet[15 - tile.wall_ID]);
                //buttons[i * size + j].sprite = sprites[15 - tile.wall_ID];
                Debug.Log(tile.path_ID);
            }
        }
    }
    /*public bool IsInField(Vector2Int position)
    {
        if (position.x >= 0 &&
            position.y >= 0 &&
            position.y < size * 4 &&
            position.x < size * 4)
            return true;
        else
            return false;
    }
    public Tile GetField(int x, int y)
    {
        Vector2Int tilePosition = new Vector2Int(x, y);
        if (IsInField(tilePosition))
        {
            tilePosition.x = Mathf.Clamp(x, 0, size * 4);
            tilePosition.y = Mathf.Clamp(y, 0, size * 4);
        }

        Vector2Int index_tile4x4 = tilePosition / 4;
        Vector2Int index_field = tilePosition - tilePosition * index_tile4x4;

        return TileBoxies[index_tile4x4.y, index_tile4x4.x].GetField(index_field);
    }*/

    public FieldData GetFieldData()
    {
        FieldData result = new FieldData(size * 4, size * 4);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                result = FieldManager.instance.Merge2FieldData(result, TileBoxies[i, j].fieldData, new Vector2Int(j * 4, i * 4));
            }
        }
        FieldData platform = new FieldData(size * 4, 2);

        StringBuilder sb = new StringBuilder(platform.fieldStrData);
        sb[0] = 'U';
        sb[1] = 'S';
        sb[2] = 'U';
        sb[3] = 'S';
        sb[4] = 'U';
        sb[5] = 'S';
        sb[6] = 'U';
        sb[7] = 'S';
        //sb[8 * size - 3] = 'S';
        //sb[2 * size - 2] = 'U';
        //sb[2 * size - 1] = 'S';
        //sb[2 * size - 0] = 'U';
        //sb[2 * size + 1] = 'S';
        //sb[2 * size + 2] = 'U';
        //sb[2 * size + 3] = 'S';
        platform.fieldStrData = sb.ToString();
        result = FieldManager.instance.Merge2FieldData(result, platform,new Vector2Int(0, result.height));
        return result;
    }
    public Tile[,] GetAllFields()
    {
        int scale = size * 4;
        Tile[,] allTiles = new Tile[scale, scale];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {

                        allTiles[i * 4 + y, j * 4 + x] = TileBoxies[size - i - 1, j].field[3 - y, x];
                    }
                }
            }
        }
        return allTiles;
    }
}
