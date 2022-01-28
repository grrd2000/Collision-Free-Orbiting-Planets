using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionToCenter : MonoBehaviour
{
    [SerializeField] private float attractionForce = 1f;
    [SerializeField] private Vector2 initialVelocity = Vector2.zero;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Controller controller;
    [SerializeField] private Vector2 centerPos = new Vector2(0, 0);
    [SerializeField] private Color collisionColor;
    [SerializeField] private Color normalColor;
    private float t = 0;

    public static List<AttractionToCenter> Attractors;

    private void OnEnable()
    {
        if (Attractors == null)
            Attractors = new List<AttractionToCenter>();

        Attractors.Add(this);

        if (initialVelocity == Vector2.zero)
        {
            Vector2 randV = new Vector2(Random.value, Random.value);
            float r = Random.Range(-10, 10);

            rb2D.velocity = randV * r;

            Debug.Log("Random initial velocity: " + rb2D.velocity);
        }
        else
        {
            rb2D.velocity = initialVelocity;
            Debug.Log("Initial velocity: " + rb2D.velocity);
        }
    }

    private void OnDisable()
    {
        Attractors.Remove(this);
    }

    private void FixedUpdate()
    {
        Vector2 toCenter = centerPos - rb2D.position;
        float distToCenter = toCenter.magnitude;

        rb2D.velocity += attractionForce * toCenter;

        foreach (AttractionToCenter a in Attractors)
        {
            if (a != this)
            {
                Vector2 collisionVec = rb2D.position - a.rb2D.position;
                float dist = collisionVec.magnitude;
                float minDist = (GetComponent<Transform>().localScale.x / 2) + (a.GetComponent<Transform>().localScale.x / 2);

                if (dist == 0f)
                    return;

                if (dist == minDist)
                {
                    //Debug.Log("COLLISION");
                    sr.color = collisionColor;
                    t = 0;
                }
                else
                {
                    if (sr.color != normalColor)
                    {
                        sr.color = Color.Lerp(collisionColor, normalColor, t);
                        t += Time.unscaledDeltaTime / 4f;
                    }
                }

                if (dist <= minDist)
                {
                    controller.collision = true;
                }

                if (dist < minDist)
                {
                    Vector2 axe = collisionVec / dist;
                    rb2D.position += 0.5f * (minDist - dist) * axe;
                    a.rb2D.position -= 0.5f * (minDist - dist) * axe;
                }
            }
        }
    }
}
