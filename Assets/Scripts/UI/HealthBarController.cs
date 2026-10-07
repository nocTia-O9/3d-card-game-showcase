using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    //血条相关
    public Transform healthBarTrans;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;
    private CharacterBase currentCharacter;
    //防御相关
    private VisualElement defenseElement;
    private Label defenseAmountLabel;
    //buff相关
    private VisualElement buffElement;
    private Label buffRoundLabel;
    [Header("Buff图片")]
    public Sprite buffSprite;
    public Sprite deBuffSprite;
    //敌人意图
    private Enemy enemy;
    private VisualElement intentElement;
    private Label intentLabel;   

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();    
        enemy = GetComponent<Enemy>();
    }

    //反复进入房间需要反复启动所以使用OnEnable
    private void OnEnable()
    {
        InitHealthBar();
    }
    
    private void MoveToWorldPosition(VisualElement element,Vector3 worldPos,Vector2 size)
    {
        //坐标转换
        Rect rect=RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPos, size,Camera.main);        
        element.transform.position=rect.position;        
    }
    
    /// <summary>
    /// 初始化血条
    /// </summary>
    public void InitHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.highValue = currentCharacter.MaxHP;
        //因为血条大小已经设置好了，所以传一个Vector2.zero即可
        MoveToWorldPosition(healthBar, healthBarTrans.position, Vector2.zero);

        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseAmountLabel = defenseElement.Q<Label>("DefenseAmount");
        defenseElement.style.display=DisplayStyle.None;

        buffElement = healthBar.Q<VisualElement>("Buff");
        buffRoundLabel = buffElement.Q<Label>("BuffRound");
        buffElement.style.display=DisplayStyle.None;

        intentElement = healthBar.Q<VisualElement>("Intent");
        intentLabel = intentElement.Q<Label>("IntentAmount");
        intentElement.style.display=DisplayStyle.None;
    }
    private void Update()
    {
        UpdateHealthBar();        
    }

    //更新血条
    public void UpdateHealthBar()
    {
        if (currentCharacter.isDead)
        {
            //角色死亡，关闭血条显示
            healthBar.style.display = DisplayStyle.None;
            return;
        }
        if(healthBar!=null)
        {
            healthBar.title = currentCharacter.CurrentHP + "/" + currentCharacter.MaxHP;
            healthBar.value = currentCharacter.CurrentHP;

            //根据百分比改变血条颜色
            //先移除这几个类
            healthBar.RemoveFromClassList("highHP");
            healthBar.RemoveFromClassList("mediumHP");
            healthBar.RemoveFromClassList("lowHP");

            var percentage=(float)currentCharacter.CurrentHP/(float)currentCharacter.MaxHP;

            if(percentage < 0.3f)
            {
                healthBar.AddToClassList("lowHP");
            }
            else if(percentage < 0.6f)
            {
                healthBar.AddToClassList("mediumHP");
            }
            else
            {
                healthBar.AddToClassList("highHP");
            }                
        }

        //防御显示更新
        defenseElement.style.display = currentCharacter.defense.currentValue>0? DisplayStyle.Flex: DisplayStyle.None;
        defenseAmountLabel.text=currentCharacter.defense.currentValue.ToString();

        //buff回合更新
        buffElement.style.display=currentCharacter.buffRound.currentValue>0? DisplayStyle.Flex : DisplayStyle.None;
        buffElement.style.backgroundImage=currentCharacter.baseStrength>1?new StyleBackground(buffSprite):new StyleBackground(deBuffSprite);
        buffRoundLabel.text=currentCharacter.buffRound.currentValue.ToString();
    }

    /// <summary>
    /// 监听：在玩家回合开始时，显示敌人意图 或者受到buff影响时改变显示时调用
    /// </summary>
    public void SetIntentElement()
    {
        intentElement.style.display = DisplayStyle.Flex;
        intentElement.style.backgroundImage=new StyleBackground(enemy.currentAction.intentSprite);

        //判断是否是攻击
        var value = enemy.currentAction.effect.value;
        if (enemy.currentAction.effect.GetType() == typeof(DamageEffect))
        {
            value = (int)Mathf.Round(enemy.currentAction.effect.value * enemy.baseStrength);
        }
        intentLabel.text = value.ToString();
    }

    /// <summary>
    /// 监听：敌人回合结束，隐藏意图
    /// </summary>
    public void HideIntentElement()
    {
        intentElement.style.display = DisplayStyle.None;
    }
}
