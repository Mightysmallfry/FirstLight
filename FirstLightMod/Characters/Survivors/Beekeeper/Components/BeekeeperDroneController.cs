using RoR2;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

namespace FirstLightMod.Survivors.Beekeeper.Components
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
            body = GetComponent<CharacterBody>();
            master = body.master;
            inputBank = GetComponent<InputBankTest>();
            teamComponent = GetComponent<TeamComponent>();
        }

        private void FixedUpdate()
        {

            //Update Drone position to be on the left and right of beekeeper

        }


    }
}

