using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDamgeableObj
{
    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [HideInInspector] public SkillPoint sp;

    [SerializeField, Header("---State---")]
    private List<ScriptableState<Player, Player_StateType>> playerState_List;
    private PlayerStateController stateController;

    #region 플레이어 입력 값

    public Vector2 InputMove { get; private set; } // 이동 키
    public bool InputDash { get; private set; } // 대시 키
    public bool InputAttack { get; private set; } // 공격 키

    #endregion

    #region 플레이어 condition 값

    public bool IsHit { get; set; } = false;
    public bool IsDead { get; set; } = false;
    public bool IsInvincible { get; private set; } = false;

    #endregion

    private Coroutine myCoroutine;

    [Header("---플레이어 데이터---")]
    public float maxHealth; // 최대 체력
    public float currentHealth; // 현재 체력
    public float moveSpeed; // 이동 속도
    public float dashSpeed; // dash 속도

    public float flipDuration; // 방향전환 회전 속도
    public float dashCoolDown; // 대시 쿨타임

    [SerializeField]
    private int currentDir = 1;
    private int queuedDir = 0;


    public float EndDashTime { get; set; } = -999f;// 대시 시작 시간

    public LayerMask hitLayer;

    private void Awake()
    {
        rigid = GetComponent <Rigidbody2D>();
        animator = GetComponent <Animator>();
        spriteRenderer = GetComponent <SpriteRenderer>();

        sp = GetComponent<SkillPoint>();

        stateController = new PlayerStateController(this, playerState_List);
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        isFlipping = false;
        currentDir = 1;
        transform.eulerAngles = Vector3.zero;

        StopAllCoroutines();
    }

    private void Update()
    {
        // 키 입력들 확인
        InputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        InputDash = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        InputAttack = Input.GetKeyDown(KeyCode.Space);
        stateController?.OnUpdate();
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-100 * transform.position.y);
    }

    [SerializeField]
    private float spawnInterval = 0.05f;
    private float afterImageTimer;

    /// <summary>
    /// 대시 잔상 이펙트 생성
    /// </summary>
    public void CreateDashAfterImage()
    {
        afterImageTimer -= Time.deltaTime;

        if(afterImageTimer <= 0f)
        {
            // 잔상 이미지 가져오기
            AfterImagePool.Instance.GetAfterImage(spriteRenderer.sprite, transform.position, transform.eulerAngles, 
                transform.localScale, spriteRenderer.color);

            afterImageTimer = spawnInterval;
        }
    }

    /// <summary>
    /// 플레이어 이동
    /// </summary>
    /// <param name="vec"></param>
    public void SetVelocity(Vector2 vec)
    {
        rigid.linearVelocity = vec;
    }

    #region 무적 상태

    public void SetInvincible(bool condition, float holdTime)
    {
        // holdtime 만큼 유지후 무적off
        if (condition == false)
        {
            myCoroutine = StartCoroutine(HoldInvincible(holdTime));
        }
        // 무적on
        else
        {
            if(myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.6f);
            IsInvincible = true;
        }
    }

    IEnumerator HoldInvincible(float time)
    {
        yield return new WaitForSeconds(time);

        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        IsInvincible = false;
    }

    #endregion

    #region 대쉬 파트

    /// <summary>
    /// IsDash setter
    /// </summary>
    /// <param name="dash"></param>

    // 대시 사용 관련 컨디션
    public SkillCondition DashCondition
    {
        get
        {
            if (Time.time - EndDashTime < dashCoolDown)
                return SkillCondition.CoolDown;
                
            else if (!sp.CanUse(1))
                return SkillCondition.NotEnoughPoint;

            else
                return SkillCondition.Ready;
        }
    }

    #endregion

    #region 공격 파트

    public ComboAttackData attackDatas;

    public bool canNextAttack = false;
    public bool attackInputTime = false;
    public int attackIndex = 0;

    
    // 플레이어 공격
    public void DoAttack()
    {
        Vector2 hitboxOffset = attackDatas.comboAttackData[attackIndex].hitBoxOffset;
        Vector2 hitboxSize = attackDatas.comboAttackData[attackIndex].hitBoxSize;

        Vector2 vec = new Vector2(transform.position.x + (currentDir * hitboxOffset.x), transform.position.y + hitboxOffset.y);

        Collider2D[] hits = Physics2D.OverlapBoxAll(vec, hitboxSize, 0f, hitLayer);

        foreach (Collider2D collider in hits)
        {
            IDamgeableObj target = collider.GetComponent<IDamgeableObj>();

            Vector2 knockbackDir = (collider.transform.position - transform.position).normalized;
            target.TakeDamage(attackDatas.comboAttackData[attackIndex].damage, knockbackDir);

            // 공격 시 데미지 비례 스킬포인트 획득
            sp.AddCharge(attackDatas.comboAttackData[attackIndex].damage * 0.2f);
        }   

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 hitboxOffset = attackDatas.comboAttackData[attackIndex].hitBoxOffset;
        Vector2 hitboxSize = attackDatas.comboAttackData[attackIndex].hitBoxSize;

        Vector2 center = new Vector2(transform.position.x + (currentDir * hitboxOffset.x), transform.position.y + hitboxOffset.y);

        Vector2 size = hitboxSize;

        Gizmos.DrawWireCube(center, size);
    }

    // 공격 선입력 체크 활성화
    public void EnableAttackInputTIme()
    {
        attackInputTime = true;
    }

    #endregion

    /// <summary>
    /// 플레이어 피격
    /// </summary>
    public void TakeDamage(float damage, Vector2 knockback)
    {
        // 무적일 경우 무시
        if (IsInvincible)
        {
            return;
        }

        currentHealth -= damage;

        // 체력 0 이하 플레이어 사망
        if (currentHealth <= 0 )
        {
            IsDead = true;
        }
        else
        {
            IsHit = true;
        }

        SetVelocity(Vector2.zero);

        rigid.AddForce(knockback, ForceMode2D.Impulse);

    }

    #region 캐릭터 스프라이트 회전

    private bool isFlipping = false;
    public void TryFlip(int targetDir)
    {
        // 입력 방향, 바라보는 방향 동일할때 return
        if (targetDir == currentDir)
            return;

        if(!isFlipping)
        {
            StartCoroutine(Flip(targetDir));
        }
        else
        {
            queuedDir = targetDir;
        }
    }
    IEnumerator Flip(int targetDir)
    {
        isFlipping = true;
        queuedDir = 0;
        currentDir = targetDir;

        float elasped = 0f;
        float startY = transform.rotation.eulerAngles.y;
        float endY = startY + 180f;

        // 스프라이트 반바퀴 회전
        while(elasped < flipDuration)
        {
            elasped += Time.deltaTime;
            float yRotation = Mathf.Lerp(startY, endY, elasped / flipDuration);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, endY, 0);

        isFlipping = false;

        // 대기중인 방향전환 flip이 있다면 실행
        if(queuedDir != 0 &&  queuedDir != currentDir)
        {
            StartCoroutine (Flip(queuedDir));
        }
    }

    #endregion

    private bool animationEnd;
    public bool GetAnimationEnd
    {
        get
        {
            if(animationEnd)
            {
                animationEnd = false;
                return true;
            }
            return false;
        }
    }

    public void EndAnimation() => animationEnd = true;

    public void SetAnimation(string name, bool condition)
    {
        animator.SetBool(name, condition);
    }
    public void SetAnimation(string name)
    {
        animator.SetTrigger(name);  
    }
    public void SetAnimation(string name, int index)
    {
        animator.SetInteger(name, index);
    }
}
