using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D<T>
{
    T[] data;

    public Vector3 Size { get; private set; }
    public Vector3 Offset { get; set; }

    public Grid2D(Vector3 size, Vector3 offset)
    {
        Size = size;
        Offset = offset;

        data = new T[(int)(size.x * size.z)];
    }

    public int GetIndex(Vector3 pos)
    {
        return (int)(pos.x + (Size.x * pos.z));
    }

    public bool InBounds(Vector3 pos)
    {
        if( Size.x > pos.x && Size.z > pos.z && pos.x >= 0 && pos.z >=0)
        {
            return true;
        }
        return false;
        
    }

    public T this[int x, float y,int z]
    {
        get
        {
            return this[new Vector3(x, y, z)];
        }
        set
        {
            this[new Vector3(x, y,z)] = value;
        }
    }

    public T this[Vector3 pos]
    {
       
        get
        {
            pos += Offset;
            return data[GetIndex(pos)];
        }
        set
        {
            pos += Offset;
            data[GetIndex(pos)] = value;
        }
    }
}
