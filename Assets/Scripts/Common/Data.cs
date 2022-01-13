using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Skills.Move;
using Model.Skills.Basic;
using static Model.Managers.FieldManager;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace Common
{
    public class Data
    {


        public static FieldData LoadFieldData()
        {
            try
            {
                TextAsset[] Starts = Resources.LoadAll<TextAsset>("/Data/Field/Start");
                TextAsset[] Neutrals = Resources.LoadAll<TextAsset>("/Data/Field/Neutral");
                TextAsset[] Ends = Resources.LoadAll<TextAsset>("/Data/Field/End");

                string jsonStr = File.ReadAllText(Application.dataPath + "/Resources/Data/Field/Data.json");

                jsonStr = jsonStr.Replace("\n", "");

                Debug.Log("load" + jsonStr);

                return JsonUtility.FromJson<FieldData>(jsonStr);
            }
            catch
            {
                // Debug.Log("no save file in path");
                // return null;

                FieldData temp = new FieldData(
                    16, 16,
                    "WL WL WL WL WL WL WL WL WL WL WL WL WL WL WL WL \n" +
                    "WL PW HL PW PW FR FR FR FR FR SL SL SL LK WL WL \n" +
                    "WL PW PW HL PW FR FR FR FR FR SL SL SL LK FR WL \n" +
                    "WL FR HL FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
                    "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
                    "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
                    "WL TN TN TN TN TN TN TN TN TN TN TN TN TN TN WL \n" +
                    "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
                    "WL FR FR FR FR FR WL WL FR FR FR FR FR FR FR WL \n" +
                    "WL FR FR FR FR FR WL WL FR FR FR FR FR FR FR WL \n" +
                    "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
                    "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
                    "WL FR FR FR FR FR FR FR FR FR FR FR FR FR FR WL \n" +
                    "WL US US FR FR DS DS FR FR FR FR FR FR FR FR WL \n" +
                    "WL US US FR FR DS DS FR FR FR FR FR FR FR FR WL \n" +
                    "WL WL WL WL WL WL WL WL WL WL WL WL WL WL WL WL \n"
                );

                SaveFieldData(temp);

                return temp;
            }
        }

        public static void SaveFieldData(FieldData fieldData)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Data/Field/");
            if (!directoryInfo.Exists) directoryInfo.Create();

            string jsonStr = JsonUtility.ToJson(fieldData);

            jsonStr = jsonStr.Replace("\\n", "\n");
            Debug.Log("json Replace " + jsonStr);

            File.WriteAllText(Application.dataPath + "/Resources/Data/Field/Data.json", jsonStr);
        }

        //유닛 저장 불러오기 실험중
        //현재 객체 자체를 바이너리 직렬화로 저장 시도
        //Vector2Int 등 유니티 기본 타입들 serializable이 아니라서 불가능.
        public static void Save_Unit_Serializable_Data(Unit unit)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Data/Unit/");
            if (!directoryInfo.Exists) directoryInfo.Create();

            string jsonStr = JsonUtility.ToJson(unit.Get_Serializable());
            File.WriteAllText(Application.dataPath + "/Resources/Data/Unit/" + unit.Name + ".json", jsonStr);


            /*BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.dataPath + "/Resources/Data/Unit/" + unit.Name + ".unit");
            bf.Serialize(file, unit);
            file.Close();*/
        }
        public static Unit_Serializable Load_Unit_Serializable_Data(string dataPath)
        {

            string jsonStr = File.ReadAllText(dataPath);

            jsonStr = jsonStr.Replace("\n", "");
            Unit_Serializable u = JsonUtility.FromJson<Unit_Serializable>(jsonStr);
            return u;
            //Unit unit = new Unit();
            //unit.Set_From_Serializable(u);
            /*
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Data/Unit/");
            if (!directoryInfo.Exists) directoryInfo.Create();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Resources/Data/Unit/" + name, FileMode.Open);*/

            /*BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);

            if (file != null && file.Length > 0)
            {
                unit = (Unit)bf.Deserialize(file);
            }
            file.Close();*/

            //return unit;
        }
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

        /// <summary>
        /// 랜덤한 스킬을 받는다. 조건을 부여할수 있다.
        /// </summary>
        /// <param name="seed">랜덤 시드 번호..</param>
        /// <param name="species">종족</param>
        /// <param name="type">부모 클래스</param>
        /// <returns></returns>
        public static Skill GetRandomSkill(int seed, UnitSpecies species = UnitSpecies.NULL, SkillCategory skillCategory = SkillCategory.NULL)
        {
            List<Skill> skills = new List<Skill>();

            foreach (Skill skill in AllSkills)
                if ((skill.species.Contains(species) || species == UnitSpecies.NULL) &&
                    (skill.Category == skillCategory || skillCategory == SkillCategory.NULL))
                    skills.Add(skill);

            if (skills.Count == 0)
                return AllSkills[0].Clone();

            int idx = seed % skills.Count;
            return skills[idx].Clone();
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

            foreach (Skill skill in AllSkills)
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

        /// <summary>
        /// 랜덤 아티펙트 생성시 사용할 리스트
        /// Clone 해서 사용
        /// </summary>
        public static List<Artifact> AllArtifacts = new List<Artifact>()
        {
            new Model.Artifacts.Normal.LeatherBoots(),
            new Model.Artifacts.Normal.ACoin(),
            new Model.Artifacts.Normal.CopperRing(),
            new Model.Artifacts.Rare.BloodStone(),
        };

        public static Artifact GetRandomArtifact(int seed, Artifact.ArtifactGrade grade)
        {
            List<Artifact> list = (from artifact in AllArtifacts
                                   where artifact.Grade == grade
                                   select artifact).ToList();

            Artifact t = list[seed % list.Count].Clone() as Artifact;

            return t;
        }

        /// <summary>
        /// 랜덤 아이템 생성시 사용할 리스트
        /// Clone 해서 사용
        /// </summary>
        public static List<Item> AllItems = new List<Item>()
        {
            new Model.Items.Barrior(),
            new Model.Items.Bind(),
            new Model.Items.Damage(),
            new Model.Items.Fast(),
            new Model.Items.Heal(),
            new Model.Items.Key(),
            new Model.Items.Poison(),
            new Model.Items.Regeneration(),
            new Model.Items.Stun(),
        };

        /// <summary>
        /// 랜덤 이펙트 생성시 사용할 리스트
        /// Clone 해서 사용
        /// </summary>
        public static List<Effect> AllEffects = new List<Effect>()
        {
            new Model.Effects.Barrier(),
            new Model.Effects.Bind(),
            new Model.Effects.Fast(),
            new Model.Effects.Poison(),
            new Model.Effects.Regeneration(),
            new Model.Effects.Stun(),
        };

        public static List<Tile> AllTiles = new List<Tile>()
        {
            new Model.Tiles.DownStair(),
            new Model.Tiles.Floor(),
            new Model.Tiles.Heal(),
            new Model.Tiles.Hole(),
            new Model.Tiles.Locked(),
            new Model.Tiles.Power(),
            new Model.Tiles.Sell(),
            new Model.Tiles.Thorn(),
            new Model.Tiles.UnLocked(),
            new Model.Tiles.UpStair(),
            new Model.Tiles.Wall()
        };

        public static List<Artifact> NomalArtifacts = new List<Artifact>();
        public static List<Artifact> RareArtifacts = new List<Artifact>();
        public static List<Artifact> LegendArtifacts = new List<Artifact>();


        public static List<Skill> AllSkills { get; } = new List<Skill>()
        {
            // Move 스킬
            new Pawn(),
            new Knight(),
            new King(),
            new Bishop() ,
            new Queen() ,
            new Rook() ,

            new Heal() ,
            new Scratch() ,
            new Fireball() ,
            new Snapshot() ,
            new Slash(),
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
                    arr[i, j] = '0';

            arr[range, range] = '1';

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
                    str += arr[i, j];
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

        public static Dictionary<UnitSpecies, List<int>> SpeciesToSpriteNumbers = new Dictionary<UnitSpecies, List<int>>
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
        // public static Sprite LoadSprite(string path)
        // {
        //     Sprite sprite;
        //     if (path == "")
        //         sprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/X");
        //     else if (path.Contains("1bitpack_kenney_1/Tilesheet/colored_transparent_packed"))
        //     {
        //         string[] splited = path.Split('_');
        //         int spriteNumber = int.Parse(splited[splited.Length - 1]);
        //         sprite = Colored[spriteNumber];
        //     }
        //     else if (path.Contains("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed"))
        //     {
        //         string[] splited = path.Split('_');
        //         int spriteNumber = int.Parse(splited[splited.Length - 1]);
        //         sprite = Monochrome[spriteNumber];
        //     }
        //     else
        //         sprite = Resources.Load<Sprite>(path);
        //     return sprite;
        // }


        /// <summary>
        /// 스프라이트 번호로 부터 모노크롬 이미지를 받아 안쪽을 inColor, 바깥쪽을 outColor로 칠해준다.
        /// </summary>
        /// <param name="spriteNumber">스프라이트 번호</param>
        /// <param name="inColor">안쪽 색</param>
        /// <param name="outColor">바깥쪽 색</param>
        /// <returns></returns>
        public static Sprite MakeSprite(int spriteNumber, Color inColor, Color outColor)
        {
            Texture2D old = Monochrome[spriteNumber].texture;
            Rect rect = Monochrome[spriteNumber].rect;

            Texture2D new_texture = new Texture2D(16, 16);
            new_texture.filterMode = FilterMode.Point;

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x, (int)rect.y + y);
                    if (color.a == 0)
                        new_texture.SetPixel(x, y, outColor);
                    else
                        new_texture.SetPixel(x, y, inColor);
                }

            new_texture.Apply();
            rect = new Rect(0, 0, new_texture.width, new_texture.height);

            return Sprite.Create(new_texture, rect, new Vector2(0.5f, 0.5f), 16);
        }

        public static Sprite MakeOutline(Sprite value, Color outline)
        {
            Texture2D old = value.texture;
            Rect rect = value.rect;
            Texture2D texture = new Texture2D(18, 18);
            texture.filterMode = FilterMode.Point;

            for (int y = 0; y < 18; y++)
                for (int x = 0; x < 18; x++)
                    texture.SetPixel(x, y, new Color(0, 0, 0, 0));

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x, (int)rect.y + y);

                    if (color.a == 0)
                        continue;

                    color = outline;
                    texture.SetPixel(x + 2, y + 1, color);
                    texture.SetPixel(x, y + 1, color);
                    texture.SetPixel(x + 1, y + 2, color);
                    texture.SetPixel(x + 1, y, color);
                }

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x, (int)rect.y + y);
                    if (color.a == 0)
                        continue;
                    texture.SetPixel(x + 1, y + 1, color);
                }

            texture.Apply();
            rect = new Rect(0, 0, texture.width, texture.height);

            return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 16);
        }

        public static Sprite MakeOutline(Sprite value, Color inline, Color outline)
        {
            Texture2D old = value.texture;
            Rect rect = value.rect;
            Texture2D texture = new Texture2D(18, 18);
            texture.filterMode = FilterMode.Point;

            for (int y = 0; y < 18; y++)
                for (int x = 0; x < 18; x++)
                    texture.SetPixel(x, y, new Color(0, 0, 0, 0));

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x, (int)rect.y + y);

                    if (color.a == 0)
                        continue;

                    color = outline;
                    texture.SetPixel(x + 2, y + 1, color);
                    texture.SetPixel(x, y + 1, color);
                    texture.SetPixel(x + 1, y + 2, color);
                    texture.SetPixel(x + 1, y, color);
                }

            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    Color color = old.GetPixel((int)rect.x + x, (int)rect.y + y);
                    if (color.a == 0)
                        continue;
                    texture.SetPixel(x + 1, y + 1, inline);
                }

            texture.Apply();
            rect = new Rect(0, 0, texture.width, texture.height);

            return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 16);
        }
    }
}