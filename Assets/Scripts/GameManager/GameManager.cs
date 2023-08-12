using UnityEngine;
using TPSHorror.PlayerControllerCharacter;
using TPSHorror.UserInterface;
using TPSHorror.CoreManager.Zone;
using TPSHorror.Interaction;
using TPSHorror.FinishedGame;
using UnityEngine.SceneManagement;
using TPSHorror.Audio;

namespace TPSHorror.CoreManager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        private bool m_IsFinished = false;
        public enum FishedGameType : byte
        {
            Win,GameOver
        }

        private FishedGameType m_FinishedGame = FishedGameType.Win;

        [Header("Ui Game Start")]
        [SerializeField]
        private UIStartGame m_UIStartGamePrefab = null;
        private UIStartGame m_UIStartGame = null;

        [Header("Ui End Game")]
        [SerializeField]
        private UIEndGame m_UIEndGamePrefab = null;
        private UIEndGame m_UIEndGame = null;

        [Header("Zone")]
        [SerializeField]
        private ZoneManagement m_MainZone = null;
        [SerializeField]
        private ZoneManagement m_RoomYellow = null;

        [SerializeField]
        private ZoneManagement m_RoomOrange = null;

        [SerializeField]
        private Door m_FinalDoor = null;

        [SerializeField]
        private FinishedGamePoint m_FinishedPoint = null;

        [Header("Player")]
        [SerializeField]
        private PlayerController m_player = null;

        [Header("Audio")]
        [SerializeField]
        private AudioClip m_StartGameVoice = null;

        [SerializeField]
        private AudioClip m_EndGameWin = null;
        [SerializeField]
        private AudioClip m_EndGameOver = null;

        // Start is called before the first frame update
        void Start()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            m_UIStartGame = Instantiate(m_UIStartGamePrefab,CanvasInstance.Instance.Canvas.transform);

            m_UIStartGame.ButtonStartGame.onClick.AddListener(GameStart);
            m_UIStartGame.Show();

            m_UIEndGame = Instantiate(m_UIEndGamePrefab, CanvasInstance.Instance.Canvas.transform);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }




        public void GameStart()
        {
            m_UIStartGame.ButtonStartGame.onClick.RemoveListener(GameStart);
           
            m_player.onPlayerWasCaughtEvent += FinishedGameOver;
            m_UIStartGame.Hide();

            ResumeGame();

            AudioManager.Instance.PlayAtWorldPosition(m_StartGameVoice,false,m_player.transform.position);
            StartMainZone();
        }


        private void StartMainZone()
        {
            m_MainZone.onZoneFinishedEvent += OnMainZoneFinishedHadler;
            m_MainZone.StartZone();
        }

        private void OnMainZoneFinishedHadler()
        {
            m_MainZone.onZoneFinishedEvent -= OnMainZoneFinishedHadler;
            Debug.Log($"Collect First Key");
            StartRoomYellow();
        }


        private void StartRoomYellow()
        {
            //Start Zone One
            Debug.Log($"Start Room Yellow");
            m_RoomYellow.onZoneFinishedEvent += OnRoomYellowFinishedHadler;
            m_RoomYellow.StartZone();
        }

        private void OnRoomYellowFinishedHadler()
        {
            m_RoomYellow.onZoneFinishedEvent -= OnRoomYellowFinishedHadler;
            Debug.Log($"Collect Second Key Start Zone Two");
            StartRoomOrange();
        }


        private void StartRoomOrange()
        {
            m_RoomOrange.StartZone();
            m_RoomOrange.onZoneFinishedEvent += OnRoomOrangeFinishedHadler;
            Debug.Log($"Start Room Orange");
        }

        private void OnRoomOrangeFinishedHadler()
        {
            m_RoomOrange.onZoneFinishedEvent -= OnRoomOrangeFinishedHadler;
            Debug.Log($"Room Orange Finished");
            m_FinalDoor.StartOperating();
            ReadyFinishedGameWin();
        }

        private void ReadyFinishedGameWin()
        {
            m_FinishedPoint.StartOperation();
            m_FinishedPoint.onPlayerTriggerEvent += OnPlayerTrigger ;
        }

        private void OnPlayerTrigger()
        {
            m_FinishedPoint.onPlayerTriggerEvent -= OnPlayerTrigger;
            FinishedGameWin();
        }

        private void FinishedGameWin()
        {
            if (!m_IsFinished)
            {
                m_IsFinished = true;
                m_FinishedGame = FishedGameType.Win;
                Debug.Log($"FinishedGameWin");
                FishedGame();

                AudioManager.Instance.PlayAtWorldPosition(m_EndGameWin, false, m_player.transform.position);
            }
        }

        private void FinishedGameOver()
        {
            if (!m_IsFinished)
            {
                m_player.onPlayerWasCaughtEvent -= FinishedGameOver;
                m_IsFinished = true;
                m_FinishedGame = FishedGameType.GameOver;
                Debug.Log($"FinishedOver");
                FishedGame();

                AudioManager.Instance.PlayAtWorldPosition(m_EndGameOver, false, m_player.transform.position);
            }
        }


        private void FishedGame()
        {
            PauseGame();

            m_UIEndGame.EndGameTypeText.text = $"{m_FinishedGame}";
            m_UIEndGame.ButtonReStartGame.onClick.AddListener(RestartGame);

            m_UIEndGame.Show();
        }

        private void RestartGame()
        {
            m_UIEndGame.Hide();
            m_UIEndGame.ButtonReStartGame.onClick.RemoveListener(RestartGame);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        public void PauseGame()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            m_player.StopOperation();
        }

        public void ResumeGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_player.StartOperation();
        }
    }
}
