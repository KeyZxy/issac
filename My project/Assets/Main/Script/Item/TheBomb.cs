using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBomb : MonoBehaviour
{
    // 炸弹爆炸后留下的痕迹的精灵
    public Sprite traces;
    // 炸弹引爆时间
    float detonatingTime = 1.5f;
    // 炸弹爆炸的影响半径
    float radius = 0.5f;

    // 初始化方法
    void Start()
    {
        // 启动炸弹引爆协程
        StartCoroutine(Detonating(detonatingTime));
    }

    // 炸弹引爆的协程
    IEnumerator Detonating(float detonatingTime)
    {
        // 获取炸弹的精灵渲染器组件
        SpriteRenderer SR = GetComponent<SpriteRenderer>();

        // 闪烁的时间间隔数组
        float[] flashCD = new float[] { 0f, 0.4f, 0.3f, 0.2f };
        // 单次闪烁的持续时间
        float flashTime = 0.05f;
        // 当前闪烁时间间隔的索引
        int flashCDIndex = 0;

        // 经过的时间
        float time = 0;
        // 下一次闪烁的时间点
        float timeGate = flashCD[0];
        // 在炸弹引爆时间内反复执行
        while (time < detonatingTime)
        {
            // 如果到达了闪烁的时间点且当前颜色为白色
            if (time >= timeGate && SR.color == Color.white)
            {
                // 将颜色设置为红色
                SR.color = Color.red;
                // 更新下一个闪烁的时间点
                timeGate += flashTime;
                // 更新闪烁间隔索引
                if (flashCDIndex < flashCD.Length - 1)
                {
                    flashCDIndex++;
                }
            }
            // 如果到达了闪烁的时间点且当前颜色为红色
            else if (time >= timeGate && SR.color == Color.red)
            {
                // 将颜色设置为白色
                SR.color = Color.white;
                // 更新下一个闪烁的时间点
                timeGate = timeGate + flashCD[flashCDIndex] - flashTime;
            }

            // 更新经过的时间
            time += Time.deltaTime;
            // 等待下一帧
            yield return 0;
        }
        // 最终将颜色设置为白色
        SR.color = Color.white;
        // 调用爆炸方法
        Explosion();
    }

    // 爆炸方法
    void Explosion()
    {
        // 获取爆炸范围内的所有碰撞器
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        // 遍历所有碰撞器
        foreach (var item in colliders)
        {
            // 计算爆炸力的方向
            Vector2 force = (item.transform.position - transform.position).normalized;
            // 如果碰撞器对象实现了 IDestructible 接口
            if (item.GetComponent<IDestructible>() != null)
            {
                // 销毁该对象
                item.GetComponent<IDestructible>().DestorySelf();
            }
            // 如果碰撞器对象是玩家
            else if (item.GetComponent<Player>())
            {
                // 攻击玩家
                item.GetComponent<Player>().BeAttacked(2, force, 1.5f);
            }
            // 如果碰撞器对象实现了 IAttackable 接口
            else if (item.GetComponent<IAttackable>() != null)
            {
                // 攻击该对象
                item.GetComponent<IAttackable>().BeAttacked(10, force, 1.5f);
            }
            // 如果碰撞器对象有刚体组件
            else if (item.GetComponent<Rigidbody2D>())
            {
                // 施加爆炸力
                item.GetComponent<Rigidbody2D>().AddForce(force * 10);
            }
        }
        // 冻结炸弹的所有物理约束
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        // 禁用炸弹的碰撞器
        GetComponent<Collider2D>().enabled = false;
        // 播放爆炸动画
        //GetComponent<Animator>().Play("Explosion");
        // 生成爆炸痕迹
        //GameManager.Instance.level.manager.GenerateTraceInCurrentRoom(traces, transform.position);
        // 延迟 0.5 秒后销毁炸弹对象
        Invoke("Destroy", 0.5f);
    }

    // 销毁炸弹对象的方法
    void Destroy()
    {
        Destroy(gameObject);
    }
}
