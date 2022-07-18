using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Artifact : MonoBehaviour, ISpriteable
{
    public enum GradeEnum { Normal, Rare, Legend }

    [SerializeField] private Sprite sprite;
    [SerializeField] private GradeEnum grade;
    [SerializeField] private int price;

    public Sprite Sprite { get => sprite; set => sprite = value; }

}