using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Otter;

namespace OtterPongGame {
    class Paddle : Entity {
        public StateMachine paddleSM = new StateMachine();

        int speed = 6;

        int pwrCountdown = 0;
        int pwrDuration = 1200;

        public int
            stateShortened,
            stateLengthened,
            stateSpeedBoosted,
            stateSpeedSlowed,
            stateNormal;

        Session player;

        Image imgPaddle = Image.CreateRectangle(10, 100);

        Image imgHealthBar = Image.CreateRectangle(100, 10, Color.Red);

        Image imgHealthBarFill = Image.CreateRectangle(100, 10, new Color("b1b1b1"));

        Image imgHealthBarBg = Image.CreateRectangle(110, 20);

        public Paddle(Session player) : base() {
            this.player = player;

            AddComponent(paddleSM);

            paddleSM.AddState("Shortened","Lengthened","SpeedBoosted","SpeedSlowed","Normal");
            paddleSM.ChangeState(stateNormal);

            SetGraphic(imgPaddle);
            imgPaddle.CenterOrigin();

            SetHitbox(10, 100, (int)Tags.Paddle);

            if (player.Id == 0) { // player 1
                X = 50;
                imgHealthBar.X = imgHealthBarBg.X = imgHealthBarFill.X = X + 50;
            }
            else { // player 2
                X = Game.Instance.Width - 50;
                imgHealthBar.X = imgHealthBarBg.X = imgHealthBarFill.X = X - 50;
            }

            imgHealthBar.Y = imgHealthBarBg.Y = imgHealthBarFill.Y = 30;
            imgHealthBarFill.CenterOrigin();
            imgHealthBarBg.CenterOrigin();
            Y = Game.Instance.HalfHeight;
        }

        public override void Update() {
            base.Update();

            if (player.Controller.Up.Down) {
                Y -= speed;
            }
            if (player.Controller.Down.Down) {
                Y += speed;
            }

            if (player.Id == 0) {
                imgHealthBar.ScaleX = Global.PlayerOneHealth / 100.0f;
            }
            else {
                imgHealthBar.ScaleX = Global.PlayerTwoHealth / 100.0f;
            }

            Y = Util.Clamp(Y, imgPaddle.ScaledHeight/2, Game.Instance.Height - imgPaddle.ScaledHeight/2);

            Collider.CenterOrigin();
            imgPaddle.CenterOrigin();

            imgHealthBar.CenterOrigin();
        }

        public override void Render() {
            base.Render();

            //Render the health bar here.
            Draw.Graphic(imgHealthBarBg, 0, 0);
            Draw.Graphic(imgHealthBarFill, 0, 0);
            Draw.Graphic(imgHealthBar, 0, 0);
        }

        // STATE STUFFS

        void EnterNormal() {
            
        }

        void UpdateNormal() {
            
        }

        void ExitNormal() {
            
        }

        void EnterShortened() {
            pwrCountdown = pwrDuration;

            // graphical things
            imgPaddle.ScaleX = 0.5f;
            Tween(imgPaddle, new { ScaleY = 0.3, ScaleX = 1 }, 60).Ease(Ease.ElasticOut);
           
            SetHitbox(10, 33, (int)Tags.Paddle);
        }

        void UpdateShortened() {
            if (pwrCountdown > 0) {
                pwrCountdown--;
                if (pwrCountdown == 0) {
                    paddleSM.ChangeState(stateNormal);
                }
            }
        }

        void ExitShortened() {
            // Grow paddle
            imgPaddle.ScaleX = 1.5f;
            Tween(imgPaddle, new { ScaleY = 1, ScaleX = 1 }, 60).Ease(Ease.ElasticOut);

            SetHitbox(10, 100, (int)Tags.Paddle);
        }

        void EnterLengthened()
        {
            pwrCountdown = pwrDuration;

            imgPaddle.ScaleX = 1.5f;
            Tween(imgPaddle, new { ScaleY = 2, ScaleX = 1 }, 60).Ease(Ease.ElasticOut);

            SetHitbox(10, 200, (int)Tags.Paddle);
        }

        void UpdateLengthened()
        {
            if (pwrCountdown > 0)
            {
                pwrCountdown--;
                if (pwrCountdown == 0)
                {
                    paddleSM.ChangeState(stateNormal);
                }
            }
        }

        void ExitLengthened()
        {
            imgPaddle.ScaleX = 0.5f;
            Tween(imgPaddle, new { ScaleY = 1, ScaleX = 1 }, 60).Ease(Ease.ElasticOut);

            SetHitbox(10, 100, (int)Tags.Paddle);
        }

        void EnterSpeedBoosted()
        {
            pwrCountdown = pwrDuration;

            Tween(this, new { speed = 12 }, 120).Ease(Ease.CircIn);
        }

        void UpdateSpeedBoosted()
        {
            if (pwrCountdown > 0)
            {
                pwrCountdown--;
                if (pwrCountdown == 0)
                {
                    paddleSM.ChangeState(stateNormal);
                }
            }
        }

        void ExitSpeedBoosted()
        {
            Tween(this, new { speed = 6 }, 120).Ease(Ease.CircIn);
        }

        void EnterSpeedSlowed()
        {
            pwrCountdown = pwrDuration;

            Tween(this, new { speed = 3 }, 120).Ease(Ease.CircIn);
        }

        void UpdateSpeedSlowed()
        {
            if (pwrCountdown > 0)
            {
                pwrCountdown--;
                if (pwrCountdown == 0)
                {
                    paddleSM.ChangeState(stateNormal);
                }
            }
        }

        void ExitSpeedSlowed()
        {
            Tween(this, new { speed = 6 }, 120).Ease(Ease.CircIn);
        }
    }
}
