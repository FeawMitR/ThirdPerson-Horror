using UnityEngine;
using TPSHorror.PlayerControllerCharacter;
using TPSHorror.UserInterface;
using TPSHorror.GameManager.Zone;
using TPSHorror.Interaction;

namespace TPSHorror.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Ui Game Start")]
        [SerializeField]
        private UIStartGame m_UIStartGame = null;

        [Header("Zone")]
        [SerializeField]
        private ZoneManagement m_MainZone = null;
        [SerializeField]
        private ZoneManagement m_RoomYellow = null;

        [SerializeField]
        private ZoneManagement m_RoomOrange = null;

        private Door m_FinalDoor = null;

        private int m_KeyItemCount = 0;
        private int m_MaxKeyItem = 3;
        [Header("Player")]
        [SerializeField]
        private PlayerController m_player = null;

        //[Header("")]

        // Start is called before the first frame update
        void Start()
        {
            m_UIStartGame.ButtonStartGame.onClick.AddListener(GameStart);
            m_UIStartGame.Show();
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        public void GameStart()
        {
            m_UIStartGame.ButtonStartGame.onClick.RemoveListener(GameStart);
            m_player.StartOperation();
            m_UIStartGame.Hide();

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
            Debug.Log($"Start Room Orange");
        }
    }
}
