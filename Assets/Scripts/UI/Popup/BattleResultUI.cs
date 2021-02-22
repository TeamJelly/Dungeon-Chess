﻿using Common.UI;
using Model;
using Model.Managers;
using Model.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace  UI.Popup
{
    public class BattleResultUI : MonoBehaviour
    {
        public GameObject WinUI;
        public GameObject DeafeatUI;
        public static BattleResultUI instance;


        public GameObject rewardList;
        public GameObject rewardPrefab;

        public Sprite goldSprite;
        
        private void Awake()
        {
            instance = this;
        }

        bool isUIEnabled = false;
        public void EnableWinUI()
        {
            if (isUIEnabled) return;
            isUIEnabled = true;

            int gold = UnityEngine.Random.Range(0, 500);

            int skillNumber = UnityEngine.Random.Range(0, 10);
            Skill skill = (Skill)Activator.CreateInstance(Type.GetType($"Model.Skills.Skill_00{skillNumber}"));

            GameObject goldReward = Instantiate(rewardPrefab,rewardList.transform);
            goldReward.GetComponent<Image>().sprite = goldSprite;
            goldReward.GetComponentInChildren<TextMeshProUGUI>().text = $"${gold}";
            goldReward.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.Gold += gold; goldReward.gameObject.SetActive(false); });
            goldReward.SetActive(true);

            GameObject skillReward = Instantiate(rewardPrefab, rewardList.transform);
            skillReward.GetComponent<Image>().sprite = skill.Sprite;
            skillReward.GetComponentInChildren<TextMeshProUGUI>().text = skill.Name;
            skillReward.GetComponent<Button>().onClick.AddListener(() => { GetSkillUI.instance.Enable(skill); skillReward.SetActive(false); });
            skillReward.SetActive(true);

            UIEffect.FadeInPanel(WinUI);
        }

        public void EnableDeafeatUI()
        {
            UIEffect.FadeInPanel(DeafeatUI);
        }
    }
}