using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public BoidsConfiguration configuration;
    public Vector3 position => transform.position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        configuration.Add(this);
        velocity = Random.insideUnitSphere;
        transform.position = Vector3.Scale(configuration.bounds.size, new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f))) + configuration.bounds.min;
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        Debug.Log(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = Vector3.zero;
        Vector3 min = configuration.bounds.min;
        Vector3 size = configuration.bounds.size;
        Vector3 halfSize = size / 2f;
        Vector3[] forces = new Vector3[configuration.forces.Count];
        float[] weights = new float[configuration.forces.Count];
        for (int i = 0; i < configuration.forces.Count; i++)
        {
            forces[i] = Vector3.zero;
            weights[i] = 0f;
        }
        foreach (Boid b in configuration.boids)
        {
            if (b == this) continue;
            var d = b.position - position;
            d.x = (d.x > halfSize.x) ? (d.x - size.x) : (d.x < -halfSize.x ? d.x + size.x : d.x);
            d.y = (d.y > halfSize.y) ? (d.y - size.y) : (d.y < -halfSize.z ? d.y + size.y : d.y);
            d.z = (d.z > halfSize.z) ? (d.z - size.z) : (d.z < -halfSize.z ? d.z + size.z : d.z);
            var normalizedD = d.normalized;
            var dInverse = 1f / d.magnitude;
            var dInverseSquared = 1f / d.sqrMagnitude;

            int i = 0;
            foreach (var force in configuration.forces)
            {
                var weight = dInverseSquared;
                var factor = force.factor * weight;
                forces[i] += factor * (force.forceMode == BoidsConfiguration.ForceMode.Alignment ? b.velocity.normalized : normalizedD);
                weights[i] += weight;
                i++;
            }
        }

        for (int i = 0; i < forces.Length; i++)
        {
            if (configuration.forces[i].aggregateMode == BoidsConfiguration.AggregateMode.WeightedMean)
                forces[i] /= weights[i];

            acceleration += forces[i] * Time.deltaTime;
        }


    }

    private void LateUpdate()
    {
        Vector3 min = configuration.bounds.min;
        Vector3 size = configuration.bounds.size;
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, 3f);

        transform.position += velocity * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Repeat(transform.position.x - min.x, size.x) + min.x,
            Mathf.Repeat(transform.position.y - min.y, size.y) + min.y,
            Mathf.Repeat(transform.position.z - min.z, size.z) + min.z
            );

        Debug.DrawRay(position, velocity.normalized * 0.2f, color);
    }
}
