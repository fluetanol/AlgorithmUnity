using System;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

namespace SerializableCollections
{
    [Serializable]
    public class SerializableQueue<T> : Queue<T>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<T> _list = new List<T>();

        public SerializableQueue() :base()
        {  }

        public SerializableQueue(List<T> list) : base(list)
        {
            _list = list;
        }

        //처음 직렬화 되려고 할 때 해야 할 작업
        public void OnBeforeSerialize()
        {
            _list.Clear();
            foreach (var item in this)
            {
                _list.Add(item);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var item in _list)
            {
                Enqueue(item);
            }
        }



    }
}
