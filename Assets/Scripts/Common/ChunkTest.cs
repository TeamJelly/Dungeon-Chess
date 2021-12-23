using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChunkTest : MonoBehaviour
{
    public class Tile4x4
    {
        public Tile4x4(int id) { ID = id; }
        public bool visited = false;
        public int ID;
        //public TileBox up = null;
        //public TileBox down = null;
        //public TileBox left = null;
        //public TileBox right = null;
        public List<Vector2Int> availableDirections = new List<Vector2Int>();

        public Vector2Int pos;
        public int path_ID = 0;
        public int wall_ID = 0; 
    }

    public bool isClear = false;
    public Image[] buttons;
    public Sprite[] sprites;
    public Tile4x4[,] TileBoxies;
    // Start is called before the first frame update

    int size = 3;
    public void Start()
    {
        GenerateMap();

        /*for (int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
               // Debug.Log(TileBoxs[i, j].ID + "===============");
                if(i > 0)
                {
                    TileBoxies[i,j].up = TileBoxies[i - 1, j];
                   // Debug.Log(TileBoxs[i, j].up.ID);
                }
                if(i < 3)
                {
                    TileBoxies[i, j].down = TileBoxies[i + 1, j];
                  //  Debug.Log(TileBoxs[i, j].down.ID);
                }
                if (j > 0)
                {
                    TileBoxies[i, j].left = TileBoxies[i, j - 1];
                  //  Debug.Log(TileBoxs[i, j].left.ID);
                }
                if (j < 3)
                {
                    TileBoxies[i, j].right = TileBoxies[i, j + 1];
                  //  Debug.Log(TileBoxs[i, j].right.ID);
                }
                TileBox tempTileBox = TileBoxies[i, j];
                //buttons[i * size + j].GetComponent<Button>().onClick.AddListener(() => ChangeTileBox(tempTileBox));

                int r = Random.Range(0, 6);
                int rot = Random.Range(0, size) * 90;
                buttons[i * size + j].sprite = sprites[r];
                buttons[i * size + j].transform.rotation = new Quaternion(0, 0, rot, 0);
            }
        }*/
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
                Tile4x4 tile = new Tile4x4(y * size + x);
                tile.pos = new Vector2Int(x, y);

                Debug.Log("tile" + tile.pos);

                int idx = 0;
                foreach (var direction in directions)
                {
                    Vector2Int v = tile.pos + direction;
                    if (!(v.x < 0 || v.x >= size || v.y < 0 || v.y >= size))
                    {
                        tile.availableDirections.Add(direction);

                        idx += (int)Mathf.Pow(2,directions.IndexOf(direction));
                        Debug.Log(direction);
                    }
                }
                //tile.spriteID = idx;


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
            for (int i = currentTile.availableDirections.Count-1; i >= 0; i--)
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

                buttons[currentTile.pos.y * size + currentTile.pos.x].sprite = sprites[currentTile.path_ID];

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
                buttons[i * size + j].sprite = sprites[15 - tile.wall_ID];
                Debug.Log(tile.path_ID);
            }
        }
    }

    /*public void ChangeTileBox(TileBox TileBox)
    {
        TileBox emptyTileBox = null;
        if(TileBox.up != null && TileBox.up.isEmpty)
        {
            emptyTileBox = TileBox.up;
        }
        if (TileBox.down != null && TileBox.down.isEmpty)
        {
            emptyTileBox = TileBox.down;
        }
        if (TileBox.left != null && TileBox.left.isEmpty)
        {
            emptyTileBox = TileBox.left;
        }
        if (TileBox.right != null && TileBox.right.isEmpty)
        {
            emptyTileBox = TileBox.right;
        }
        //Debug.Log(TileBox.ID);
        if (emptyTileBox != null)
        {
            Sprite tempImg = buttons[TileBox.ID].sprite;
            buttons[TileBox.ID].sprite = buttons[emptyTileBox.ID].sprite;
            buttons[emptyTileBox.ID].sprite = tempImg;

            //int tempID = TileBox.ID;
            //TileBox.ID = emptyTileBox.ID;
            //emptyTileBox.ID = tempID;

            TileBox.isEmpty = true;
            emptyTileBox.isEmpty = false;
            
        }
        
    }*/
}
