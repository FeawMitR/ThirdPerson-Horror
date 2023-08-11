using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.PlayerControllerCharacter;

namespace TPSHorror.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField]
        private PlayerController player = null;

        // Start is called before the first frame update
        void Start()
        {
            player.StartOperation();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
