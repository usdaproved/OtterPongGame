using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Otter;

namespace OtterPongGame {
    class Ball : Entity {

        Image imgBall = Image.CreateCircle(7);

        Paddle paddle = null;

        Speed speed = new Speed(10, 10);

        int startCountdown = 0;
        int startTime = 60;

        public Ball() : base() {
            SetHitbox(7, 7, (int)Tags.Ball);
            Collider.CenterOrigin();

            SetGraphic(imgBall);
            imgBall.CenterOrigin();

            X = Game.Instance.HalfWidth;
            Y = Game.Instance.HalfHeight;

            startCountdown = startTime;
        }

        public override void Update() {
            base.Update();

            if (startCountdown > 0) {
                startCountdown--;
                if (startCountdown == 0) {
                    Start();
                }
            }

            X += speed.X;
            Y += speed.Y;

            var c = Collider.Collide(X, Y, (int)Tags.Paddle);
            if (c != null) {
                paddle = c.Entity as Paddle;
                speed.X *= -1;
                speed.Y = (Y - paddle.Y) * 0.5f;
                imgBall.Scale = 3;
                Tween(imgBall, new { ScaleX = 1, ScaleY = 1 }, 60).Ease(Ease.ElasticOut);
                
                for (int i = 0; i < Rand.Int(5, 15); i++)
                {
                    Scene.Add(new BallTrail(X, Y, true));
                }
            }

            var pu = Collider.Collide(X, Y, (int)Tags.PowerUp);
            if (pu != null) {
                var pwrUp = pu.Entity as PowerUp;
                // Set graphical things to happen and corresponding paddle to change
                for (int i = 0; i < Rand.Int(5, 15); i++)
                    Scene.Add(new BallTrail(X, Y, true));

                // TODO: Set up Switch case for types of pwrups.
                if (paddle != null) {
                    switch (pwrUp.type) { 
                        case (int)PowerUps.Shorten:
                            paddle.paddleSM.ChangeState(paddle.stateShortened);
                            break;
                        case (int)PowerUps.Lengthen:
                            paddle.paddleSM.ChangeState(paddle.stateLengthened);
                            break;
                        case (int)PowerUps.SpeedBoost:
                            paddle.paddleSM.ChangeState(paddle.stateSpeedBoosted);
                            break;
                        case (int)PowerUps.SpeedDecrement:
                            paddle.paddleSM.ChangeState(paddle.stateSpeedSlowed);
                            break;
                        case (int)PowerUps.RotateScene:
                            var ps = Scene as PongScene;
                            ps.RotateScene(90.0f);
                            break;
                    }
                }
                    

                pwrUp.Collected();
            }

            if (Y < 0) {
                speed.Y *= -1;
            }
            if (Y > Game.Instance.Height) {
                speed.Y *= -1;
            }

            if (X > Game.Instance.Width) {
                // player 1 scores
                Global.PlayerTwoHealth -= 10;
                Score();
            }
            if (X < 0) {
                // player 2 scores
                Global.PlayerOneHealth -= 10;
                Score();
            }

            Scene.Add(new BallTrail(X, Y, false));
        }

        public void Score() {
            startCountdown = startTime;
            speed.X = 0;
            speed.Y = 0;
            X = Game.Instance.HalfWidth;
            Y = Game.Instance.HalfHeight;
            paddle = null;
        }

        public void Start() {
            speed.X = Rand.Choose(-5, 5);
        }


    }
}
