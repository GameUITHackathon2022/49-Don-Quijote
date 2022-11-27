using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleFoxLite
{
    public enum InteractType
    {
        grab, dialogue
    }
    public abstract class InteracBase : MonoBehaviour
    {
        public InteractType type;
    }
}
