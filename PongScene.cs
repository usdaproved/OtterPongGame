using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Otter;

namespace OtterPongGame {
    class PongScene : Scene {

        int pwrUpTimer = Rand.Int(200, 1500);
        int rotateTime = 1200;
        int rotateTimer = 0;

        public PongScene() : base() {
            Add(new Paddle(Global.PlayerOne));
            Add(new Paddle(Global.PlayerTwo));
            Add(new Ball());

            Add(new PowerUp((int)PowerUps.RotateScene));
        }

        public override void Update() {
            base.Update();

            if (pwrUpTimer > 0 && GetCount<PowerUp>() < 3) {
                pwrUpTimer--;
                if (pwrUpTimer == 0) {
                    Add(new PowerUp((int)Rand.Int(0,5)));

                    pwrUpTimer = Rand.Int(200, 1500);
                }
            }

            if (rotateTimer > 0) {
                rotateTimer--;
                if (rotateTimer == 0)
                    Tween(this, new { CameraAngle = 0 }, 150).Ease(Ease.ElasticOut);
            }
        }

        public void RotateScene(float angle) {
            Tween(this, new { CameraAngle = angle }, 150).Ease(Ease.ElasticOut);

            rotateTimer = rotateTime;
        }
    }
}
