using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Skills.Move;
using Model.Skills.Basic;
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
            string name = NameData[rand];
            return name.Substring(0, name.Length - 1);
        }

        public static Skill GetRandomSkill(int seed, UnitSpecies species, SkillCategory category)
        {
            List<Skill> skills = new List<Skill>();
            
            foreach(Skill skill in allSkills)
                if (skill.species.Contains(species) && skill.Category == category)
                    skills.Add(skill);
            
            if (skills.Count == 0)
                return allSkills[0];

            int idx = seed % skills.Count;
            return skills[idx];
        }

        private static void InitSkillDictionary()
        {
            speciesToSkillList = new Dictionary<UnitSpecies, List<Skill>>()
            {
                { UnitSpecies.Human, new List<Skill>() },
                { UnitSpecies.Golem, new List<Skill>() },
                { UnitSpecies.SmallBeast, new List<Skill>() },
                { UnitSpecies.MediumBeast, new List<Skill>() },
                { UnitSpecies.LargeBeast, new List<Skill>() },
            };
            categoryToSkillList = new Dictionary<SkillCategory, List<Skill>>()
            {
                {SkillCategory.Move,   new List<Skill>()},
                {SkillCategory.Basic, new List<Skill>()},
                {SkillCategory.Intermediate, new List<Skill>()},
                {SkillCategory.Advanced, new List<Skill>()},
            };

            foreach (Skill skill in allSkills)
            {
                categoryToSkillList[skill.Category].Add(skill);
                foreach (UnitSpecies s in skill.species)
                    speciesToSkillList[s].Add(skill);
            }
        }

        private static Dictionary<UnitSpecies, List<Skill>> speciesToSkillList = null;

        public static Dictionary<UnitSpecies, List<Skill>> SpeciesToSkillList
        {
            get
            {
                if (speciesToSkillList == null)
                    InitSkillDictionary();
                return speciesToSkillList;
            }
        }

        private static Dictionary<SkillCategory, List<Skill>> categoryToSkillList = null;

        public static Dictionary<SkillCategory, List<Skill>> CategoryToSkillList
        {
            get
            {
                if (categoryToSkillList == null)
                    InitSkillDictionary();
                return categoryToSkillList;
            }
        }

        static List<Skill> allSkills = new List<Skill>()
        {
            // Move 스킬
            new Pawn(),
            new Knight(),
            new Bishop(),
            new Rook(),
            new Queen(),
            new King(),

            // Basic 스킬
            new Fireball(),
            new Heal(),
            new Scratch(),
            new Fireball(),
            new Snapshot(),

        };

        //인간
        //소형
        //중형
        //대형
        //골렘

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

        /// <summary>
        /// range data string을 제작한다.
        /// </summary>
        /// <param name="option">1 : 마름모, 2 : 십자가, 3 : 정사각형, 4 : X 자</param>
        /// <param name="range">중앙으로부터 확장되는 횟수</param>
        /// <returns></returns>
        public static string MakeRangeData(int option, int range)
        {
            // 가로세로 길이
            int size = 1 + range * 2;
            char[,] arr = new char[size, size];

            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                    arr[i,j] = '0';

            arr[range,range] = '1';

            if (option == 1) // 마름모꼴로 확장
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                    for (int j = 0; j < arr.GetLength(1); j++)
                        if (Math.Abs(range - i) + Math.Abs(range - j) <= range)
                            arr[i, j] = '1';
            }
            else if (option == 2) // 십자가꼴로 확장
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                    for (int j = 0; j < arr.GetLength(1); j++)
                        if (i == range || j == range)
                            arr[i, j] = '1';
            }
            else if (option == 3) // 정사각형꼴로 확장
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                    for (int j = 0; j < arr.GetLength(1); j++)
                        arr[i, j] = '1';
            }
            else if (option == 4) // X꼴로 확장
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                    for (int j = 0; j < arr.GetLength(1); j++)
                        if (i == j || i == (range * 2 - j))
                            arr[i, j] = '1';
            }
            // 배열을 하나의 문자열로 전환
            string str = "" + size;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                str += ";";
                for (int j = 0; j < arr.GetLength(1); j++)
                    str += arr[i,j];
            }

            return str;
        }

        private static Sprite[] colored_transparent_packed;
        private static Sprite[] monochrome_transparent_packed;

        public static Sprite[] Colored
        {
            get
            {
                if (colored_transparent_packed == null)
                    colored_transparent_packed = Resources.LoadAll<Sprite>("1bitpack_kenney_1/Tilesheet/colored_transparent_packed");
                return colored_transparent_packed;
            }
        }
        public static Sprite[] Monochrome
        {
            get
            {
                if (monochrome_transparent_packed == null)
                    monochrome_transparent_packed = Resources.LoadAll<Sprite>("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed");
                return monochrome_transparent_packed;
            }
        }

        public static Sprite GetRandomSprite(UnitSpecies unitSpecies, int seed)
        {
            int rand = seed % SpeciesToSpriteNumbers[unitSpecies].Count;
            return Colored[SpeciesToSpriteNumbers[unitSpecies][rand]];
        }

        private static Dictionary<UnitSpecies, List<int>> SpeciesToSpriteNumbers = new Dictionary<UnitSpecies, List<int>>
        {
            {
                UnitSpecies.Human,
                new List<int>
                {
                    23,24,25,26,27,28,29,30,
                    71,72,73,74,75,76,77,78,
                    119,120,121,122,123,124,125,126,
                    167,168,169,170,171,172,173,174,
                    215,216,217,218,219,220,221,222,
                    462
                }
            },
            {
                UnitSpecies.SmallBeast,
                new List<int>
                {
                    263,264,265,266,267,268,269,270,360,361,409,414
                }
            },
            {
                UnitSpecies.MediumBeast,
                new List<int>
                {
                    364,365,366,408,411
                }
            },
            {
                UnitSpecies.LargeBeast,
                new List<int>
                {
                    362,363,412,413
                }
            }
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
                string[] splited = path.Split('_');
                int spriteNumber = int.Parse(splited[splited.Length - 1]);
                sprite = Colored[spriteNumber];
            }
            else if (path.Contains("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed"))
            {
                string[] splited = path.Split('_');
                int spriteNumber = int.Parse(splited[splited.Length - 1]);
                sprite = Monochrome[spriteNumber];
            }
            else
                sprite = Resources.Load<Sprite>(path);
            return sprite;
        }

        
        public static Sprite MakeOutline(Sprite value, Color outline)
        {
            Texture2D old = value.texture;
            Rect rect = value.rect;
            Texture2D texture = new Texture2D(18,18);
            texture.filterMode = FilterMode.Point;

            for (int y = 0; y < 18; y++)
                for (int x = 0; x < 18; x++)
                    texture.SetPixel(x,y, new Color(0,0,0,0));

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x , (int)rect.y + y);

                    if (color.a == 0)
                        continue;

                    color = outline;
                    texture.SetPixel(x+2, y+1, color);
                    texture.SetPixel(x, y+1, color);
                    texture.SetPixel(x+1, y+2, color);
                    texture.SetPixel(x+1, y, color);
                }

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x , (int)rect.y + y);
                    if (color.a == 0)
                        continue;
                    texture.SetPixel(x+1, y+1, color);
                }

            texture.Apply();
            rect = new Rect(0, 0, texture.width, texture.height);

            return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 16);
        }

        public static Sprite MakeOutline(Sprite value, Color inline, Color outline)
        {
            Texture2D old = value.texture;
            Rect rect = value.rect;
            Texture2D texture = new Texture2D(18,18);
            texture.filterMode = FilterMode.Point;

            for (int y = 0; y < 18; y++)
                for (int x = 0; x < 18; x++)
                    texture.SetPixel(x,y, new Color(0,0,0,0));

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x , (int)rect.y + y);

                    if (color.a == 0)
                        continue;

                    color = outline;
                    texture.SetPixel(x+2, y+1, color);
                    texture.SetPixel(x, y+1, color);
                    texture.SetPixel(x+1, y+2, color);
                    texture.SetPixel(x+1, y, color);
                }

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x , (int)rect.y + y);
                    if (color.a == 0)
                        continue;
                    texture.SetPixel(x+1, y+1, inline);
                }

            texture.Apply();
            rect = new Rect(0, 0, texture.width, texture.height);

            return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 16);
        }
    }
}