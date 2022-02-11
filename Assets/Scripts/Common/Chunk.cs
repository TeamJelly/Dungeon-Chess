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
    Dictionary<int, List<FieldData>> FieldDataDictionary
    {
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


    /// <summary>
    /// 0이면 막힌거. 1이면 뚫린거.
    /// </summary>
    /// <param name="value">b0000 ~ b1111</param>
    /// <returns></returns>
    public FieldData GetWallChunk(int value)
    {
        return new FieldData(4, 4,
        (Random.Range(0, 2) == 0 ? "WL" : "FR") +
        ((value & (int)Mathf.Pow(2, 0)) == 0 ? "WL" : "FR") +
        ((value & (int)Mathf.Pow(2, 0)) == 0 ? "WL" : "FR") +
        (Random.Range(0, 2) == 0 ? "WL" : "FR") +

        ((value & (int)Mathf.Pow(2, 3)) == 0 ? "WL" : "FR") +
        ("FR") +
        ("FR") +
        ((value & (int)Mathf.Pow(2, 1)) == 0 ? "WL" : "FR") +

        ((value & (int)Mathf.Pow(2, 3)) == 0 ? "WL" : "FR") +
        ("FR") +
        ("FR") +
        ((value & (int)Mathf.Pow(2, 1)) == 0 ? "WL" : "FR") +

        (Random.Range(0, 2) == 0 ? "WL" : "FR") +
        ((value & (int)Mathf.Pow(2, 2)) == 0 ? "WL" : "FR") +
        ((value & (int)Mathf.Pow(2, 2)) == 0 ? "WL" : "FR") +
        (Random.Range(0, 2) == 0 ? "WL" : "FR")
        );
    }

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

    int map_size = 3;

    void InitTileBoxies(Tile4x4[,] tileBoxies)
    {
        for (int y = 0; y < map_size; y++)
        {
            for (int x = 0; x < map_size; x++)
            {
                Tile4x4 tile = new Tile4x4(x, y);

                int idx = 0;
                foreach (var direction in directions)
                {
                    Vector2Int v = tile.pos + direction;
                    if (!(v.x < 0 || v.x >= map_size || v.y < 0 || v.y >= map_size))
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
        Tile4x4 currentTile = tileBoxies[map_size - 1, 0];

        // 입구와 연결되도록 길을 뚫어준다.
        currentTile.path_ID += 4;

        Stack<Tile4x4> visited_Tiles = new Stack<Tile4x4>();

        int visited_count = 1;
        int all_tile_count = map_size * map_size;

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
        for (int y = 0; y < map_size; y++)
        {
            for (int x = 0; x < map_size; x++)
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
                        if (v.x < 0 || v.x >= map_size || v.y < 0 || v.y >= map_size)
                        {
                            // tile.wall_ID += Random.Range(0, 2) * (int)Mathf.Pow(2, directions.IndexOf(direction));
                            continue;
                        }

                        Tile4x4 next = tileBoxies[v.y, v.x];
                        //      그 방향에 있는 타일이 벽을 세워놨다.        > 벽 있어도 되고 없어도 됨
                        if ((next.wall_ID & (int)Mathf.Pow(2, directions.IndexOf(-direction))) != 0)
                        {
                            // tile.wall_ID += Random.Range(0, 2) * (int)Mathf.Pow(2, directions.IndexOf(direction));
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

    public float GetPerlinNoiseIntensity(int fieldSize, Vector2Int pos, float scale, int wantCount)
    {
        float bestFit = 0;
        int bestCount = 0;

        for (float intensity = 0f; intensity < 1f; intensity += 0.024f)
        {
            int count = 0;
            for (int y = 0; y < fieldSize; y++)
            {
                for (int x = 0; x < fieldSize; x++)
                {
                    if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity)
                        count++;
                }
            }

            if (Mathf.Abs(wantCount - bestCount) > Mathf.Abs(wantCount - count))
            {
                bestFit = intensity;
                bestCount = count;
            }
        }

        return bestFit;
    }

    public FieldData GenerateMap()
    {
        FieldData wallData = GenerateWall();        

        int fieldSize = map_size * 4;

        FieldData fieldData = new FieldData(fieldSize, fieldSize);

        StringBuilder sb = new StringBuilder();

        Vector2Int pos = new Vector2Int(Random.Range(0,100),Random.Range(0,100));
        float scale = Random.Range(0.1f, 0.3f);
        float intensity = GetPerlinNoiseIntensity(fieldSize, pos, scale, 20);

        List<string> tiles = new List<string>(){
            "TN", "HL", "PW"
        };

        string tile1 = tiles[Random.Range(0,tiles.Count)];

        tiles.Remove(tile1);

        string tile2 = tiles[Random.Range(0,tiles.Count)];

        // 가시 타일 생성
        for (int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity) sb.Append(tile1);
                else sb.Append("FR");
            }
        }
        fieldData.fieldStrData = sb.ToString();

        char[] temp = wallData.fieldStrData.ToCharArray();

        for (int i = 0; i < fieldSize * fieldSize; i++)
        {
            // char wc1 = wallData.fieldStrData[i*2];
            // char wc2 = wallData.fieldStrData[i*2+1];

            char fc1 = fieldData.fieldStrData[i*2];
            char fc2 = fieldData.fieldStrData[i*2+1];

            if ($"{fc1}{fc2}" != "FR")
            {
                temp[i*2] = fc1;
                temp[i*2+1] = fc2;
            }
        }

        #region 파워 타일 생성
        sb = new StringBuilder();

        pos = new Vector2Int(Random.Range(0,100),Random.Range(0,100));
        scale = Random.Range(0.1f, 0.3f);
        intensity = GetPerlinNoiseIntensity(fieldSize, pos, scale, 20);

        for (int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity) sb.Append(tile2);
                else sb.Append("FR");
            }
        }
        fieldData.fieldStrData = sb.ToString();

        for (int i = 0; i < fieldSize * fieldSize; i++)
        {
            // char wc1 = wallData.fieldStrData[i*2];
            // char wc2 = wallData.fieldStrData[i*2+1];

            char fc1 = fieldData.fieldStrData[i*2];
            char fc2 = fieldData.fieldStrData[i*2+1];

            if ($"{fc1}{fc2}" != "FR")
            {
                temp[i*2] = fc1;
                temp[i*2+1] = fc2;
            }
        }

        #endregion

        
        #region Void 타일 생성
        sb = new StringBuilder();

        pos = new Vector2Int(Random.Range(0,100),Random.Range(0,100));
        scale = Random.Range(0.1f, 0.3f);
        intensity = GetPerlinNoiseIntensity(fieldSize, pos, scale, 20);

        for (int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity) sb.Append("VO");
                else sb.Append("FR");
            }
        }
        fieldData.fieldStrData = sb.ToString();

        for (int i = 0; i < fieldSize * fieldSize; i++)
        {
            char wc1 = wallData.fieldStrData[i*2];
            char wc2 = wallData.fieldStrData[i*2+1];

            char fc1 = fieldData.fieldStrData[i*2];
            char fc2 = fieldData.fieldStrData[i*2+1];

            if ($"{wc1}{wc2}" == "WL" && $"{fc1}{fc2}" == "VO")
            {
                temp[i*2] = fc1;
                temp[i*2+1] = fc2;
            }
        }

        #endregion

        temp[fieldSize * (fieldSize - 1) * 2 + 0] = 'U';
        temp[fieldSize * (fieldSize - 1) * 2 + 1] = 'S';
        temp[fieldSize * (fieldSize - 1) * 2 + 2] = 'U';
        temp[fieldSize * (fieldSize - 1) * 2 + 3] = 'S';
        temp[fieldSize * (fieldSize - 1) * 2 + 4] = 'U';
        temp[fieldSize * (fieldSize - 1) * 2 + 5] = 'S';
        temp[fieldSize * (fieldSize - 1) * 2 + 6] = 'U';
        temp[fieldSize * (fieldSize - 1) * 2 + 7] = 'S';

        wallData.fieldStrData = new string(temp);

        return wallData;
    }

    public FieldData GenerateWall()
    {

        Tile4x4[,] tileBoxies1 = new Tile4x4[map_size, map_size];
        // Tile4x4[,] tileBoxies2 = new Tile4x4[size, size];

        //타일 생성
        InitTileBoxies(tileBoxies1);
        // InitTileBoxies(tileBoxies2);

        //미로 데이터 생성
        GenerateMazeData(tileBoxies1);
        // GenerateMazeData(tileBoxies2);

        //두개의 미로 데이터를 이용해서 길 두번 뚫기.
        // for (int y = 0; y < size; y++)
        // {
        //     for (int x = 0; x < size; x++)
        //     {
        //         Tile4x4 tile1 = tileBoxies1[y, x];
        //         Tile4x4 tile2 = tileBoxies2[y, x];
        //         tile1.path_ID |= tile2.path_ID;
        //     }
        // }

        //생성된 미로 데이터를 기반으로 벽의 다양성을 부여.
        MakeRoughWall(tileBoxies1);

        //생성된 미로 데이터를 기반으로 필드 채우기
        for (int i = 0; i < map_size; i++)
        {
            for (int j = 0; j < map_size; j++)
            {
                Tile4x4 tile = tileBoxies1[i, j];

                /// 여기서 파일 불러와서 아이디 적용.

                tile.SetFieldData(GetWallChunk(15 - tile.wall_ID));
            }
        }

        return GetFieldData(tileBoxies1);
    }
    

    FieldData GetFieldData(Tile4x4[,] tileBoxies)
    {
        FieldData result = new FieldData(map_size * 4, map_size * 4);

        for (int i = 0; i < map_size; i++)
        {
            for (int j = 0; j < map_size; j++)
            {
                result = FieldManager.instance.Merge2FieldData(result, tileBoxies[i, j].fieldData, new Vector2Int(j * 4, i * 4));
            }
        }

        // FieldData platform = new FieldData(map_size * 4, 1, 'W', 'L');

        // StringBuilder sb = new StringBuilder(platform.fieldStrData);
        // sb[0] = 'U';
        // sb[1] = 'S';
        // sb[2] = 'U';
        // sb[3] = 'S';
        // sb[4] = 'U';
        // sb[5] = 'S';
        // sb[6] = 'U';
        // sb[7] = 'S';

        // platform.fieldStrData = sb.ToString();
        // result = FieldManager.instance.Merge2FieldData(result, platform, new Vector2Int(0, result.height));
        return result;
    }
}
