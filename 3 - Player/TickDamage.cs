using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickDamage : MonoBehaviour
{
    private Player player;
    public int damage;
    private bool check;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        check = false;
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && !check)
        {
            StartCoroutine(TickDamageApply());
            check = true;
        }
    }

    private IEnumerator TickDamageApply()
    {
        float counter = 0;
        float waitTime = 1f;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            if (counter >= waitTime)
            {
                player.TakeDamage(damage);
                check = false;
                yield break;
            }
            yield return null;
        }
    }
}
