using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArmorHelp
{
    public class MyToggle : Toggle
    {
        private int id;
        [SerializeField] Text text;

        public int Id { get => id; set => id = value; }
        public Text Text { get => text; }
    }
}

