using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<KeyValuePair<T ,float>> elements = new List<KeyValuePair<T ,float>>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float> ( item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for(int i=0; i<elements.Count; i++)
        {
            if (elements[i].Value > elements[bestIndex].Value) {
            bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex); return bestItem;
    }

    public T DDequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Value < elements[bestIndex].Value)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex); return bestItem;
    }

    public bool TryGetPriority(T item, float priority)
    {

        for(int i =0; i<elements.Count; i++)
        {

            if (EqualityComparer<T>.Default.Equals(elements[i].Key, item))
            {
                return true;    
            }
            
        }
        return false;
    }

    public bool GetPri(T item, float priority)
    {

        

        for (int i = 0; i < elements.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(elements[i].Key, item))
            {
                return true;
            }
            
        }
        return false;
    }

    public void UpdatePriority(T item, float priority)
    {
        int bestIndex = 0;
        for(int i =0; i<elements.Count; i++)
        {
            //새로 올라온 우선순위를 갱신하는 업데이트
            //해당 item을 dequeue후에
            //업데이트를 할려면 받아온 priority로 다시 enqeue
            if(EqualityComparer<T>.Default.Equals(elements[i].Key, item))
            {
                bestIndex = i;

            }

        }
        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex);

        Enqueue(item, priority);
    }
}
