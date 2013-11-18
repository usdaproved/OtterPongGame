using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Otter;

namespace OtterPongGame {
    class Global {

        public static Session
            PlayerOne,
            PlayerTwo;

        public static int
            PlayerOneHealth = 100,
            PlayerTwoHealth = 100;
    }

    public enum Tags {
        Paddle,
        Ball,
        PowerUp
    }

    public enum PowerUps {
        Shorten,
        Lengthen,
        SpeedBoost,
        SpeedDecrement,
        RotateScene
    }
}
