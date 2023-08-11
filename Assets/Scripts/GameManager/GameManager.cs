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
        private ZoneManagement m_ZoneOne = null;

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
            StartZoneOne();
        }


        private void StartZoneOne()
        {
            //Start Zone One
            Debug.Log($"Start Zone One");
            m_ZoneOne.onZoneFinishedEvent += OnZoneOneFinishedHadler;
            m_ZoneOne.StartZone();
        }

        private void OnZoneOneFinishedHadler()
        {
            m_MainZone.onZoneFinishedEvent -= OnZoneOneFinishedHadler;
            Debug.Log($"Collect Second Key Start Zone Two");
            
        }
    }
}
