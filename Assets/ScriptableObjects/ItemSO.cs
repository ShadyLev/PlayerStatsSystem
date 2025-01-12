using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
    public class ItemSO : ScriptableObject
    {
        public int Id;
        public string Name;
        public string Description;
        public Sprite Icon;
        public GameObject Prefab;
    }
}