using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using View;

namespace Model.Managers
{
    public class FieldManager : MonoBehaviour
    {
        public static FieldManager instance;

        public Tilemap tileMap;

        /// <summary>
        /// Tile.cs의 Category와 똑같이 사용해야 작동한다.
        /// </summary>
        public List<TileBase> tileBases;
        public List<char> tileBasesChar;

        // 현재 전투의 모든 타일
        public Tile[,] field;

        // 필드 데이터
        [TextArea(0, 20)]
        public List<string> FieldDatas = new List<string>()
        {
            "WWWWWWWWWWWWWWWW\n" +
            "WWWWWWWWWWWWWWWW\n" +
            "WWWWWWWWWWWWWWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWFSFFFFFFFFWWW\n" +
            "WWWFFFFFFFFFFWWW\n" +
            "WWWWWWWWWWWWWWWW\n" +
            "WWWWWWWWWWWWWWWW\n" +
            "WWWWWWWWWWWWWWWW"
            ,
            "WWWWWWWWWWWWWWWW\n" +
            "WWWWWWWWWWWWWWWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWTTTTTTTTTTTTWW\n" +
            "WWTTTTTTTTTTTTWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWFFFFFTTFFFFFWW\n" +
            "WWWWWWWWWWWWWWWW\n" +
            "WWWWWWWWWWWWWWWW"
        };

        public void InitField(string fieldData)
        {
            char[] chars = fieldData.ToCharArray();

            // 열과 행
            int col = 0, row = 1;

            // 개행의 개수를 세줍니다.
            for (int i = 0; i < fieldData.Length; i++)
                if (chars[i] == '\n')
                    row++;

            // 전체 길이를 row로 나눈거를 올림한다. \n이 있으므로 값을 하나 빼주면 열의 개수이다.
            col = (fieldData.Length + row - 1) / row - 1;

            field = new Tile[row, col];

            int x = 0, y = row - 1;

            for (int i = 0; i < fieldData.Length; i++)
            {
                if (chars[i] == '\n')
                {
                    x = 0;
                    y--;
                }
                else
                {
                    field[y, x] = chars[i] == 'D' ?  new DownStairTIle() : new Tile();
                    field[y, x].position = new Vector2Int(x,y);
                    field[y, x].category = (Tile.Category)chars[i];
                    x++;
                }
            }

            UpdateTileMap();
        }

        public void UpdateTileMap()
        {
            for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    char c = (char)field[y, x].category;
                    int i = tileBasesChar.IndexOf(c);
                    tileMap.SetTile(new Vector3Int(x, y, 0), tileBases[i]);
                }
            }
        }

        public static bool IsInField(Vector2Int position)
        {
            if (position.x >= 0 &&
                position.y >= 0 &&
                position.x < instance.field.GetLength(0) &&
                position.y < instance.field.GetLength(1))
                return true;
            else
                return false;
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
                x = Mathf.Clamp(x, 0, instance.field.GetLength(0));
                y = Mathf.Clamp(y, 0, instance.field.GetLength(1));
                return instance.field[y, x];
            }
        }

        public static Tile[,] GetField()
        {
            return instance.field;
        }

        // 필드에서 position에 가까운 빈(Unit과 Obatainable이 없는) 바닥 타일을 가져옵니다.
        public static List<Tile> GetBlankFloorTiles(Vector2Int position, int count)
        {
            List<Tile> tiles = new List<Tile>();

            return tiles;
        }


        // 필드에서 랜덤한 빈 타일을 가져옵니다.
        public static List<Tile> GetBlankFloorTiles(int count)
        {
            List<Tile> allBlankTiles = new List<Tile>();
            List<Tile> tiles = new List<Tile>();

            foreach (Tile tile in GetField())
                if (tile.IsObtainablePositionable())
                    allBlankTiles.Add(tile);

            for (int i = 0; i < count; i++)
            {
                if (allBlankTiles.Count == 0)
                    return tiles;

                Tile temp = allBlankTiles[Random.Range(0, allBlankTiles.Count)];
                allBlankTiles.Remove(temp);
                tiles.Add(temp);
            }

            return tiles;
        }

        public List<Vector2Int> GetStairAroundPosition()
        {
            List<Vector2Int> around = new List<Vector2Int>()
            {
                new Vector2Int(-1, -1),
                new Vector2Int(-1,  0),
                new Vector2Int(-1,  1),
                new Vector2Int( 0, -1),
                new Vector2Int( 0,  0),
                new Vector2Int( 0,  1),
                new Vector2Int( 1, -1),
                new Vector2Int( 1,  0),
                new Vector2Int( 1,  1)
            };

            List<Vector2Int> StairAroundPosition = new List<Vector2Int>();

            Vector2Int stairPosition = GetStairPosition();
            foreach (var vector in around)
            {
                Vector2Int position = stairPosition + vector;

                if (StairAroundPosition.Contains(position) == false &&
                    field[position.y, position.x].category != Model.Tile.Category.Hole &&
                    field[position.y, position.x].category != Model.Tile.Category.Wall &&
                    field[position.y, position.x].HasUnit() == false)
                    StairAroundPosition.Add(position);
                //else
                //    Debug.LogError($"{x},{y} + {vector}");
            }
            return StairAroundPosition;
        }

        public Vector2Int GetStairPosition()
        {
             for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    if (field[y, x].category == Model.Tile.Category.UpStair)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return Vector2Int.zero;
        }

        private void Awake()
        {
            instance = this;
            tileMap = GetComponentInChildren<Tilemap>();
        }
    }

}
