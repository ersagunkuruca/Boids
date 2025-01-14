using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidsConfiguration : ScriptableObject
{
    [NonSerialized]
    public List<Boid> boids = new List<Boid>();

    public List<BoidsForce> forces;
    public Bounds bounds;

    public enum DistanceFactor
    {
        Constant,
        Inverse,
        InverseSquared
    }

    public enum AggregateMode
    {
        Total,
        WeightedMean
    }

    public enum ForceMode
    {
        Distance,
        Alignment
    }

    [Serializable]
    public class BoidsForce {
        public float factor = 1f;
        public ForceMode forceMode;
        public DistanceFactor distanceFactor;
        public AggregateMode aggregateMode;
    }

    public void Add(Boid boid) {
        boids.Add(boid); 
    }

}
