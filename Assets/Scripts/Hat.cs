using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hat", menuName = "Hat")]
public class Hat : ScriptableObject
{
    public new string description, stat;

    [SerializeField]
    public Sprite hatDesign;


    public virtual void OnWear(Villager wearer) {}
}
