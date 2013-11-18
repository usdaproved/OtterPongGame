using System.Linq;
using System.Text;
using System;
using Otter;

namespace OtterPongGame {
    class PowerUp : Entity {
        Image imgPwrUp = Image.CreateRectangle(30, 30, Color.Gold);

        float rotationSpeed = (float)Math.PI * 5 / 2;

        public int type;

        bool collected;

        public PowerUp(int type) : base() {
            this.type = type;

            SetHitbox(30, 30, (int)Tags.PowerUp);
            Collider.CenterOrigin();

            SetGraphic(imgPwrUp);
            imgPwrUp.CenterOrigin();

            X = (int)((Rand.Float(0.5f) + 0.25f) * Game.Instance.Width);
            Y = (int)((Rand.Float(0.4f) + 0.3f) * Game.Instance.Height);

            Console.WriteLine(this.type);

            collected = false;
        }

        public override void Update()
        {
            base.Update();

            imgPwrUp.Angle += Util.SinScale(Timer, rotationSpeed, rotationSpeed * -(float)Math.PI / 6);
            if (!collected) {
                imgPwrUp.ScaleY = Util.SinScaleClamp(Timer, (float)Math.PI / 2, (float)Math.PI / 4);

                SetHitbox((int)imgPwrUp.ScaledWidth, (int)imgPwrUp.ScaledHeight, (int)Tags.PowerUp);
                Collider.CenterOrigin();
            }
        }

        public void Collected() {
            collected = true;
            RemoveCollider(Collider);

            Tween(imgPwrUp, new { Alpha = 0 }, 40).Ease(Ease.ElasticOut);
            Tween(imgPwrUp, new { ScaleX = 0, ScaleY = 0 }, 40).Ease(Ease.CubeOut).OnComplete(RemoveSelf);
        }
    }
}
