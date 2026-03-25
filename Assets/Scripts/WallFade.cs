using System.Collections.Generic;
using UnityEngine;

public class WallFade : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera cam;

    [Header("Fade Settings")]
    public LayerMask wallLayer;
    [Range(0f, 1f)] public float fadedAlpha = 0.75f;
    public float fadeSpeed = 5f;

    // Track every wall currently being managed
    private Dictionary<Renderer, float> _wallTargets = new();
    private HashSet<Renderer> _currentlyHit = new();

    void LateUpdate()
    {
        // Find the player if we don't have one yet
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                return; // Player hasn't spawned yet, skip this frame
        }

        _currentlyHit.Clear();

        // Raycast from camera to player
        Vector3 dir = player.position - cam.transform.position;
        float dist = dir.magnitude;
        Ray ray = new Ray(cam.transform.position, dir.normalized);

        float sphereRadius = 1.5f; // Adjust this to catch more or fewer walls
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRadius, dist, wallLayer);

        // Also fade walls close to the player even if not in the ray path
        float proximityRadius = 2.0f; // How close the player needs to be
        Collider[] nearbyWalls = Physics.OverlapSphere(player.position, proximityRadius, wallLayer);

        // Mark walls hit by the sphere cast
        foreach (var hit in hits)
        {
            var rend = hit.collider.GetComponent<Renderer>();
            if (rend == null) continue;

            _currentlyHit.Add(rend);

            if (!_wallTargets.ContainsKey(rend))
                _wallTargets[rend] = 1f;
        }

        // Mark walls near the player
        foreach (var col in nearbyWalls)
        {
            var rend = col.GetComponent<Renderer>();
            if (rend == null) continue;

            _currentlyHit.Add(rend);

            if (!_wallTargets.ContainsKey(rend))
                _wallTargets[rend] = 1f;
        }

        // Update alpha targets and lerp
        var toRemove = new List<Renderer>();

        foreach (var kvp in new Dictionary<Renderer, float>(_wallTargets))
        {
            var rend = kvp.Key;
            float current = kvp.Value;

            // Decide target: faded if currently hit, opaque if not
            float target = _currentlyHit.Contains(rend) ? fadedAlpha : 1f;
            float newAlpha = Mathf.MoveTowards(current, target, fadeSpeed * Time.deltaTime);

            _wallTargets[rend] = newAlpha;
            SetAlpha(rend, newAlpha);

            // Clean up walls that have fully returned to opaque
            if (!_currentlyHit.Contains(rend) && Mathf.Approximately(newAlpha, 1f))
                toRemove.Add(rend);
        }

        foreach (var r in toRemove)
            _wallTargets.Remove(r);
    }

    void SetAlpha(Renderer rend, float alpha)
    {
        // Set the _Fade property on the DitherFade shader
        foreach (var mat in rend.materials)
        {
            mat.SetFloat("_Fade", alpha);
        }
    }
}