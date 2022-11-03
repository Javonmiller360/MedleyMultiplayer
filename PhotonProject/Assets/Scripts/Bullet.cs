using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask Enemy;

    [Range(0f,1f)]
    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start() {
        Setup();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            //stream.SendNext(IsFiring);
            //stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            //this.IsFiring = (bool)stream.ReceiveNext();
            //this.Health = (float)stream.ReceiveNext();
        }
    }

    private void Update(){
        if (collisions > maxCollisions) Explode();

        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0){
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.collider.CompareTag("Bullet")) return;

        collisions++;

        if(collision.collider.CompareTag("Enemy") && explodeOnTouch){
            Explode();
        }
    }

    private void Explode(){
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, Enemy);

        //Enemy Takes Damage
        for (int i = 0; i < enemies.Length; i++){
            //enemies[i].GetComponent<EnemyAI>().TakeDamage(explosionDamage);
            if (enemies[i].GetComponent<Rigidbody>()){
            enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }
        }

        Invoke("Delay", 0.01f);
    }

    private void Delay(){
        Destroy(gameObject);
    }

    private void Setup(){
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
