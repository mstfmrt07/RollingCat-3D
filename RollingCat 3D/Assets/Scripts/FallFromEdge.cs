using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallFromEdge : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            gameObject.SetActive(false);
            PlayerController.Instance.canMove = false;

            PlayerController.Instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            PlayerController.Instance.GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * 100f, collision.GetContact(0).point, ForceMode.Force);
        }
    }
}
