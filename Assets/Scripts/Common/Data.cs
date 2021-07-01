using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class Data
    {
        /// <summary>
        /// 범위 스키마를 범위 리스트로 해석합니다.
        /// </summary>
        /// <param name="data">범위 스키마</param>
        /// <param name="list">범위 리스트</param>
        public static List<Vector2Int> ParseRangeData(string data)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            var rows = data.Split(';');
            var size = int.Parse(rows[0]);
            var mid = size / 2;
            for (int i = 1; i < rows.Length; i++)
            {
                int y = mid - i + 1;
                for (int j = 0; j < size; j++)
                {
                    if (rows[i][j] == '1')
                    {
                        int x = -mid + j;
                        list.Add(new Vector2Int(x, y));
                    }
                }
            }
            return list;
        }

        public static Sprite LoadSprite(string path)
        {
            Sprite sprite;
            if (path == "")
                sprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/X");
            else
                sprite = Resources.Load<Sprite>(path);
            return sprite;
        }
    }
}