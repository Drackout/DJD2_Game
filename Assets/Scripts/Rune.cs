using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public float minDist = 5;
    public float maxDist = 10;
    public float displayDist = 15;
    public float tolerance = 10.0f;
    public float maxEmission = 0.7f;

    public Transform player;

    Material material;

    // Start is called before the first frame update
    void Start()
    {
        var mr = GetComponent<MeshRenderer>();

        material = new Material(mr.material);
        mr.material = material;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

            float meanDist = (maxDist + minDist) * 0.5f;
            float winDist = (maxDist - minDist) * 0.5f;
            float factor = 1 - Mathf.Abs(Mathf.Clamp((distance - meanDist) / winDist, -1, 1));

            float tol = Mathf.Cos(tolerance * Mathf.Deg2Rad);
            float dp = Vector3.Dot(transform.forward, player.forward);
            dp = Mathf.Clamp01((dp - tol) / (1.0f - tol));

            //limit the emission light 
            factor = Mathf.Min(factor * dp, maxEmission);

            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.green * factor);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDist);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxDist);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, displayDist);
    }
}
