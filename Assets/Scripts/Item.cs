using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleFoxLite
{
    public class Item : InteracBase
    {
        [SerializeField] ItemInfor itemInfor;
        private void Awake()
        {
            type = InteractType.grab;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
