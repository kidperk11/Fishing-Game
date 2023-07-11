using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPTeleporter : MonoBehaviour
{
    [Header("External References")]
    public FPTeleporter destinationTeleporter;

    [Header("Teleportation Control Variables")]
    [SerializeField] private float maxCooldownTime;
    private bool onCooldown;
    

    public void TeleportHere(GameObject player)
    {
        onCooldown = true;
        player.transform.position = this.transform.position;
        StartCoroutine(TeleporterCooldown());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !onCooldown){
            destinationTeleporter.TeleportHere(other.gameObject);
        }
    }
    private IEnumerator TeleporterCooldown()
    {
        yield return new WaitForSeconds(maxCooldownTime);
        onCooldown = false;
        Debug.Log("Cooldown has ended for teleporter.");
    }
}
