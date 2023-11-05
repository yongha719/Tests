using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour, IPooledObject
{
    public float Speed;

    private PlayerType playerType = PlayerType.LeftPlayer;

    public bool CanGet { get; set; } = true;

    public IObjectPool<Bullet> pool { get; set; }


    private WaitForSeconds destroyWait = new WaitForSeconds(3f);

    void OnEnable()
    {
        StartCoroutine(ReleaseCoroutine());
    }

    // WaitUntil을 사용하여 특정조건이 됐을 때 릴리즈되도록 해도 됨
    // UniTask 활용가능
    private IEnumerator ReleaseCoroutine()
    {
        yield return destroyWait;

        pool.Release(this);
    }

    // PlayerType으로 Dir를 설정
    public void SetDir(PlayerType type)
    {
        playerType = type;
    }

    public void OnDestroyObject()
    {
        print("삭제됨");
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * (Speed * Time.deltaTime * (float)playerType), Space.World);
    }
}
