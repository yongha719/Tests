using System;
using UnityEngine;

public enum PlayerType
{
    LeftPlayer = 1,
    RightPlayer = -1
}

public class Player : MonoBehaviour
{
    public PlayerType PlayerType;

    public float Speed;

    private KeyCode attackKey;

    void Start()
    {
        attackKey = PlayerType == PlayerType.LeftPlayer ? KeyCode.LeftControl : KeyCode.RightShift;
    }

    void FixedUpdate()
    {
        float vertical = Input.GetAxis(PlayerType.ToString());

        transform.Translate(Vector2.down * (vertical * Speed * Time.deltaTime), Space.Self);
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Bullet bullet = ObjectPoolManager.Instance.BulletPool.Get(transform);
            bullet.SetDir(PlayerType);
            bullet.transform.SetPositionAndRotation(transform.position + (Vector3.right * (float)PlayerType), transform.rotation);
        }
    }
}
