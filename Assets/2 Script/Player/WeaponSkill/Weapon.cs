using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Animator animator;

    public GameObject ChargingEffect_Prefab;
    public GameObject UsingEffect_Prefab;
    public GameObject AppearEffect_Prefab;

    [SerializeField] private Transform player;

    private bool isHolding =false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Using");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isHolding = true;
            animator.SetBool("Charging", isHolding);
        }
        if(isHolding && Input.GetKeyDown(KeyCode.E) )
        {
            ChargingEffect_Prefab.SetActive(true);
        }
        if(isHolding&& Input.GetKeyUp(KeyCode.E))
        {
            isHolding = false;
            ChargingEffect_Prefab.SetActive(false);
            animator.SetTrigger("Using");
            animator.SetBool("Charging", isHolding);
        }
    }

}
    