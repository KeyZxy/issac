using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    #region singleTon
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    [Header("Text")]
    [SerializeField] Image trinket; // 장신구
    [SerializeField] Image active;  // 액티브
    [SerializeField] Text coinText; // 코인
    [SerializeField] Text bombText; // 폭탄
    [SerializeField] Text keyText;  // 열쇠
    [SerializeField] public Text deathCount; //목숨 카운트 [은총]
    [SerializeField] public Text guardCount; //가드 카운트 [은총]

    [Header("Player Stat")]
    [SerializeField] Text damageText;
    [SerializeField] Text moveSpeedText;
    [SerializeField] Text attackSpeedText;
    [SerializeField] Text attackRangeText;
    [SerializeField] Text tearSpeedText;

    [Header("Hearts")]
    [SerializeField] Transform heartUI; // 하트 UI 
    [SerializeField] GameObject emptyHeart; // 빈하트

    [Header("Active")]
    [SerializeField] Image activeEnergy;
    [SerializeField] Sprite nullImage;

    [Header("UI")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject optionUI;
    [SerializeField] GameObject gameoverUI;
    public GameObject BossCutSceneUI;
    [SerializeField] GameObject loadingImage;
    [SerializeField] Sprite[] volumeNum;
    [SerializeField] GameObject[] volumeObj;
    [SerializeField] Stuff pauseStuff;
    [SerializeField] Stuff gameOverStuff;
    [SerializeField] Sprite joinedSymbol;

    public GameObject LodingImage { get => loadingImage; }
    public Sprite JoinedSymbol { get => joinedSymbol; }

    [Header("Banner")]
    [SerializeField] Banner banner;


    private void Start()
    {
        SetPlayerMaxHP(); // 하트HP 초기세팅
        OnLoading();
    }

    private void Update()
    {
        //AddHeart(); // 최대체력 증가하는곳에서 호출해주세요, [은총] 일단 아이템 쪽에선 다 했음. 혹시 몰라서 지우진 않겠소
        //DelHeart(); // 최대체력 감소하는곳에서 호출해주세요, [은총] AddHeart랑 똑같음
        SetPlayerCurrentHP(); // 현재 체력이 감소 또는 증가할때 호출해주세요 ( GetDamage / GetHeart  등등 ) [은총] 아이템 쪽은 완료
        //UpdateActiveEnergy(); // 액티브 아이템 게이지 증가또는 감소(사용)할때 호출해주세요. [은총] 일단 확인 가능한 부분에서 적용 완료

        PauseUIOnOff(); // Pause UI
        UpdateUI(); // 각종 UI 업데이트
        GameOverButton(); // Esc, Space 버튼 기능
    }

    #region Loading
    public void OnLoading()
    {
        loadingImage.SetActive(true);
        StartCoroutine(FadeOutStart());
        //FadeOutStart();
    }
    IEnumerator FadeOutStart()
    {
        Image loadingColor = loadingImage.GetComponent<Image>();

        Color defaultColor = loadingColor.color;
        yield return new WaitForSeconds(1.2f);

        loadingColor.DOColor(new Color(0, 0, 0, 0), 3f);

        yield return new WaitForSeconds(3.2f);
        loadingColor.color = defaultColor;
        loadingImage.SetActive(false);
    }
    //IEnumerator FadeOutStart()
    //{
    //    Image lodingColor = loadingImage.GetComponent<Image>();
    //    Color defaultColor = lodingColor.color;
    //    yield return new WaitForSeconds(1.2f);
    //    for (float f = 1f; f > 0; f -= 0.005f)
    //    {
    //        lodingColor.color = new Color(0, 0, 0, f);
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(0.2f);
    //    lodingColor.color = defaultColor;
    //    loadingImage.SetActive(false);
    //}
    #endregion

    #region PauseUI
    public void PauseUIOnOff()
    {
        // 게임오버 UI가 꺼져있을떄 or 로딩 UI가 꺼져있을때
        if(!gameoverUI.activeSelf && !loadingImage.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pauseUI.activeSelf)
                {
                    pauseUI.SetActive(true);
                    pauseStuff.SetStuff();
                    Time.timeScale = 0;
                }

                else if(pauseUI.activeSelf)
                {
                    if (optionUI.activeSelf)
                    {
                        optionUI.SetActive(false);
                    }
                    else
                    {
                        pauseUI.SetActive(false);
                        Time.timeScale = 1;
                    }
                }
            }
        }
    }

    public void PauseExitBtn()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("01_Intro");
    }

    public void PauseResumeBtn()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    #endregion

    #region optionUI
    public void OptionUIOn()
    {
        optionUI.SetActive(true);
        InitVolumeImage();
    }

    void InitVolumeImage()
    {
        int[] volumes = SoundManager.instance.GetVolumes();
        volumeObj[0].GetComponent<Image>().sprite = volumeNum[volumes[0]];
        volumeObj[1].GetComponent<Image>().sprite = volumeNum[volumes[1]];
        volumeObj[2].GetComponent<Image>().sprite = volumeNum[volumes[2]];
    }

    public void MasterVolume(int increase)
    {
        int volume = SoundManager.instance.VolumeControl(0, increase);
        volumeObj[0].GetComponent<Image>().sprite = volumeNum[volume];
    }
    public void BGMVolume(int increase)
    {
        int volume = SoundManager.instance.VolumeControl(1, increase);
        volumeObj[1].GetComponent<Image>().sprite = volumeNum[volume];
    }
    public void SFXVolume(int increase)
    {
        int volume = SoundManager.instance.VolumeControl(2, increase);
        volumeObj[2].GetComponent<Image>().sprite = volumeNum[volume];
    }
    #endregion

    #region GameOverUI
    public void GameOver()
    {
        gameoverUI.SetActive(true);
        gameOverStuff.SetStuff();
        Time.timeScale = 0;
    }

    public void GameOverRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("02_Game");
    }

    public void GameOverButton()
    {
        if (gameoverUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("02_Game");
                gameoverUI.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("01_Intro");
                gameoverUI.SetActive(false);
            }
        }
    }
    #endregion

    #region HP
    public void SetPlayerMaxHP()
    {
        for (int i = 0; i < PlayerManager.instance.playerMaxHp/2; i++)
        {
            GameObject eheart = Instantiate(emptyHeart) as GameObject;
            eheart.transform.SetParent(heartUI);
        }
    }

    public void AddHeart() // 최대체력 증가
    {
        int addCount = (PlayerManager.instance.playerMaxHp / 2) - heartUI.childCount;
        for (int i = 0; i < addCount; i++)
        {
            GameObject eheart = Instantiate(emptyHeart) as GameObject;
            eheart.transform.SetParent(heartUI);
        }
        SetPlayerCurrentHP();
    }

    public void DelHeart() // 최대 체력 감소
    {
        int delCount = heartUI.childCount - (PlayerManager.instance.playerMaxHp / 2);
        for (int i = 0; i < delCount; i++)
        {
            Destroy(heartUI.GetChild(i).gameObject);
        }
        SetPlayerCurrentHP();
    }

    public void SetPlayerCurrentHP()
    {
        int tmp = PlayerManager.instance.playerHp;
        for(int i = 0; i < heartUI.childCount; i++)
        {
            if(tmp >= 2)
            {
                heartUI.GetChild(i).GetComponent<UIHeart>().SetHeart(2);
                tmp -= 2;
            }
            else if(tmp >= 1)
            {
                heartUI.GetChild(i).GetComponent<UIHeart>().SetHeart(1);
                tmp -= 1;
            }
            else
            {
                heartUI.GetChild(i).GetComponent<UIHeart>().SetHeart(0);
            }
        }
    }
    #endregion

    public void UpdateActiveEnergy()
    {
        if(ItemManager.instance.ActiveItem != null)
        {
            ActiveInfo active = ItemManager.instance.ActiveItem.GetComponent<ActiveInfo>();
            // 필요한 게이지가 없을때 액티브아이템 게이지를 꽉 채워줌.
            if(active.needEnergy == 0)
            {
                activeEnergy.fillAmount = 1;
            }
            else
            {
                activeEnergy.fillAmount = (float)active.currentEnergy / (float)active.needEnergy;
            }
        }
        // 보유중인 액티브 아이템이 없을때 
        // 액티브 아이템 게이지를 0으로 설정
        else
        {
            activeEnergy.fillAmount = 0;
        }
    }

    // UI 업데이트 
    public void UpdateUI()
    {
        ItemUpdate();
        DropItemUpdate();
        PlayerStatUpdate();
    }

    // 드랍 아이템 현황 업데이트
    public void DropItemUpdate()
    {
        coinText.text = ItemManager.instance.coinCount.ToString();
        bombText.text = ItemManager.instance.bombCount.ToString();
        keyText.text = ItemManager.instance.keyCount.ToString();
        deathCount.text = PlayerManager.instance.deathCount.ToString(); //은총
        guardCount.text = PlayerManager.instance.CanBlockDamage.ToString(); //은총
    }

    // 보유 아이템 현황 업데이트
    public void ItemUpdate()
    {
        if (ItemManager.instance.TrinketItem != null)
        {
            trinket.sprite = ItemManager.instance.TrinketItem.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            trinket.sprite = nullImage;
        }

        if (ItemManager.instance.ActiveItem != null)
        {
            active.sprite = ItemManager.instance.ActiveItem.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            active.sprite = nullImage;
        }
    }

    // 플레이어 스탯 업데이트
    public void PlayerStatUpdate()
    {
        damageText.text = PlayerManager.instance.playerDamage.ToString("0.00");
        moveSpeedText.text = PlayerManager.instance.playerMoveSpeed.ToString("0.00");
        attackSpeedText.text = (1 / PlayerManager.instance.playerShotDelay).ToString("0.00");
        attackRangeText.text = PlayerManager.instance.playerRange.ToString("0.00");
        tearSpeedText.text = PlayerManager.instance.playerTearSpeed.ToString("0.00");
    }


    #region banner
    public IEnumerator StageBanner(int stage)
    {
        yield return new WaitForSeconds(2.5f);
        banner.StageBanner(stage);
    }

    public void ItemBanner(string title, string info)
    {
        banner.ItemBanner(title,info);
    }
    #endregion
}
