using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    private Rigidbody _rigidbody;

    protected void Awake () {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        // Only collide with "Player" (6), "Bullet" (7), "Wall" (9)
        int wallLayerMask = (1 << 9);
        int collidableLayerMask = wallLayerMask;
        
        GameObject otherGameObject = other.gameObject;

        if ((collidableLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(this.gameObject);
        }
    }

}