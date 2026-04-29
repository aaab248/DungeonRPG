using TMPro;
using UnityEngine;

using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [SerializeField] private GameObject MenuPanel;
    private bool setMenuUI = false;
    public Game_State game_state { get; private set; }

    public void Start()
    {
        game_state = Game_State.Playing;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Change_GameState(setMenuUI ? Game_State.Playing : Game_State.Stop);

            setMenuUI = !setMenuUI;
            MenuPanel.SetActive(setMenuUI);
        }
    }
    
    public void Change_GameState(Game_State state)
    {
        game_state = state;
    }

    public void GameOver()
    {
        
    }

}
