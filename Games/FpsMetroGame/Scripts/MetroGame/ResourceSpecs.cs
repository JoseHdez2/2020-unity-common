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
    public float start, end;
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