using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Weapon", menuName = "Assets/Item/Weapon")]
public class Weapon : Item
{
    [SerializeField] protected float _baseDamage;
    private float _damage;
    [SerializeField] protected float _baseFireRate;
    private float _fireRate;
    [SerializeField] protected float _baseSize;
    private float _size;
    [SerializeField] protected float _baseLifetime;
    private float _lifetime;
    [SerializeField] protected float _baseHeatBuildup;
    private float _heatBuildup;
    [SerializeField] protected float _baseCoolingTime;
    private float _coolingTime;
    [SerializeField] protected int _baseProjectileCount;
    private int _projectileCount;

    [SerializeField] protected GameObject _projectile;

    public float BaseDamage
    {
        get { return _baseDamage; }
    }

    public float BaseFireRate
    {
        get { return _baseFireRate; }
    }

    public float BaseSize
    {
        get { return _baseSize; }
    }

    public float BaseLifetime
    {
        get { return _baseLifetime; }
    }

    public float BaseHeatBuildup
    {
        get { return _heatBuildup; }
    }

    public float BaseCoolingTime
    {
        get { return _coolingTime; }
    }

    public int BaseProjectileCount
    {
        get { return _baseProjectileCount; }
    }

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public float FireRate
    {
        get { return _fireRate; }
        set { _fireRate = value; }
    }

    public float Size
    {
        get { return _size; }
        set { _size = value; }
    }

    public float Lifetime
    {
        get { return _lifetime; }
        set { _lifetime = value; }
    }

    public float HeatBuildup
    {
        get { return _heatBuildup; }
        set { _heatBuildup = value; }
    }

    public float CoolingTime
    {
        get { return _coolingTime; }
        set { _coolingTime = value; }
    }

    public int ProjectileCount
    {
        get { return _projectileCount; }
        set { _projectileCount = value; }
    }

    private void OnEnable()
    {
        _damage = _baseDamage;
        _fireRate = _baseFireRate;
        _size = _baseSize;
        _lifetime = _baseLifetime;
        _heatBuildup = _baseHeatBuildup;
        _coolingTime = _baseCoolingTime;
        _projectileCount = _baseProjectileCount;
    }

    void ResetStats()
    {
        _damage = _baseDamage;
        _fireRate = _baseFireRate;
        _size = _baseSize;
        _lifetime = _baseLifetime;
        _heatBuildup = _baseHeatBuildup;
        _coolingTime = _baseCoolingTime;
        _projectileCount = _baseProjectileCount;
    }
}
