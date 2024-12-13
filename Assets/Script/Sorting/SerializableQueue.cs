using System;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

namespace SerializableCollections
{
    [Serializable]
    public class SerializableQueue<T> : ISerializationCallbackReceiver
    {
        [SerializeField] private List<T> _list = new List<T>();
        private Queue<T> _queue;

        public SerializableQueue()
        {
            _queue = new Queue<T>();
        }

        public SerializableQueue(List<T> list)
        {
            _queue = new Queue<T>(list);
        }

        //처음 직렬화 되려고 할 때 해야 할 작업
        public void OnBeforeSerialize()
        {
            // Debug.Log("OnBeforeSerialize");
            _list.Clear();
            foreach (var item in _queue)
            {
                _list.Add(item);
            }
        }

        public void OnAfterDeserialize()
        {
            //Debug.Log("OnAfterDeserialize");
            _queue = new Queue<T>();
            foreach (var item in _list)
            {
                _queue.Enqueue(item);
            }
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            return _queue.Dequeue();
        }

        public int Count => _queue.Count;


    }
}
