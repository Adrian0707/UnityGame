using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTile 
{
    public enum Type { Empty,Full}
    Type type;

    public int x { get; private set; }
    public int y { get; private set; }
     public MyTile(int x ,int y)
    {
        this.x = x;
        this.y = y;
        this.type = Type.Full;
    }

}
