using UnityEngine;
using System.Collections.Generic;

public class LongRainDetector : MonoBehaviour
{
    private RaindropConnector connector;

    // LongRain‚ª’Ê‰ß‚µ‚½‰J—±
    private GameObject previousRaindrop;

    // “¯‚¶‰J—±‚É‰½“x‚à”½‰‚µ‚È‚¢‚½‚ß‚Ì‹L˜^
    private HashSet<GameObject> passedRaindrops =
        new HashSet<GameObject>();

    private void Start()
    {
        connector = FindObjectOfType<RaindropConnector>();

        if (connector == null)
        {
            Debug.LogError("RaindropConnector‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñB");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Raindrop"))
        {
            return;
        }

        GameObject currentRaindrop = other.gameObject;

        if (passedRaindrops.Contains(currentRaindrop))
        {
            return;
        }

        passedRaindrops.Add(currentRaindrop);

        Debug.Log("’Ê‰ß‚µ‚½‰J—±F" + currentRaindrop.name);

        if (previousRaindrop != null && connector != null)
        {
            connector.ConnectRaindrops(
                previousRaindrop,
                currentRaindrop
            );
        }

        previousRaindrop = currentRaindrop;
    }
}