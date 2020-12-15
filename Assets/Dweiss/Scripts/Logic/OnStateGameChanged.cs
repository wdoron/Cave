using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dweiss
{
    public class OnStateGameChanged : MonoBehaviour
    {
        public int[] correctLevels;


        public Dweiss.EventBool onCorrectStateBool;

        public Dweiss.EventEmpty onCorrectState;
        public Dweiss.EventEmpty onIncorrectState;

        private const string LevelChangedId = "LevelSelected";

        private void LevelSelected(int newLvlNum)
        {
            var isCorrectLvl = correctLevels.Contains(newLvlNum);
            if (isCorrectLvl) onCorrectState.Invoke();
            else onIncorrectState.Invoke();

            onCorrectStateBool.Invoke(isCorrectLvl);
        }

        private void OnEnable()
        {
            Msg.MsgSystem.Register(LevelChangedId, LevelSelected);
        }
        private void OnDisable()
        {
            if(Msg.MsgSystem.S)
                Msg.MsgSystem.Unregister(LevelChangedId, LevelSelected);
        }
    }
}