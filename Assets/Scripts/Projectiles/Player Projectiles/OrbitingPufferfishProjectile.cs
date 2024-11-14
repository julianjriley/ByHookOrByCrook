using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class OrbitingPufferfishProjectile : Projectile
{
    [SerializeField, Tooltip("Used to trigger animiations.")]
    private Animator _anim;

    // Start is called before the first frame update
    float resetInterval = 4f;
    Collider _collider;
    GameObject _parent;
    GameObject _player;
    // Update is called once per frame]
    protected override void Start()
    {
        base.Start();
        _collider = GetComponent<Collider>();
        _parent = transform.parent.gameObject;
        
    }
    void Update()
    {
        if (_player == null)
            return;
        _parent.transform.RotateAround(_player.transform.position, new Vector3(0, 0, 1), 60 * Time.deltaTime);
        _parent.transform.position = _player.transform.position;
    }
  

    public void AssertPlayer(GameObject player)
    {
        _player = player;
    }

    public override void TakeDamage(float damage)
    {
        
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();

        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);

        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collider.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        StartCoroutine(WaitForReactivation());
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();

        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);

        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        StartCoroutine(WaitForReactivation());

    }

    IEnumerator WaitForReactivation()
    {
        _anim.SetTrigger("Deflate");
        _collider.enabled = false;
        yield return new WaitForSeconds(resetInterval);
        _anim.SetTrigger("Inflate");
        _collider.enabled = true;
    }
}
