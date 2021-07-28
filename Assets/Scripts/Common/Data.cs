using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Skills;
using System;

namespace Common
{
    public class Data
    {
        private static string[] nameData;

        public static string[] NameData
        {
            get
            {
                if (nameData == null)
                {
                    TextAsset data = Resources.Load<TextAsset>("names");
                    nameData = data.text.Split('\n');
                }

                return nameData;
            }
        }

        /// <summary>
        /// 이름 데이터에서 랜덤하게 하나를 뽑습니다.
        /// </summary>
        /// <param name="seed">시드</param>
        /// <returns>이름</returns>
        public static string GetRandomName(int seed)
        {
            int rand = seed % NameData.Length;
            return NameData[rand];
        }

        /// <summary>
        /// 스킬 데이터에서 랜덤하게 하나를 뽑습니다.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="option">
        /// option 0 : 전체스킬 중에서 랜덤 스킬 반환
        /// option 1 : 이동스킬 중에서 랜덤 스킬 반환
        /// option 2 : 일반공격 중에서 랜덤 스킬 반환
        /// option 3 : 중급스킬 중에서 랜덤 스킬 반환
        /// option 4 : 고급스킬 중에서 랜덤 스킬 반환
        /// </param>
        /// <returns></returns>
        public static Skill GetRandomSkill(int seed, Skill.SkillCategory category = Skill.SkillCategory.Null)
        {
            Type type = skillData[seed % skillData.Count];
            Skill instance = Activator.CreateInstance(type) as Skill;

            return instance;
        }

        private static List<Type> skillData = new List<Type>()
        {
            Type.GetType("Model.Skills.S000_Cut"),
            Type.GetType("Model.Skills.S001_Snapshot"),
            Type.GetType("Model.Skills.S002_MagicArrow"),
            Type.GetType("Model.Skills.S003_Judgement"),
        };

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

        private static Sprite[] spritesData;

        public static Sprite[] SpriteData
        {
            get
            {
                if (spritesData == null)
                    spritesData = Resources.LoadAll<Sprite>("1bitpack_kenney_1/Tilesheet/colored_transparent_packed");
                return spritesData;
            }
        }

        public static Sprite GetRandomSprite(int seed)
        {
            int rand = seed % HumanSpriteNumbers.Length;
            return SpriteData[HumanSpriteNumbers[rand]];
        }

        private static int[] HumanSpriteNumbers = {
            23,24,25,26,27,28,29,30,
            71,72,73,74,75,76,77,78,
            119,120,121,122,123,124,125,126,
            167,168,169,170,171,172,173,174,
            215,216,217,218,219,220,221,222
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Sprite LoadSprite(string path)
        {
            Sprite sprite;
            if (path == "")
                sprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/X");
            else if (path.Contains("1bitpack_kenney_1/Tilesheet/colored_transparent_packed"))
            {
                string [] splited = path.Split('_');
                int spriteNumber = int.Parse(splited[splited.Length]);
                sprite = SpriteData[spriteNumber];
            }
            else
                sprite = Resources.Load<Sprite>(path);
            return sprite;
        }
    }
}