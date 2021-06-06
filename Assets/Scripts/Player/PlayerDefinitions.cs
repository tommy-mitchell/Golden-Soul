using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLibrary
{
    public static class PlayerDefinitions
    {
        public static int THROWING_INACTIVE = 0;
        public static int THROWING_DOWN     = 1;
        public static int THROWING_SIDE     = 2;
        public static int THROWING_UP       = 3;

        public static float ATTACK_CD = 0.3f;

        public static float VERTICAL_THROW_OFFSET = -12; //Changes how far down mouse needs to be on player sprite to throw down
    }
}