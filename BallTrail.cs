using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Otter;

namespace OtterPongGame {
    class BallTrail : Entity {

        Image imgBall = Image.CreateCircle(7, Color.Cyan);
        Speed speed = new Speed(10, 10);
        bool explosion;

        public BallTrail(float x, float y, bool explosion = false) : base(x, y) {
            this.explosion = explosion;
            SetGraphic(imgBall);
            imgBall.CenterOrigin();

            speed.X = Rand.Float(0, 10);
            speed.Y = Rand.Float(0, 10);

            if (X > Game.Instance.Width - 55)
                speed.X *= -1;
            else if (Rand.Chance(0.5f))
                speed.X *= -1;
            if (Rand.Chance(0.5f))
                speed.Y *= -1;
            if (explosion)
            {
                imgBall.Color = Color.Random;
                Tween(imgBall, new { ScaleX = 0, ScaleY = 0 }, 60).Ease(Ease.BounceOut);
                LifeSpan = 60;
            } else
                LifeSpan = 20;

            //Tween(speed, new { X = 0, Y = 0 }, 60).Ease(Ease.ElasticOut);
            Tween(speed, new { X = 0, Y = 0 }, 60).Ease(Ease.CubeOut);

            Layer = 100;
        }

        public override void Update() {
            base.Update();

            if (explosion)
            {
                X += speed.X;
                Y += speed.Y;
            }
            else
                imgBall.Scale = Util.ScaleClamp(Timer, 0, LifeSpan, 1, 0);
            
            imgBall.Alpha = Util.ScaleClamp(Timer, 0, LifeSpan, 1, 0);
           
        }
    }
}
