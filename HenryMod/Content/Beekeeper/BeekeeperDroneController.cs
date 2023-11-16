using RoR2;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

namespace FirstLightMod.Content.Beekeeper
{
    public class BeekeeperDroneController : MonoBehaviour
    {

        //Put this on pause, Equipment is not working right.


        CharacterBody body;
        CharacterMaster master;

        TeamComponent teamComponent;

        InputBankTest inputBank;


        private void Start()
        {
            body = base.GetComponent<CharacterBody>();
            master = body.master;
            inputBank = base.GetComponent<InputBankTest>();
            teamComponent = base.GetComponent<TeamComponent>();
        }

        private void FixedUpdate()
        {

            //Update Drone position to be on the left and right of beekeeper

        }


    }
}

