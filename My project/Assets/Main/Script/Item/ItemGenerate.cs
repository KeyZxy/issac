using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerate : MonoBehaviour
{
    public List<GameObject> basicDrops = new List<GameObject>(); // �����������б�
    private bool generated = false;
    public float generateLength;
    public float generateWidth;

    void Start()
    {
        GenerateDrops();
    }

    void Update()
    {

    }

    public void GenerateDrops()
    {
        int ItemCount = Random.Range(2, 5); // �������2��5������������

        for (int i = 0; i < ItemCount; i++)
        {
            Collider2D[] Colliders;
            GameObject dropPrefab;
            float x;
            float y;
            do
            {
                x = Random.Range(-generateLength, generateLength);
                y = Random.Range(-generateWidth, generateWidth);
                int dropIndex = Random.Range(0, basicDrops.Count);
                dropPrefab = basicDrops[dropIndex];
                Vector2 generatorPosition = new Vector2(x, y);
                Colliders = Physics2D.OverlapBoxAll(generatorPosition, new Vector2(dropPrefab.transform.localScale.x / 2, dropPrefab.transform.localScale.y / 2), 0);
            }
            while (Colliders.Length != 0);
            //����
            GameObject item = Instantiate(dropPrefab, new Vector3(x, y, 0), Quaternion.identity);
            //Debug.Log("���ɽ�����Ʒ�ɹ�");
        }
    }
}
