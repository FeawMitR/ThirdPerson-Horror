using UnityEngine;
using TPSHorror.PlayerControllerCharacter;
using TPSHorror.UserInterface;

namespace TPSHorror.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Ui Game Start")]
        [SerializeField]
        private UIStartGame m_UIStartGame = null;

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
        }
    }
}
