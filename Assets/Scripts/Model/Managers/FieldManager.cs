using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Model.Tiles;

namespace Model.Managers
{
    public class FieldManager : MonoBehaviour
    {
        public static FieldManager instance;
        public Tilemap tileMap;

        // /// <summary>
        // /// Tile.cs의 Category와 똑같이 사용해야 작동한다.
        // /// </summary>
        // public List<TileBase> tileBases;
        // public List<char> tileBasesChar;

        // 현재 전투의 모든 타일
        public Tile[,] field;

        public List<Vector2Int> allTilesPosition 
        {
            get
            {
                List<Vector2Int> positions = new List<Vector2Int>();

                if (fieldData != null)
                    for (int y = 0; y < fieldData.height; y++)
                        for (int x = 0; x < fieldData.width; x++)
                            positions.Add(new Vector2Int(x, y));

                return positions;
            }
        }

        [Serializable]
        public class FieldData
        {
            public FieldData(int width, int height, string str)
            {
                this.width = width;
                this.height = height;
                this.fieldStrData = str;
            }

            // 행과 열의 개수
            public int width, height;
            public string fieldStrData;

            public void ToJson(FieldData fieldData)
            {
                string jsonStr = JsonUtility.ToJson(fieldData);
                Debug.Log(jsonStr);
            }
        }

        FieldData fieldData;

        public void InitField(FieldData fieldData)
        {
            ScreenTouchManager.instance.RightUpLimit = new Vector2(fieldData.width, fieldData.height);

            this.fieldData = fieldData;

            char[] chars = fieldData.fieldStrData.ToCharArray();

            //Dictionary<Vector2Int, chunk>

            field = new Tile[fieldData.height, fieldData.width];

            int index = 0;

            for (int y = fieldData.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < fieldData.width; x++)
                {
                    while (chars[index] == ' ' || chars[index] == '\n')
                        index++;
                    char c1 = chars[index];

                    index++;

                    while (chars[index] == ' ' || chars[index] == '\n')
                        index++;
                    char c2 = chars[index];

                    if ($"{c1}{c2}" == "FR")        // Floor
                        field[y,x] = new Floor();
                    else if ($"{c1}{c2}" == "WL")   // Wall
                        field[y,x] = new Wall();
                    else if ($"{c1}{c2}" == "US")   // UpStair
                        field[y,x] = new UpStair();
                    else if ($"{c1}{c2}" == "DS")   // DownStair
                        field[y,x] = new DownStair();
                    else if ($"{c1}{c2}" == "TN")   // Thorn
                        field[y,x] = new Thorn();
                    else if ($"{c1}{c2}" == "VO")   // Void
                        field[y,x] = new Model.Tiles.Hole();
                    else if ($"{c1}{c2}" == "SL")   // Sell
                        field[y,x] = new Sell();
                    else if ($"{c1}{c2}" == "HL")   // Heal
                        field[y,x] = new Heal();
                    else if ($"{c1}{c2}" == "PW")   // Power
                        field[y,x] = new Power();
                    else if ($"{c1}{c2}" == "LK")   // Locked
                        field[y,x] = new Locked();
                    else if ($"{c1}{c2}" == "UL")   // UnLocked
                        field[y,x] = new UnLocked();
                    else
                        field[y,x] = new Floor();

                    field[y, x].position = new Vector2Int(x, y);
                    index++;
                }
            }
            

            UpdateTileMap();
        }

        public void UpdateTileMap()
        {
            for (int y = 0; y < field.GetLength(0); y++)
                for (int x = 0; x < field.GetLength(1); x++)
                    tileMap.SetTile(new Vector3Int(x, y, 0), field[y,x].TileBase);
        }

        public void UpdateTile(Tile tile)
        {
            tileMap.SetTile(new Vector3Int(tile.position.x, tile.position.y, 0), tile.TileBase);
        }
        
        public static bool IsInField(Vector2Int position)
        {
            if (position.x >= 0 &&
                position.y >= 0 &&
                position.y < instance.field.GetLength(0) &&
                position.x < instance.field.GetLength(1))
                return true;
            else
                return false;
        }
        
        public List<Vector2Int> GetAllPositions()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            for (int y = 0; y < field.GetLength(0); y++)
                for (int x = 0; x < field.GetLength(1); x++)
                    positions.Add(new Vector2Int(x,y));
            return positions;
        }

        public static Tile GetTile(Vector2Int position)
        {
            return GetTile(position.x, position.y);
        }

        public static Tile GetTile(int x, int y)
        {
            if (IsInField(new Vector2Int(x, y)))
                return instance.field[y, x];
            else
            {
                //맵 범위 안으로 값 조정
                x = Mathf.Clamp(x, 0, instance.field.GetLength(1));
                y = Mathf.Clamp(y, 0, instance.field.GetLength(0));
                return instance.field[y, x];
            }
        }

        public static Tile[,] GetField()
        {
            return instance.field;
        }

        // 필드에서 랜덤한 빈 타일을 가져옵니다.
        public static List<Tile> GetBlankFloorTiles(int count)
        {
            List<Tile> allBlankTiles = new List<Tile>();
            List<Tile> tiles = new List<Tile>();

            foreach (Tile tile in GetField())
                if (tile.IsItemPositionable())
                    allBlankTiles.Add(tile);

            for (int i = 0; i < count; i++)
            {
                if (allBlankTiles.Count == 0)
                    return tiles;

                Tile temp = allBlankTiles[UnityEngine.Random.Range(0, allBlankTiles.Count)];
                allBlankTiles.Remove(temp);
                tiles.Add(temp);
            }

            return tiles;
        }

        // public List<Vector2Int> GetStairAroundPosition()
        // {
        //     List<Vector2Int> around = new List<Vector2Int>()
        //     {
        //         new Vector2Int(-1, -1),
        //         new Vector2Int(-1,  0),
        //         new Vector2Int(-1,  1),
        //         new Vector2Int( 0, -1),
        //         new Vector2Int( 0,  0),
        //         new Vector2Int( 0,  1),
        //         new Vector2Int( 1, -1),
        //         new Vector2Int( 1,  0),
        //         new Vector2Int( 1,  1)
        //     };

        //     List<Vector2Int> StairAroundPosition = new List<Vector2Int>();

        //     Vector2Int stairPosition = GetStairPosition();

        //     Debug.Log(stairPosition);


        //     foreach (var vector in around)
        //     {
        //         Vector2Int position = stairPosition + vector;

        //         // Debug.Log(position + " " + FieldManager.IsInField(position));

        //         if (FieldManager.IsInField(position) == true &&
        //             StairAroundPosition.Contains(position) == false &&
        //             field[position.y, position.x].category != Model.TileCategory.Hole &&
        //             field[position.y, position.x].category != Model.TileCategory.Wall &&
        //             field[position.y, position.x].category != Model.TileCategory.Locked &&
        //             field[position.y, position.x].HasUnit() == false)
        //         {
        //             StairAroundPosition.Add(position);
        //         }
        //         //else
        //         //    Debug.LogError($"{x},{y} + {vector}");
        //     }
        //     return StairAroundPosition;
        // }

        // public Vector2Int GetStairPosition()
        // {
        //     for (int y = 0; y < field.GetLength(0); y++)
        //     {
        //         for (int x = 0; x < field.GetLength(1); x++)
        //         {
        //             if (field[y, x].category == Model.TileCategory.UpStair)
        //             {
        //                 return new Vector2Int(x, y);
        //             }
        //         }
        //     }
        //     return Vector2Int.zero;
        // }

        public List<Vector2Int> GetTileCategoryPositions(Model.TileCategory category)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            for (int y = 0; y < field.GetLength(0); y++)
                for (int x = 0; x < field.GetLength(1); x++)
                    if (field[y, x].category == category)
                        positions.Add(field[y,x].position);

            return positions;
        }

        private void Awake()
        {
            instance = this;
            tileMap = GetComponentInChildren<Tilemap>();
        }
    }

}
