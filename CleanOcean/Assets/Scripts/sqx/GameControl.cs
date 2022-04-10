using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {
	//单例模式(实例化这个类)
    private static GameControl _instance;

    public static GameControl Instance
    {
        get
        {
            return _instance;
        }
    }


    
	public Text textCost;
	public Text moneyText;
	public Text lvText;
	public Text lvNameText;
	public Text smallCountdownText;
	public Text bigCountdownText;
	public Button backButton;
	public Button settingButton;
	public Button bigCountdownButton;
    public Button levelNext;
	public Slider expSlider;
    public Color moneyColor;

    public GameObject scriptHolder;
	public GameObject lvUpdate_Ef;
	public GameObject lvUpTip_Ef;
	public GameObject changeGun_Ef;
	public GameObject money_Ef;
    public GameObject levelNextPanel;

    public int LV=0;
	public int exp=0;
	public int money=1000;
	public const int bigCountdown = 20;
	public const int samllCountdown = 240;
	public float bigTimer = bigCountdown;
	public float smallTime = samllCountdown;

	public Transform bulletHolder;

	public int bgIndex=0;
	public Image bgImage;
	public GameObject seaWave;
	public Sprite[] bgSprites;//4个背景图（更换）

	public GameObject[] bullet1Cos;
	public GameObject[] bullet2Cos;
	public GameObject[] bullet3Cos;
	public GameObject[] bullet4Cos;
	public GameObject[] bullet5Cos;
	public GameObject[] gunCos;
	//使用的是第几档的炮
	public int costIndex = 0;
	//每一炮所需的金币数和造成的伤害值
	private int[] oneShootCosts = {5,10,20,30,40,50,60,70,80,90,100,200,300,400,500,600,700,800,900,1000};
	private string[] lvName = {"小白","渔夫","水手","黑铁","青铜","黄金","钻石","白金","荣耀","海王"};
    #region 数据初始化
    void Start()
    {
        money = PlayerPrefs.GetInt("money", money);
        LV = PlayerPrefs.GetInt("lv", LV);
        exp = PlayerPrefs.GetInt("exp", exp);
        smallTime = PlayerPrefs.GetFloat("scd", smallTime);
        bigTimer = PlayerPrefs.GetFloat("bcd", bigTimer);

        UpdateUI();
    }
    #endregion
    //各种方法调用
    void Update(){
		//加钱BUG
		Bug();

		OneBulletCost();
		Fire();
		UpdateUI();
		ChangeBg ();
        ChooseLevel();
    }
    #region  //更换背景
    void ChangeBg()
    {
        if (bgIndex != LV % 10)
        {
            bgIndex = LV % 10;
            if (bgIndex % 10 == 0)
            {
                bgImage.sprite = bgSprites[9];
                Audiomanager.Instance.PlayEffectSound(Audiomanager.Instance.seaWaveClip);
                Instantiate(seaWave);
            }
            else
            {
                bgImage.sprite = bgSprites[bgIndex];
            }
        }
    } 
    #endregion

    //更新ui（倒计时 钱等的）显示
    void UpdateUI()
    {
		bigTimer -= Time.deltaTime;
		smallTime -= Time.deltaTime;
		if (smallTime<=0) {
			smallTime = samllCountdown;
			money += 50;
		}
		if (bigTimer<=0&&bigCountdownText.gameObject.activeSelf==true) {
            bigCountdownText.gameObject.SetActive(false);
            bigCountdownButton.gameObject.SetActive(true);
        }
		//经验等级换算：升级所需经验=1000+200*当前等级
		while (exp >= 1000+200*LV)
        {
			exp = exp - (1000+200*LV);
			LV++;
			//升级提示面板显示
			lvUpTip_Ef.SetActive(true);
			lvUpTip_Ef.transform.Find ("TipText").GetComponent<Text>().text = LV.ToString();
			Audiomanager.Instance.PlayEffectSound (Audiomanager.Instance.LvUpClip);
            //实例化升级提示特效
			Instantiate(lvUpdate_Ef);
			//升级提示面板隐藏
			StartCoroutine(lvUpTip_Ef.GetComponent<Ef_Setactive>().HideSelf(0.5f));
		}
     
        //文字头衔和等级挂钩
        moneyText.text = "￥"+money;
		lvText.text = LV.ToString ();
		if ((LV / 1) <= 9)
        {
			lvNameText.text = lvName [LV / 1];
		} 
		else
        {
			lvNameText.text = lvName [9];
		}

		smallCountdownText.text = " "+(int)smallTime/10+" "+(int)smallTime%10;
		bigCountdownText.text = (int)bigTimer+"s";
		expSlider.value = ((float)exp) / (1000 + 200 * LV);
    }

    //点击进入下一关面板
    void ChooseLevel()
    {
        if (LV > 9)
        {
            levelNextPanel.SetActive(true);
            Destroy(scriptHolder.GetComponent<RubbishMaker>());
            Destroy(scriptHolder.GetComponent<FishMaker>());
        }
    }
    

    #region 鼠标滚轮控制加减炮的方法 
    void OneBulletCost()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            OnButtonD();
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            OnButtonP();
        }
    } 
    #endregion

    #region 加炮 减炮
    //1.两个方法，加炮 减炮
    public void OnButtonP()
    {
        gunCos[costIndex / 4].SetActive(false);
        costIndex++;
        Audiomanager.Instance.PlayEffectSound(Audiomanager.Instance.changeGunCilp);
        //changeGun_Ef
        Instantiate(changeGun_Ef);
        costIndex = (costIndex > oneShootCosts.Length - 1) ? 0 : costIndex;
        gunCos[costIndex / 4].SetActive(true);
        textCost.text = "￥" + oneShootCosts[costIndex];
    }
    public void OnButtonD()
    {
        gunCos[costIndex / 4].SetActive(false);
        costIndex--;
        costIndex = (costIndex < 0) ? oneShootCosts.Length - 1 : costIndex;
        gunCos[costIndex / 4].SetActive(true);
        textCost.text = "￥" + oneShootCosts[costIndex];
    }
    #endregion

    #region  开火发射子弹
    void Fire()
    {
        GameObject[] useBullets = bullet5Cos;//为了防止case不到值，使useBullets=一个值（如果出错就让它为最大号炮）
        int bulletIndex;
        //EventSystem.current.IsPointerOverGameObject()==false只有点鱼才发射炮弹
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            //如果钱够打炮 就可以发射子弹
            if (money - oneShootCosts[costIndex] >= 0)
            {
                //知道用哪一种子弹
                switch (costIndex / 4)
                {
                    case 0: useBullets = bullet1Cos; break;
                    case 1: useBullets = bullet2Cos; break;
                    case 2: useBullets = bullet3Cos; break;
                    case 3: useBullets = bullet4Cos; break;
                    case 4: useBullets = bullet5Cos; break;
                }
                //子弹和等级挂钩
                bulletIndex = (LV % 10 >= 9) ? 9 : LV % 10;//随等级1到9，调用的 子弹显示图片 也改变
                money -= oneShootCosts[costIndex];
                Audiomanager.Instance.PlayEffectSound(Audiomanager.Instance.fireClip);
                GameObject bullet = Instantiate(useBullets[bulletIndex]);
                bullet.transform.SetParent(bulletHolder, false);//子弹不应该再与原来的父物体一致,跟炮口的位置一致
                bullet.transform.position = gunCos[costIndex / 4].transform.Find("FirePos").transform.position;
                bullet.transform.rotation = gunCos[costIndex / 4].transform.Find("FirePos").transform.rotation;
                bullet.GetComponent<BulletAttribute>().damage = oneShootCosts[costIndex];//子弹伤害和钱对应
                bullet.AddComponent<AutoMove>().dir = Vector3.up;
                bullet.GetComponent<AutoMove>().speed = bullet.GetComponent<BulletAttribute>().speed;//上句代码已添加组件 AddComponent，这里直接使用 GetComponent 
            }
            //钱不够文本闪烁
            else
            {
                //钱不够文本闪烁
                StartCoroutine(MoneyNotEnough());
            }
        }

    }
    #endregion

    #region 点击“奖赏”加钱
    public void BonusButtonDown()
    {
        money += 500;
        Audiomanager.Instance.PlayEffectSound(Audiomanager.Instance.rewardClip);
        Instantiate(money_Ef);
        bigCountdownButton.gameObject.SetActive(false);
        bigCountdownText.gameObject.SetActive(true);
        bigTimer = bigCountdown;
    } 
    #endregion
    //单例模式（实例化）
    void Awake()
    {
        _instance = this;
    }
    //协程控制金币不够闪烁提示
    IEnumerator  MoneyNotEnough()
    {
        moneyText.color = moneyColor;
        moneyText.color = Color.red;
        yield return new WaitForSeconds(0.8f);
        moneyText.color = moneyColor;

    }
    #region 加钱BUG
    void Bug()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            money += 10000;
        }
    } 
    #endregion
}
