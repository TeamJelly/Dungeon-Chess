using Model;
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
                itemSlots.Add(newButton);
            }
        }

        void UpdateUI()
        {
            //아이템 슬롯 갱신. 새 아이템 들어왔을 경우 고려.
            for (int i = 0; i < 3; i++)
            {
                itemSlots[i].properties.onClick.RemoveAllListeners();// 리스너 중복 적용 방지.

                if (GameManager.Instance.itemBag.Count > i)
                {
                    itemSlots[i].MainImage.sprite = GameManager.Instance.itemBag[i].Sprite;
                }

                else itemSlots[i].MainImage.sprite = emptySprite;
                int idx = i;
                itemSlots[i].properties.onClick.AddListener(() =>
                {
                    itemSlots[idx].properties.onClick.RemoveAllListeners();// 가방 열고서 쓴다음 리스너 제거
                    itemSlots[idx].MainImage.sprite = emptySprite;
                });
            }

            //유물 슬롯 갱신. 새 유물 들어왔을 경우 고려.
            //개수가 늘어났다는 것은 기존 유물들 포함해서 더 추가가 되었다는 의미.
            //유물 슬롯은 유물 사용 시 그때 바로 제거되므로 
            //가방 열때마다 기존 유물을 담던 슬롯은 갱신할 필요는 없음.
            if (artifactSlots.Count < GameManager.Instance.artifactBag.Count)
            {
                Debug.Log("Updated");
                for (int i = artifactSlots.Count; i < GameManager.Instance.artifactBag.Count; i++)
                {
                    GameObject newSlot = Instantiate(buttonPrefab, artifactContainer);
                    PixelButton newButton = newSlot.GetComponent<PixelButton>();

                    Artifact artifact = GameManager.Instance.artifactBag[i];
                    newButton.MainImage.sprite = artifact.Sprite;


                    int idx = i;
                    newButton.properties.onClick.AddListener(() =>
                    {
                        //artifactSlots.Remove(newButton);
                        //GameManager.Instance.artifactBag.Remove(artifact);
                        //Destroy(newButton.gameObject);
                    });
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