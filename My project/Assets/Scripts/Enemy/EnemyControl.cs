using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("LocalControl")]
    public List<GameObject> EnemyList = new List<GameObject>();
    public Transform PointPos;

    [Header("EnemyType")]
    public EnemyType enemy;
    public enum Enemytype { Spider, Clotty, Fatty, RoundWorm };
    public static Enemytype enemyType;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUpEnemy(int EnemyNum, Transform SetUpPos)//随机生成EnemyNum个怪
    {
        for (int i = 0; i < EnemyNum; i++)
        {
            enemyType = (Enemytype)UnityEngine.Random.Range(0, 4);
            switch (enemyType)
            {
                case Enemytype.Spider:
                    EnemyList.Add(Instantiate(enemy.Spider, new Vector3(SetUpPos.position.x + UnityEngine.Random.Range(-8.0f, 8.0f), SetUpPos.position.y + UnityEngine.Random.Range(-3.0f, 3.0f), SetUpPos.position.z), Quaternion.identity));
                    break;
                case Enemytype.Clotty:
                    EnemyList.Add(Instantiate(enemy.Clotty, new Vector3(SetUpPos.position.x + UnityEngine.Random.Range(-8.0f, 8.0f), SetUpPos.position.y + UnityEngine.Random.Range(-3.0f, 3.0f), SetUpPos.position.z), Quaternion.identity));
                    break;
                case Enemytype.RoundWorm:
                    EnemyList.Add(Instantiate(enemy.RoundWorm, new Vector3(SetUpPos.position.x + UnityEngine.Random.Range(-8.0f, 8.0f), SetUpPos.position.y + UnityEngine.Random.Range(-3.0f, 3.0f), SetUpPos.position.z), Quaternion.identity));
                    break;
                case Enemytype.Fatty:
                    EnemyList.Add(Instantiate(enemy.Fatty, new Vector3(SetUpPos.position.x + UnityEngine.Random.Range(-8.0f, 8.0f), SetUpPos.position.y + UnityEngine.Random.Range(-3.0f, 3.0f), SetUpPos.position.z), Quaternion.identity));
                    break;
            }
        }
    }

}
[System.Serializable]
public class EnemyType
{
    public GameObject Spider;
    public GameObject Clotty;
    public GameObject Fatty;
    public GameObject RoundWorm;

}
