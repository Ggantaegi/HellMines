using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    Move,
    Attack
}

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public int _hp;
    public float _attackRangeSqr;
    public float _attackAccuracy;
    public int _attackDamage;
    public float _attackDelay;
    public float _moveSpeed;

    private GameObject _player;
    private Rigidbody _rb;
    private Animator _anim;

    private float _lastAttackTime;
    private EnemyState _enemyState = EnemyState.Move;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        Debug.Assert(_player != null);
    }

    public void OnSpawn()
    {
        _hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceToPlayer() < _attackRangeSqr)
        {
            Attack();
        }
        else
            Move();
    }

    private float DistanceToPlayer()
    {
        return (_player.transform.position-transform.position).sqrMagnitude;
    }

    private void Attack()
    {
        if( _enemyState == EnemyState.Move )
        {
            _enemyState = EnemyState.Attack;
            _anim.SetTrigger("Attack");
        }
        LookAt2D(_player.transform);
        if (_lastAttackTime + _attackDelay < Time.time)
        {
            // TODO: ���� ��� ���ϱ�
            // ź���� �����ϴ� ��� �������� �������� ����
            _rb.velocity = Vector3.zero;
            if (Random.Range(0, 1f) < _attackAccuracy)
            {
                _player.GetComponent<PlayerHP>().DoDamage(_attackDamage);
            }
            _lastAttackTime = Time.time;
        }
    }

    private void Move()
    {
        if (_enemyState == EnemyState.Attack)
        {
            _enemyState = EnemyState.Move;
            _anim.SetTrigger("Move");
        }
        LookAt2D(_player.transform);
        if(_rb.velocity.sqrMagnitude < _moveSpeed)
        _rb.AddForce(transform.forward * _moveSpeed, ForceMode.VelocityChange);
    }

    private void LookAt2D(Transform target)
    {
        transform.LookAt(target);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void DoDamage(int damage)
    {
        // TODO: ������ ���׼�
        _hp -= damage;
        if (_hp <= 0)
            Die();
    }

    private void Die()
    {
        StartCoroutine(DoDie());
    }

    private IEnumerator DoDie()
    {
        // TODO: ���� ����
        var particle = GetComponent<ParticleSystem>();
        particle.Stop();
        particle.Play();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
