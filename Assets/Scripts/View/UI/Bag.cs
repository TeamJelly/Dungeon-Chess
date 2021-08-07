using Model;
using Model.Artifacts;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View.UI
{
    public class Bag : MonoBehaviour
    {
        public GameObject panel;
        public GameObject buttonPrefab;
        public Transform itemContainer;
        public Transform artifactContainer;
        public List<PixelButton> itemSlots;
        public List<PixelButton> artifactSlots;

        Sprite emptySprite;

        public static Bag instance;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            InitUI();
        }
        public void InitUI()
        {
            //빈 이미지 불러오기
            emptySprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/X");

            //아이템 버튼 생성.
            for (int i = 0; i < 3; i++)
            {
                GameObject newSlot = Instantiate(buttonPrefab, itemContainer);
                PixelButton newButton = newSlot.GetComponent<PixelButton>();
                newButton.toggleOption = true;
                itemSlots.Add(newButton);
            }
        }

        void UpdateItemSlots()
        {
            for (int i = 0; i < 3; i++)
            {
                //아이템 슬롯 인덱스보다 보유 아이템 수가 클때
                if (GameManager.Instance.itemBag.Count > i)
                {
                    itemSlots[i].MainImage.sprite = GameManager.Instance.itemBag[i].Sprite;
                    itemSlots[i].SetInteractable(true);

                    int idx = i;

                    //버튼 눌렸을 시 이벤트 추가
                    itemSlots[i].OnPushButton = () =>
                    {
                        UnitControlView.instance.ToggleAllButtons();
                        ToggleAllSlots();
                        itemSlots[idx].SetInteractable(true);
                        //사용 가능 타일 보여주기
                        IndicatorView.ShowTileIndicator(
                            FieldManager.instance.allTilesPosition,
                            (pos) =>
                            {
                                Tile tile = FieldManager.GetTile(pos);
                                //타일 위에 유닛 있으면 해당 유닛에 아이템 사용
                                if (tile.HasUnit())
                                {
                                    GameManager.Instance.itemBag[idx].Use(tile.GetUnit());
                                }
                                ResetSlot(itemSlots[idx]);
                                IndicatorView.HideTileIndicator();
                            },
                            GameManager.Instance.itemBag[idx].GetRelatePositions
                        );
                    };

                    itemSlots[i].OnPopButton = () =>
                    {
                        UnitControlView.instance.ToggleAllButtons();
                        ToggleAllSlots();
                        itemSlots[idx].SetInteractable(true);
                        IndicatorView.HideTileIndicator();
                    };
                }
                else ResetSlot(itemSlots[i]);
            }
        }

        void ResetSlot(PixelButton button)
        {
            button.OnPushButton = null;
            button.SetInteractable(false);
            button.MainImage.sprite = emptySprite;
        }
        void DisableSlot(PixelButton button)
        {
            button.SetInteractable(false);
        }
        public void ToggleAllSlots()
        {
            foreach (var slot in itemSlots)
                slot.SetInteractable(!slot.properties.interactable && slot.OnPushButton != null);

            foreach (var slot in artifactSlots)
                slot.SetInteractable(!slot.properties.interactable);
        }
        public void UpdateUI()
        {
            //아이템 슬롯 갱신. 새 아이템 들어왔을 경우 고려.
            UpdateItemSlots();

            //유물 슬롯 갱신. 새 유물 들어왔을 경우 고려.
            //개수가 늘어났다는 것은 기존 유물들 포함해서 더 추가가 되었다는 의미.
            //유물 슬롯은 유물 사용 시 그때 바로 제거되므로 
            //가방 열때마다 기존 유물을 담던 슬롯은 갱신할 필요는 없음.
            if (artifactSlots.Count < GameManager.Instance.artifactBag.Count)
            {
                Debug.Log("Updated");
                for (int i = artifactSlots.Count; i < GameManager.Instance.artifactBag.Count; i++)
                {
                    int idx = i;
                    GameObject newSlot = Instantiate(buttonPrefab, artifactContainer);
                   
                    Artifact artifact = GameManager.Instance.artifactBag[idx];
                    
                    PixelButton newButton = newSlot.GetComponent<PixelButton>();
                    newButton.MainImage.sprite = artifact.Sprite;
                    newButton.toggleOption = true;
                    newButton.OnPushButton = () =>
                    {
                        UnitControlView.instance.ToggleAllButtons();
                        ToggleAllSlots();
                        artifactSlots[idx].SetInteractable(true);
                        Debug.Log("Pushed Artifact Button");
                        IndicatorView.ShowTileIndicator(
                            FieldManager.instance.allTilesPosition, 
                            (pos) =>
                            {
                                Debug.Log("Tile Clicked");
                                Tile tile = FieldManager.GetTile(pos);
                                if (tile.HasUnit())
                                {
                                    Common.Command.AddArtifact(tile.GetUnit(), artifact);
                                }
                                artifactSlots.Remove(newButton);
                                GameManager.Instance.artifactBag.Remove(artifact);
                                Destroy(newButton.gameObject);
                                IndicatorView.HideTileIndicator();
                            }
                        );
                    };
                    newButton.OnPopButton = () =>
                    {
                        UnitControlView.instance.ToggleAllButtons();
                        ToggleAllSlots();
                        artifactSlots[idx].SetInteractable(true);
                        IndicatorView.HideTileIndicator();
                    };

                    artifactSlots.Add(newButton);
                }
            }
        }
        public void Show()
        {
            UpdateUI();
            Common.UIEffect.FadeInPanel(panel);
        }

        public void Hide()
        {
            Common.UIEffect.FadeOutPanel(panel);
        }
    }
}