using UnityEngine;

public class TrashCan : Pickup{
    [SerializeField] private float collisionPushSpeed = 20f;
    [SerializeField] private Vector3 collisionTorque = new Vector3(2f, 2f, 2f);
    [SerializeField] private float destroyDelay = 3f;

    protected override void PickedUpBy(GameObject picker){
        GetMovementComponent().enabled = false;
        GetComponent<Collider>().enabled = false;
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        rigidBody.AddForce((transform.position - picker.transform.position).normalized * collisionPushSpeed,
            ForceMode.VelocityChange);
        rigidBody.AddTorque(collisionTorque, ForceMode.VelocityChange);
        Invoke(nameof(DestroySelf), destroyDelay);
    }

    private void DestroySelf(){
        Destroy(gameObject);
    }
}