// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Model;
// using static Model.Managers.FieldManager;
// using Model.Tiles;
// using Model.Managers;
// using System.Text;
// using Common;

// public class ChunkField
// {
//     /// <summary>
//     /// 인자로 받은 값에 따라 벽의 모양을 제작합니다.
//     /// 0이면 막힌거. 1이면 뚫린거.
//     /// </summary>
//     /// <param name="value">b0000 ~ b1111</param>
//     /// <returns></returns>
//     public FieldData MakeWallChunk(int value)
//     {
//         return new FieldData(4, 4,
//         (Random.Range(0, 2) == 0 ? "WL" : "FL") +
//         ((value & (int)Mathf.Pow(2, 0)) == 0 ? "WL" : "FL") +
//         ((value & (int)Mathf.Pow(2, 0)) == 0 ? "WL" : "FL") +
//         (Random.Range(0, 2) == 0 ? "WL" : "FL") +

//         ((value & (int)Mathf.Pow(2, 3)) == 0 ? "WL" : "FL") +
//         ("FL") +
//         ("FL") +
//         ((value & (int)Mathf.Pow(2, 1)) == 0 ? "WL" : "FL") +

//         ((value & (int)Mathf.Pow(2, 3)) == 0 ? "WL" : "FL") +
//         ("FL") +
//         ("FL") +
//         ((value & (int)Mathf.Pow(2, 1)) == 0 ? "WL" : "FL") +

//         (Random.Range(0, 2) == 0 ? "WL" : "FL") +
//         ((value & (int)Mathf.Pow(2, 2)) == 0 ? "WL" : "FL") +
//         ((value & (int)Mathf.Pow(2, 2)) == 0 ? "WL" : "FL") +
//         (Random.Range(0, 2) == 0 ? "WL" : "FL")
//         );
//     }

//     /// <summary>
//     /// 청크 맵 생성을 위한 4x4 타일 데이터
//     /// </summary>
//     class TileChunkData
//     {
//         // 청크에 포함된 2차원 타일 배열
//         public Tile[,] field;

//         // 청크 필드 데이터
//         public FieldData fieldData;

//         // 맵 생성 알고리즘 도중, 방문했음을 기록
//         public bool visited = false;

//         // 맵 생성 알고리즘에서 갈수 있는 방향을 기록
//         public List<Vector2Int> availableDirections = new List<Vector2Int>();

//         // 필드(청크의 모음)에서 청크가 위치한 곳
//         public Vector2Int pos;

//         // 생성자
//         public TileChunkData(int x, int y)
//         {
//             field = new Tile[4, 4];
//             pos = new Vector2Int(x, y);
//         }

//         // 필드 데이터 설정
//         public void SetFieldData(FieldData fieldData) => this.fieldData = fieldData;

//         /// <summary>
//         /// 벽이 없는 곳 기록용
//         /// b 0    0     0    0
//         /// b(왼)(아래)(오른)(위)
//         /// </summary>
//         public int path_ID = 0;

//         /// <summary>
//         /// 벽이 위치한 곳 기록용
//         /// 실제 사용 ID
//         /// </summary>
//         public int wall_ID = 0;
//     }


//     /// <summary>
//     /// 반 시계방향 순서의 4방위
//     /// </summary>
//     /// <typeparam name="Vector2Int"></typeparam>
//     /// <returns></returns>
//     List<Vector2Int> directions = new List<Vector2Int>()
//     {
//         Vector2Int.down,
//         Vector2Int.right,
//         Vector2Int.up,
//         Vector2Int.left
//     };


//     /// <summary>
//     /// 청크 맵을 생성한다.
//     /// </summary>
//     /// <param name="map_size"></param>
//     /// <returns></returns>
//     public FieldData GenerateChunkMap(int map_size = 3, bool isAttacked = false)
//     {
//         TileChunkData[,] chunkMap = new TileChunkData[map_size, map_size];

//         //청크 맵 초기화
//         InitChunkMap(chunkMap);

//         //미로 데이터 생성
//         GenerateMazeData(chunkMap);

//         //생성된 미로 데이터를 기반으로 벽의 다양성을 부여.
//         MakeRoughWall(chunkMap);

//         //생성된 미로 데이터를 기반으로 필드 채우기
//         for (int i = 0; i < map_size; i++)
//             for (int j = 0; j < map_size; j++)
//             {
//                 TileChunkData tile = chunkMap[i, j];
//                 /// 여기서 파일 불러와서 아이디 적용.
//                 tile.SetFieldData(MakeWallChunk(15 - tile.wall_ID));
//             }

//         FieldData wallData = MergeChunkDatasToFieldData(chunkMap);

//         int fieldSize = map_size * 4;

//         FieldData fieldData = new FieldData(fieldSize, fieldSize);

//         StringBuilder sb = new StringBuilder();

//         Vector2Int pos = new Vector2Int(Random.Range(0, 100), Random.Range(0, 100));
//         float scale = Random.Range(0.1f, 0.3f);
//         float intensity = GetPerlinNoiseIntensity(fieldSize, pos, scale, 20);

//         List<string> tiles = new List<string>(){
//             "TN", "HL", "PW", "WT"
//         };

//         string tile1 = tiles[Random.Range(0, tiles.Count)];
//         tiles.Remove(tile1);
//         string tile2 = tiles[Random.Range(0, tiles.Count)];

//         // 가시 타일 생성
//         for (int y = 0; y < fieldSize; y++)
//         {
//             for (int x = 0; x < fieldSize; x++)
//             {
//                 if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity) sb.Append(tile1);
//                 else sb.Append("FL");
//             }
//         }
//         fieldData.fieldStrData = sb.ToString();

//         char[] temp = wallData.fieldStrData.ToCharArray();

//         for (int i = 0; i < fieldSize * fieldSize; i++)
//         {
//             // char wc1 = wallData.fieldStrData[i*2];
//             // char wc2 = wallData.fieldStrData[i*2+1];

//             char fc1 = fieldData.fieldStrData[i * 2];
//             char fc2 = fieldData.fieldStrData[i * 2 + 1];

//             if ($"{fc1}{fc2}" != "FL")
//             {
//                 temp[i * 2] = fc1;
//                 temp[i * 2 + 1] = fc2;
//             }
//         }

//         #region 파워 타일 생성
//         sb = new StringBuilder();

//         pos = new Vector2Int(Random.Range(0, 100), Random.Range(0, 100));
//         scale = Random.Range(0.1f, 0.3f);
//         intensity = GetPerlinNoiseIntensity(fieldSize, pos, scale, 20);

//         for (int y = 0; y < fieldSize; y++)
//         {
//             for (int x = 0; x < fieldSize; x++)
//             {
//                 if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity) sb.Append(tile2);
//                 else sb.Append("FL");
//             }
//         }
//         fieldData.fieldStrData = sb.ToString();

//         for (int i = 0; i < fieldSize * fieldSize; i++)
//         {
//             // char wc1 = wallData.fieldStrData[i*2];
//             // char wc2 = wallData.fieldStrData[i*2+1];

//             char fc1 = fieldData.fieldStrData[i * 2];
//             char fc2 = fieldData.fieldStrData[i * 2 + 1];

//             if ($"{fc1}{fc2}" != "FL")
//             {
//                 temp[i * 2] = fc1;
//                 temp[i * 2 + 1] = fc2;
//             }
//         }

//         #endregion


//         #region Void 타일 생성
//         sb = new StringBuilder();

//         pos = new Vector2Int(Random.Range(0, 100), Random.Range(0, 100));
//         scale = Random.Range(0.1f, 0.3f);
//         intensity = GetPerlinNoiseIntensity(fieldSize, pos, scale, 20);

//         for (int y = 0; y < fieldSize; y++)
//         {
//             for (int x = 0; x < fieldSize; x++)
//             {
//                 if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity) sb.Append("HO");
//                 else sb.Append("FL");
//             }
//         }
//         fieldData.fieldStrData = sb.ToString();

//         for (int i = 0; i < fieldSize * fieldSize; i++)
//         {
//             char wc1 = wallData.fieldStrData[i * 2];
//             char wc2 = wallData.fieldStrData[i * 2 + 1];

//             char fc1 = fieldData.fieldStrData[i * 2];
//             char fc2 = fieldData.fieldStrData[i * 2 + 1];

//             if ($"{wc1}{wc2}" == "WL" && $"{fc1}{fc2}" == "HO")
//             {
//                 temp[i * 2] = fc1;
//                 temp[i * 2 + 1] = fc2;
//             }
//         }

//         #endregion

//         // 기습당하면 랜덤 위치에서 시작
//         if (isAttacked)
//         {
//             List<Vector2Int> randomPositions = new List<Vector2Int>();

//             while (randomPositions.Count < 4)
//             {
//                 Vector2Int tempPosition = new Vector2Int(Random.Range(0, fieldSize), Random.Range(0, fieldSize));
//                 if (!randomPositions.Contains(tempPosition))
//                 {
//                     randomPositions.Add(tempPosition);
//                     temp[(tempPosition.x + tempPosition.y * fieldSize) * 2 + 0] = 'S';
//                     temp[(tempPosition.x + tempPosition.y * fieldSize) * 2 + 1] = 'T';
//                 }
//             }
//         }
//         // 기습 안당하면 왼쪽 아래에서 시작
//         else
//         {
//             // 계단생성.
//             temp[fieldSize * (fieldSize - 2) * 2 + 0] = 'S';
//             temp[fieldSize * (fieldSize - 2) * 2 + 1] = 'T';
//             temp[fieldSize * (fieldSize - 2) * 2 + 2] = 'S';
//             temp[fieldSize * (fieldSize - 2) * 2 + 3] = 'T';
//             temp[fieldSize * (fieldSize - 1) * 2 + 0] = 'S';
//             temp[fieldSize * (fieldSize - 1) * 2 + 1] = 'T';
//             temp[fieldSize * (fieldSize - 1) * 2 + 2] = 'S';
//             temp[fieldSize * (fieldSize - 1) * 2 + 3] = 'T';
//         }

//         wallData.fieldStrData = new string(temp);

//         return wallData;
//     }


//     /// <summary>
//     /// 타일 박스를 초기화,
//     /// 순회 과정에서 외곽 막아버림
//     /// </summary>
//     /// <param name="chunk_map">맵, 청크 2차원 배열</param>
//     void InitChunkMap(TileChunkData[,] chunk_map)
//     {
//         int map_size = chunk_map.GetLength(0);

//         for (int y = 0; y < map_size; y++)
//         {
//             for (int x = 0; x < map_size; x++)
//             {
//                 TileChunkData tile = new TileChunkData(x, y);

//                 // 타일 위치와 4방위를 검사하여, 맵을 넘어가는 방향을 막아버린다.
//                 foreach (var direction in directions)
//                 {
//                     Vector2Int v = tile.pos + direction;
//                     if (!(v.x < 0 || v.x >= map_size || v.y < 0 || v.y >= map_size))
//                         tile.availableDirections.Add(direction);
//                 }
//                 chunk_map[y, x] = tile;
//             }
//         }
//     }

//     /// <summary>
//     /// 인자로 받은 chunk_map으로
//     /// </summary>
//     /// <param name="chunk_map"></param>
//     void GenerateMazeData(TileChunkData[,] chunk_map)
//     {
//         int map_size = chunk_map.GetLength(0);

//         Vector2Int nextPos;
//         TileChunkData currentTile = chunk_map[map_size - 1, 0];

//         // 입구와 연결되도록 길을 뚫어준다.
//         currentTile.path_ID += 4;

//         Stack<TileChunkData> visited_Tiles = new Stack<TileChunkData>();

//         int visited_count = 1;
//         int all_tile_count = map_size * map_size;

//         // 미로 생성 알고리즘. 완료될때까지 반복
//         while (true)
//         {
//             currentTile.visited = true;
//             for (int i = currentTile.availableDirections.Count - 1; i >= 0; i--)
//             {
//                 var pos = currentTile.pos + currentTile.availableDirections[i];
//                 if (chunk_map[pos.y, pos.x].visited)
//                     currentTile.availableDirections.RemoveAt(i);
//             }

//             //더 이상 갈 방향이 없을 때
//             if (currentTile.availableDirections.Count == 0)
//             {
//                 //모든 타일이 할당 되었을 때
//                 if (visited_count == all_tile_count)
//                     break; // end

//                 //이전 타일로 돌아감
//                 else
//                 {
//                     currentTile = visited_Tiles.Pop();
//                 }
//             }

//             //갈 수 있는 방향 중 하나를 선택.
//             else
//             {
//                 //방향 선택하고 현재 타일에서 해당 방향 제거

//                 Vector2Int direction = currentTile.availableDirections[Random.Range(0, currentTile.availableDirections.Count)];
//                 nextPos = currentTile.pos + direction;
//                 currentTile.availableDirections.Remove(direction);
//                 currentTile.path_ID += (int)Mathf.Pow(2, directions.IndexOf(direction));

//                 //stack에 push 후 타일 이동.
//                 visited_Tiles.Push(currentTile);
//                 visited_count++;

//                 //이전에 있던 타일로 가는 길 제거
//                 currentTile = chunk_map[nextPos.y, nextPos.x];
//                 currentTile.path_ID += (int)Mathf.Pow(2, directions.IndexOf(-direction));
//             }
//         }
//     }


//     void MakeRoughWall(TileChunkData[,] chunkMap)
//     {
//         int map_size = chunkMap.GetLength(0);

//         for (int y = 0; y < map_size; y++)
//         {
//             for (int x = 0; x < map_size; x++)
//             {
//                 TileChunkData tile = chunkMap[y, x];

//                 foreach (var direction in directions)
//                 {
//                     // 길이다 > 벽을 세우면 안됨
//                     // 길이 아니다. 
//                     //      그 방향에 타일이 없다.                      > 벽 있어도 되고 없어도 됨
//                     //      그 방향에 있는 타일이 벽을 세워놨다.        > 벽 있어도 되고 없어도 됨
//                     //      그 방향에 있는 타일이 아직 벽을 안세워놨다. > 벽을 세워야 함

//                     // 길이다 > 벽을 세우면 안됨
//                     if ((tile.path_ID & (int)Mathf.Pow(2, directions.IndexOf(direction))) != 0)
//                         continue;
//                     // 길이 아니다. 
//                     else
//                     {
//                         Vector2Int v = tile.pos + direction;

//                         //      그 방향에 타일이 없다.                      > 벽 있어도 되고 없어도 됨
//                         if (v.x < 0 || v.x >= map_size || v.y < 0 || v.y >= map_size)
//                         {
//                             // tile.wall_ID += Random.Range(0, 2) * (int)Mathf.Pow(2, directions.IndexOf(direction));
//                             continue;
//                         }

//                         TileChunkData next = chunkMap[v.y, v.x];
//                         //      그 방향에 있는 타일이 벽을 세워놨다.        > 벽 있어도 되고 없어도 됨
//                         if ((next.wall_ID & (int)Mathf.Pow(2, directions.IndexOf(-direction))) != 0)
//                         {
//                             // tile.wall_ID += Random.Range(0, 2) * (int)Mathf.Pow(2, directions.IndexOf(direction));
//                         }
//                         //      그 방향에 있는 타일이 아직 벽을 안세워놨다. > 벽을 세워야 함
//                         else
//                         {
//                             tile.wall_ID += (int)Mathf.Pow(2, directions.IndexOf(direction));
//                         }
//                     }
//                 }
//             }
//         }
//     }

//     public float GetPerlinNoiseIntensity(int fieldSize, Vector2Int pos, float scale, int wantCount)
//     {
//         float bestFit = 0;
//         int bestCount = 0;

//         for (float intensity = 0f; intensity < 1f; intensity += 0.024f)
//         {
//             int count = 0;
//             for (int y = 0; y < fieldSize; y++)
//             {
//                 for (int x = 0; x < fieldSize; x++)
//                 {
//                     if ((Mathf.PerlinNoise((x + pos.x) * scale, (y + pos.y) * scale)) < intensity)
//                         count++;
//                 }
//             }

//             if (Mathf.Abs(wantCount - bestCount) > Mathf.Abs(wantCount - count))
//             {
//                 bestFit = intensity;
//                 bestCount = count;
//             }
//         }

//         return bestFit;
//     }



//     FieldData MergeChunkDatasToFieldData(TileChunkData[,] chunkMap)
//     {
//         int map_size = chunkMap.GetLength(0);

//         FieldData result = new FieldData(map_size * 4, map_size * 4);

//         for (int i = 0; i < map_size; i++)
//         {
//             for (int j = 0; j < map_size; j++)
//             {
//                 result = FieldManager.instance.Merge2FieldData(result, chunkMap[i, j].fieldData, new Vector2Int(j * 4, i * 4));
//             }
//         }

//         return result;
//     }
// }
