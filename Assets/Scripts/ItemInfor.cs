using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class ItemInfor : ScriptableObject
{
    public Sprite Image;
    public string Name;
    [TextArea]
    public string Description;
}
