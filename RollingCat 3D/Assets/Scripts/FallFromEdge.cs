using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallFromEdge : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ForcePointUp" || other.tag == "ForcePointDown")
        {
            gameObject.SetActive(false);
            PlayerController.Instance.canMove = false;
            PlayerController.Instance.AddForce(other.tag == "ForcePointUp" ? 0 : 1);
        }
    }
}
