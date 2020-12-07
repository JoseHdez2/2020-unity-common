using UnityEngine;
using System.Collections;
using System;

public enum EResource
{
    BATTERIES,
    FOOD,
    MATERIALS,
    MEDICAL,
    GUNS
}

[Serializable]
public class RangeFloat
{
    public float min, max;

    public RangeFloat() {}
    public RangeFloat(float min, float max) { (this.min, this.max) = (min, max);}
}

[System.Serializable]
public class ResourceSpecs
{
    public EResource resource;
    public int quantityMinPerTurn;
    public int quantityMaxPerTurn;
    public int priceRangeMin;
    public int priceRangeMax;
}