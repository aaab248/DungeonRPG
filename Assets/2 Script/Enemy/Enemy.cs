using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour,IDamgeableObj
{
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid;
    protected Animator animator;

    [SerializeField, Header("---상태 리스트---")]
    protected List<ScriptableState<Enemy, Enemy_StateType>> EnemyState_List;
    protected EnemyStateController enemyStateController;

    [SerializeField]
    private List<GameObject> ChildObj;

    private HealthBar healthBar; // 체력바
    private Coroutine healthBarRemainCor; // 체력바 코루틴 용

    #region 데이터 부분

    [Header("---Enemy 데이터---")]
    public string enemyID;

    public float currentHealth; // 현재 체력
    public float maxHealth; // 최대체력

    public float contactDmg; // 접촉 데미지
    public float attackDmg; // 공격 데미지

    public float chaseSpeed; // 쫓는 속도
    public float basicSpeed; // 기본 속도

    public float detectionRadius; // 탐지 범위

    [SerializeField]
    private Enemy_MovingType movingType; // 이동 타입
    public Enemy_MovingType MovingType => movingType;

    #endregion



    public float IdleDuration { get; set; }

    public Transform DetectTarget { get; private set; } = null;
    public Vector2 TargetVec { get; protected set; } // 이동 타겟 위치
    public Vector2 OriginVec { get; protected set; } // 원래 위치


    public float StateTimer { get; private set; } = 0f; // 상태 타이머
    public bool IsRoaming { get; protected set; } = false; // 로밍 확인
    public bool IsDetect { get; protected set; } = false; // 적 탐지 확인
    public bool IsHit { get; set; } = false; // 피격 확인
    public bool Isdie { get; protected set; } = false; // 사망 확인
    // 상태 전환 딜레이용
    public bool StateDelay { get; set; } = false;



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        enemyStateController = new EnemyStateController(this, EnemyState_List);
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }
    protected virtual void Update() { }
    protected virtual void FixedUpdate()
    {
        enemyStateController?.OnUpdate();
    }
    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-100 * transform.position.y);

        if(rigid.linearVelocityX != 0)
        {
            spriteRenderer.flipX = rigid.linearVelocityX < 0f;
        }
    }


    public void ResetStateTimer() => StateTimer = Time.time;
    public virtual void ResetCondition() => StateDelay = false;

    public void SetVelocity(Vector2 vec) => rigid.linearVelocity = vec;

    /// <summary>
    /// 타겟 벡터 = 로밍 랜덤 설정
    /// </summary>
    public void SetRoamingPosition() => TargetVec = Random.insideUnitCircle.normalized;
    /// <summary>
    /// 타겟 벡터 = 탐지 방향 설정
    /// </summary>
    public void SetChasingPosition() => TargetVec = (DetectTarget.position - transform.position).normalized;

    public void SetIsDetect(Transform target, bool val)
    {
        DetectTarget = target;
        IsDetect = val;
    }

    public abstract void Move(Vector2 targetVec);
    public abstract void Attack();
    public abstract void SpecialActing();

    public void TakeDamage(float damage, Vector2 vec)
    {
        // 체력바 표시
        if(healthBar == null)
        {
            healthBar = HealthBarPool.Instance.GetHealthBar(this);
        }
        else
        {
            StopCoroutine(healthBarRemainCor);
        }

        healthBarRemainCor = StartCoroutine(HealthBarTimer());
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StopCoroutine(healthBarRemainCor);
            HealthBarPool.Instance.ReturnHealthBar(healthBar);

            foreach (GameObject obj in ChildObj)
            {
                obj.SetActive(false);
            }

            Isdie = true;
        }
        else
        {
            IsHit = true;
        }
    }

    IEnumerator HealthBarTimer()
    {
        yield return new WaitForSeconds(2.5f);
        HealthBarPool.Instance.ReturnHealthBar(healthBar);

        healthBar = null;
        healthBarRemainCor = null;
    }

    // 접촉 시 공격
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Vector2 knockbackVec = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<IDamgeableObj>().TakeDamage(contactDmg, knockbackVec * contactDmg);
        }
    }

    // 애니메이션 종료 확인 값
    private bool animationEnd = false;

    // 애니메이션 종료 확인 (-> 첫 호출에만 true값 )
    public bool GetAnimationEnd 
    {
        get
        {
            if (animationEnd)
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
