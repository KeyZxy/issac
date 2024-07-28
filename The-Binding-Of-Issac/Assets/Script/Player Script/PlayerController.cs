
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    public Animator playerMoveAnim;
    public Animator playerShotAnim;
    public Animator playerAnim;
    public Animator getItem;

    [Header("Transform")]
    public Transform itemPosition;
    public Transform head;
    public Transform body;
    public Transform useActiveItemImage;
    public Transform knifePosition;
    public Transform familiarPosition;
    public Transform tearPointTransform;
    public Transform bombPointTransform;
    public Transform DrBombPointTransform;

    [Header("Sprite")]
    SpriteRenderer bodyRenderer;
    SpriteRenderer headRenderer;
    SpriteRenderer headItem;
    Rigidbody2D playerRB;
    public Sprite tearDefaultSprite;
    public Sprite bombDefaultSprite;

    [Header("Function")]
    private float lastshot;
    Vector2 moveInput;
    Vector2 shotInput;
    public float tearY;

    [Header("PlayerState")]
    public GameObject HeadItem;
    public GameObject CheckedObject;
    public GameObject tear;
    public GameObject putBomb;
    public GameObject DrBomb;
    public TearPoint tearPoint;
    GameObject DefaultTearObject;

    [Header("ItemState")]
    public GameObject knife;
    public bool bombState;
    public bool nailActivated; // ��� �������� ������� ��
    public bool canUseActive = true; //��Ƽ�� ������ �������� �����ϱ� ����
    public bool canChangeItem = false; //��Ƽ�� ������ ���� �����ϸ� ����

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] hitClips;
    public AudioClip[] deadClips;
    public AudioClip getItemClip;
    public AudioClip ShootClip;

    Stack<GameObject> tearPool;
    Stack<GameObject> putBombPool;
    Stack<GameObject> DrBombPool;
    GameObject tearPoolChild;
    GameObject DrBombChild;
    GameObject putBombChild;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerRB = GetComponent<Rigidbody2D>();
        bodyRenderer = body.GetComponent<SpriteRenderer>();
        headRenderer = head.GetComponent<SpriteRenderer>();
        headItem = GameManager.instance.playerObject.transform.GetChild(6).gameObject.GetComponent<SpriteRenderer>();

        canUseActive = true; // ��Ƽ�� ������ �������� �����ϱ� ����
        canChangeItem = true; // ��Ƽ�� ������ ���� �����ϸ� ����
        nailActivated = false;
        tearPool = new Stack<GameObject>();
        putBombPool = new Stack<GameObject>();
        DrBombPool = new Stack<GameObject>();
        SetTearPooling();
        SetBombPooling();
        SetDrBombPooling();
        //knifePosition.gameObject.SetActive(false);
    }

    void Update()
    {
        MoveAnim();
        ShotAnim();
        InstallBomb();
        UseActive();
    }
    void FixedUpdate()
    {
        Movement();
    }
    #region Dr_BombPooling

    public void SetDrBombPooling()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject DrbombObj = Instantiate(DrBomb, bombPointTransform.position, Quaternion.identity);
            DrBombPool.Push(DrbombObj);
            DrbombObj.transform.SetParent(DrBombPointTransform.transform);
            DrbombObj.gameObject.SetActive(false);
        }
        DrBombChild = DrBombPool.Pop();
    }
    public GameObject GetDrBombPooling()
    {
        if (DrBombPool.Count == 0)
        {
            GameObject DrbombObj = Instantiate(DrBombChild, bombPointTransform.position, Quaternion.identity);
            DrBombPool.Push(DrbombObj);
            DrbombObj.transform.SetParent(DrBombPointTransform.transform);
            DrbombObj.gameObject.SetActive(false);
        }
        GameObject DrbombObject = DrBombPool.Pop();
        DrbombObject.SetActive(true);
        return DrbombObject;
    }
    public void ReturnDrBombPooling(GameObject bombObj)
    {
        bombObj.GetComponent<SpriteRenderer>().sprite = bombDefaultSprite;
        bombObj.transform.localPosition = Vector3.zero;
        //bombObj.transform.localScale = Vector3.one;
        bombObj.SetActive(false);
        bombObj.GetComponent<PutBomb>().CanAttack = false;
        bombObj.GetComponent<BoxCollider2D>().offset = new Vector2(0.04f, -0.03f);
        bombObj.GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 0.64f);
        DrBombPool.Push(bombObj);
    }
    #endregion

    #region putBombPooling

    public void SetBombPooling()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject bombObj = Instantiate(putBomb, bombPointTransform.position, Quaternion.identity);
            putBombPool.Push(bombObj);
            bombObj.transform.SetParent(bombPointTransform.transform);
            bombObj.gameObject.SetActive(false);
        }
        putBombChild = putBombPool.Pop();
    }
    public GameObject GetBombPooling()
    {
        if (putBombPool.Count == 0)
        {
            GameObject bombObj = Instantiate(putBombChild, bombPointTransform.position, Quaternion.identity);
            putBombPool.Push(bombObj);
            bombObj.transform.SetParent(bombPointTransform.transform);
            bombObj.gameObject.SetActive(false);
        }
        GameObject bombObject = putBombPool.Pop();
        bombObject.SetActive(true);
        return bombObject;
    }
    public void ReturnBombPooling(GameObject bombObj)
    {
        bombObj.GetComponent<SpriteRenderer>().sprite = bombDefaultSprite;
        bombObj.transform.localPosition = Vector3.zero;
        bombObj.SetActive(false);
        bombObj.GetComponent<PutBomb>().CanAttack = false;
        bombObj.GetComponent<BoxCollider2D>().isTrigger = true;
        bombObj.GetComponent<BoxCollider2D>().offset = new Vector2(0.04f, -0.03f);
        bombObj.GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 0.64f);
        putBombPool.Push(bombObj);
    }
    #endregion

    #region tearPooling
    public void SetTearPooling()
    {
        for (int i = 0; i < 40; i++)
        {
            GameObject tearObj = Instantiate(tear, tearPointTransform.position, Quaternion.identity);
            tearPool.Push(tearObj);
            SetSFXObject(tearObj);
            tearObj.transform.SetParent(tearPoint.transform);
            tearObj.gameObject.SetActive(false);
        }
        tearPoolChild = tearPool.Pop();
    }
    public GameObject GetTearPooling()
    {
        if (tearPool.Count == 0)
        {
            GameObject tearObj = Instantiate(tearPoolChild, tearPointTransform.position, Quaternion.identity);
            tearPool.Push(tearObj);
            SetSFXObject(tearObj);
            tearObj.GetComponent<AudioSource>().volume = SoundManager.instance.GetSFXVolume();
            tearObj.transform.SetParent(tearPoint.transform);
            tearObj.gameObject.SetActive(false);
        }
        GameObject tearObject = tearPool.Pop();
        tearObject.SetActive(true);
        tearObject.GetComponent<Tear>().SetPlayerPosition(transform.position);
        return tearObject;
    }

    public void ReturnTearPooling(GameObject bullet)
    {
        bullet.GetComponent<SpriteRenderer>().sprite = tearDefaultSprite;
        //bullet.transform.localScale = new Vector3(1, 1, 1);
        bullet.transform.localPosition = Vector3.zero;
        bullet.GetComponent<CircleCollider2D>().enabled = true;
        bullet.GetComponent<SpriteRenderer>().sortingOrder = 103;
        bullet.SetActive(false);
        tearPool.Push(bullet);
    }
    #endregion

    #region UseActiveItem
    //��Ƽ�� ������ ���
    void UseActive()
    {
        // �������� �ְ�, �����̽��� ��������
        if (ItemManager.instance.ActiveItem != null && canUseActive)
        {
            if (ItemManager.instance.ActiveItem.GetComponent<ActiveInfo>().canUse)
            {
                ItemManager.instance.ActiveItem.GetComponent<ActiveInfo>().CheckedItem();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ActiveInfo active = ItemManager.instance.ActiveItem.GetComponent<ActiveInfo>();

                    if (active.currentEnergy >= active.needEnergy) // �ʿ� ������ �Ѿ�����.
                    {
                        if (active.activeItemCode == 1 && ItemManager.instance.coinCount <= 0)
                        {
                            return;
                        }
                        else
                        {
                            StartCoroutine(UseActiveItem()); // ������ ��� �ִϸ��̼�
                            active.UseActive();  // ������ ��� ����
                            if (active.activeItemCode == 0)
                            {
                                nailActivated = true;
                            }
                        }
                        active.currentEnergy = 0;
                        UIManager.instance.UpdateActiveEnergy();
                        canUseActive = false;
                        Invoke("SetActiveDelay", 1f);
                        Invoke("SetCanChangeItem", 1f);
                    }
                }
            }
        }
    }

    void SetActiveDelay()
    {
        canUseActive = true;
    }
    
    void SetCanChangeItem()
    {
        canChangeItem = true;
    }
    #endregion

    #region PlayerFunction
    //�̵� ���
    void Movement()
    {
        //�̵��ӵ�
        float moveSpeed = PlayerManager.instance.playerMoveSpeed;
        //�߻� ������
        float shotDelay = PlayerManager.instance.playerShotDelay;

        //���� �̵� Ű�Է�
        float hori = Input.GetAxis("Horizontal");
        //���� �̵� Ű�Է�
        float verti = Input.GetAxis("Vertical");
        //�Է����� �� �̵� ���� ����
        moveInput = hori * Vector2.right + verti * Vector2.up;
        //�밢 �̵��ӵ� 1 �ѱ��� �ʱ�
        if(moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
        //���� �߻� Ű �Է�
        float shootHor = Input.GetAxis("ShootHorizontal");
        //���� �߻� Ű �Է�
        float shootVer = Input.GetAxis("ShootVertical");

        shotInput = shootHor * Vector2.right + shootVer * Vector2.up;
        if (ItemManager.instance.PassiveItems[13] && !ItemManager.instance.PassiveItems[16])
        {
            KnifeAttack(hori,verti,shootHor, shootVer);
        }
        else
        {
            //�Ѿ� �߻� ������ ������ �߻�� �����̸� ���� ����ð�(�ʴ���)�� �Ѿ�� ����
            if ((shootHor != 0 || shootVer != 0) && Time.time > lastshot + shotDelay)
            {
                //���γ� ���� �Է��� ���� �� ��
                if (shootHor != 0 && shootVer != 0)
                {
                    //�밢 �߻� X
                    shootHor = 0;
                }
                Shoot(shootHor, shootVer);
                //������ �߻翡 ���� �ð�(�ʴ���)�� ����
                lastshot = Time.time;
            }
        }
        //�÷��̾� ������
        playerRB.velocity = moveInput * moveSpeed;
    }
    //�߻� ���
    public void Shoot(float x, float y)
    {
        //���� Y��
        tearY = y;
        //���� �߻� �ӵ�
        float tearSpeed = PlayerManager.instance.playerTearSpeed;
        //���� ���� ����
        Vector3 firePoint = tearPoint.transform.position;

        if (ItemManager.instance.PassiveItems[16])
        {
            DefaultTearObject = DrFetusBomb();
            bombState = true;
        }
        else
        {
            DefaultTearObject = GetTearPooling();
        }
        
        DefaultTearObject.transform.position = firePoint;
        //������ ������ �����ӵ� ���ؼ� ���ֱ�
        DefaultTearObject.GetComponent<Rigidbody2D>().velocity = new Vector3(x * tearSpeed, y * tearSpeed, 0);
        if (ItemManager.instance.PassiveItems[9] && !ItemManager.instance.PassiveItems[16])
        {
            if (x > 0) //������
            {
                DefaultTearObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (x < 0) //����
            {
                DefaultTearObject.transform.rotation = Quaternion.Euler(180, 0, 90);
            }
            if (y > 0) //��
            {
                DefaultTearObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (y < 0) //�Ʒ�
            {
                DefaultTearObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        CheckedObject = null;
        if (y != 1) // �� ������ �ƴҶ�
        {
            CheckedObject = tearPoint.overLapObject;
        }

        //������ �̵��ӵ��� ������ ����
        //�ش� �̵�Ű�� ������ 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Rigidbody2D rigid_bullet = DefaultTearObject.GetComponent<Rigidbody2D>();
            //�ش� �������� ���� ��
            rigid_bullet.AddForce(moveInput * 2.5f, ForceMode2D.Impulse);
        }
        
        //2�� �нú� �������� ������
        if (ItemManager.instance.PassiveItems[2])
        {
            //�ش� �Լ� 4�� ����
            for (int i = 0; i < 3; i++)
                MutantShoot(x, y);
        }
    }
    public void MutantShoot(float x, float y)
        {
        //���� �߻� �ӵ�
        float tearSpeed = PlayerManager.instance.playerTearSpeed;
        //���� ���� ����
        Vector3 firePoint = tearPoint.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

        //16�� �нú� ������ ������
        if (ItemManager.instance.PassiveItems[16])
        {
            DefaultTearObject = DrFetusBomb();
            bombState = true;
        }
        else
        {
            DefaultTearObject = GetTearPooling();
        }

        DefaultTearObject.transform.position = firePoint;
        //������ ������ �����ӵ� ���ؼ� ���ֱ�
        DefaultTearObject.GetComponent<Rigidbody2D>().velocity = new Vector3(x * tearSpeed, y * tearSpeed, 0);
        if (ItemManager.instance.PassiveItems[9] && !ItemManager.instance.PassiveItems[16])
        {
            if (x > 0) //������
            {
                DefaultTearObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (x < 0) //����
            {
                DefaultTearObject.transform.rotation = Quaternion.Euler(180, 0, 90);
            }
            if (y > 0) //��
            {
                DefaultTearObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (y < 0) //�Ʒ�
            {
                DefaultTearObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        CheckedObject = null;
        if (y != 1) // �� ������ �ƴҶ�
        {
            CheckedObject = tearPoint.overLapObject;
        }

        //������ �̵��ӵ��� ������ ����
        //�ش� �̵�Ű�� ������ 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Rigidbody2D rigid_bullet = DefaultTearObject.GetComponent<Rigidbody2D>();
            //�ش� �������� ���� ��
            rigid_bullet.AddForce(moveInput * 2.5f, ForceMode2D.Impulse);
        }
    }

    //���� �ִϸ��̼� Ŭ�� �̺�Ʈ�� �������� �߰� �Ǿ�����

    public void KnifeAttack(float moveX, float moveY, float shootX, float shootY)
    {
        if(knife.GetComponent<KnifeObject>().canShoot)
        {
            if (shootX > 0) //������
            {
                knifePosition.localPosition = new Vector3(0.94f, 0.2f, 0);
                knife.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (shootX < 0) //����
            {
                knifePosition.localPosition = new Vector3(-0.84f, 0.2f, 0);
                knife.transform.rotation = Quaternion.Euler(180, 0, 90);
            }
            if (shootY > 0) //��
            {
                knifePosition.localPosition = new Vector3(0, 1.31f, 0);
                knife.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (shootY < 0) //�Ʒ�
            {
                knifePosition.localPosition = new Vector3(0, -0.69f, 0);
                knife.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

    //��ź ��ġ ���
    void InstallBomb()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            // �ʵ忡 ��ġ�� ��ź�̾����� && �������� ��ź ������ 1�� �̻��϶�
            if(GameObject.Find("Putbomb") == null && ItemManager.instance.bombCount > 0)
            {
                GameObject bomb = GetBombPooling();
                bomb.GetComponent<PutBomb>().PlayerBomb();
                ItemManager.instance.bombCount--;
                bombState = false;
                bomb.name = "Putbomb"; // ������ ��ź ������Ʈ �̸� ����
            }
        }
    }

    public GameObject DrFetusBomb()
    {
        GameObject bomb = GetDrBombPooling();
        bomb.GetComponent<PutBomb>().PlayerBomb();
        return bomb;
    }
    #endregion

    #region PlayerAnim
    //�̵� �ִϸ��̼�
    void MoveAnim()
    {
        //�����Է¹��� -> -1 ���� �ִϸ��̼� ������(����)
        if (moveInput.x < 0) { bodyRenderer.flipX = true; headRenderer.flipX = true; }
        //�����Է¹��� -> 1 ���� �ִϸ��̼� �״��(������)
        if (moveInput.x > 0) { bodyRenderer.flipX = false; headRenderer.flipX = false; }
        //�����Է� ����
        playerMoveAnim.SetFloat("Up&Down", Mathf.Abs(moveInput.y));
        //�����Է� ����
        playerMoveAnim.SetFloat("Left&Right", Mathf.Abs(moveInput.x));
        playerShotAnim.SetFloat("Left&RightLook", Mathf.Abs(moveInput.x));

        if (moveInput.y > 0)
        {
            playerShotAnim.SetBool("UpLook", true); // ���� ����
        }
        else
        {
            playerShotAnim.SetBool("UpLook", false);
        }
        if (moveInput.y < 0)
        {
            playerShotAnim.SetBool("DownLook", true); // �Ʒ��� ����
        }
        else
        {
            playerShotAnim.SetBool("DownLook", false);
        }
    }

    //�߻� �ִϸ��̼�
    void ShotAnim()
    {
        if (shotInput.x < 0) { headRenderer.flipX = true; }
        if (shotInput.x > 0) { headRenderer.flipX = false; }
        playerShotAnim.SetFloat("Left&RightShot", Mathf.Abs(shotInput.x));

        if (shotInput.y > 0)
        {
            playerShotAnim.SetBool("UpShot", true); // ���� ����
        }
        else
        {
            playerShotAnim.SetBool("UpShot", false);
        }
        if (shotInput.y < 0)
        {
            playerShotAnim.SetBool("DownShot", true); // �Ʒ��� ����
        }
        else
        {
            playerShotAnim.SetBool("DownShot", false);
        }
    }

    //�ǰ� �ִϸ��̼�
    public void Hit()
    {
        //�Ӹ� ����
        headRenderer.color = new Color(1, 1, 1, 0);
        //���� ����
        bodyRenderer.color = new Color(1, 1, 1, 0);
        //�Ӹ��� ������ ������ ����
        headItem.color = new Color(1, 1, 1, 0);
        //�ǰ� �ִϸ��̼� ����
        playerAnim.SetTrigger("Hit");
        //�ǰ� ���� ����
        HitSound();
    }

    // ��� �ִϸ��̼�
    public void Dead()
    {
        //��� ���� ����
        DeadSound();

        //player head, player body ������Ʈ ã�Ƽ� ����
        head.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
        //��� �ִϸ��̼� ����
        playerAnim.SetTrigger("Death");
    }
    #endregion

    #region GetItemAnim
    //������ ȹ�� �ִϸ��̼�
    public IEnumerator GetItemTime()
    {
        //���� head, body,headitem ����� ������
        headRenderer.color = new Color(1, 1, 1, 0);
        bodyRenderer.color = new Color(1, 1, 1, 0);
        headItem.color = new Color(1, 1, 1, 0);
        //������ ȹ�� �ִϸ��̼� ����
        getItem.SetTrigger("GetItem");
        //�ִϸ��̼� 1�ʰ� ����
        yield return new WaitForSeconds(1f);
        //�÷��̾� ��� ���̰� ��
        headRenderer.color = new Color(1, 1, 1, 1);
        bodyRenderer.color = new Color(1, 1, 1, 1);
        headItem.color = new Color(1, 1, 1, 1);

        //itemPosition �ڽ��� �����
        if (itemPosition.childCount != 0)
        {
            Destroy(itemPosition.GetChild(0).gameObject);
        }
    }

    public IEnumerator GetTrinketItem()
    {
        GetitemSound();
        //���� ����� ������
        headRenderer.color = new Color(1, 1, 1, 0);
        bodyRenderer.color = new Color(1, 1, 1, 0);
        headItem.color = new Color(1, 1, 1, 0);
        //������ ȹ�� �ִϸ��̼� ����
        getItem.SetTrigger("GetItem");
        //�ִϸ��̼� 1�ʰ� ����
        yield return new WaitForSeconds(1f);
        //�÷��̾� ��� ���̰� ��
        headRenderer.color = new Color(1, 1, 1, 1);
        bodyRenderer.color = new Color(1, 1, 1, 1);
        headItem.color = new Color(1, 1, 1, 1);
        ItemManager.instance.TrinketItem.GetComponent<TrinketInfo>().KeepItem();
    }

    public IEnumerator GetActiveItem()
    {
        GetitemSound();
        headRenderer.color = new Color(1, 1, 1, 0);
        bodyRenderer.color = new Color(1, 1, 1, 0);
        headItem.color = new Color(1, 1, 1, 0);

        getItem.SetTrigger("GetItem");
        yield return new WaitForSeconds(1f);

        headRenderer.color = new Color(1, 1, 1, 1);
        bodyRenderer.color = new Color(1, 1, 1, 1);
        headItem.color = new Color(1, 1, 1, 1);
        ItemManager.instance.ActiveItem.GetComponent<ActiveInfo>().KeepItem();
    }

    public IEnumerator UseActiveItem()
    {
        Sprite activeSpr = ItemManager.instance.ActiveItem.GetComponent<SpriteRenderer>().sprite;
        useActiveItemImage.GetComponent<SpriteRenderer>().sprite = activeSpr;
        headRenderer.color = new Color(1, 1, 1, 0);
        bodyRenderer.color = new Color(1, 1, 1, 0);
        headItem.color = new Color(1, 1, 1, 0);

        getItem.SetTrigger("GetItem");
        yield return new WaitForSeconds(1f);

        headRenderer.color = new Color(1, 1, 1, 1);
        bodyRenderer.color = new Color(1, 1, 1, 1);
        headItem.color = new Color(1, 1, 1, 1);
        useActiveItemImage.GetComponent<SpriteRenderer>().sprite = null;
    }
    #endregion

    #region Sound
    public void HitSound()
    {
        int randomIndex = Random.Range(0, hitClips.Length);

        audioSource.clip = hitClips[randomIndex];
        audioSource.Play();
    }
    public void DeadSound()
    {
        int randomIndex = Random.Range(0, deadClips.Length);

        audioSource.clip = deadClips[randomIndex];
        audioSource.Play();
    }

    public void GetitemSound()
    {
        audioSource.clip = getItemClip;
        audioSource.Play();
    }

    void SetSFXObject(GameObject obj)
    {
        if(obj.GetComponent<AudioSource>() != null)
        {
            SoundManager.instance.sfxObjects.Add(obj.GetComponent<AudioSource>());
        }
    }
    #endregion
}