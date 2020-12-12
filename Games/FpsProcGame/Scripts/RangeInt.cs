using UnityEngine;

[System.Serializable]
public class RangeInt{
    public int min, max;

    public RangeInt() {}
    public RangeInt(int min, int max) { (this.min, this.max) = (min, max);}

    public int RandomInt() => Random.Range(min, max);
}