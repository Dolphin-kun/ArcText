using Vortice.Direct2D1;
using YukkuriMovieMaker.Player.Video;

namespace ArcText
{
    internal class ArcTextEffectProcessor : IVideoEffectProcessor
    {
        readonly ArcTextEffect item;
        ID2D1Image? input;
        public ID2D1Image Output => input ?? throw new NullReferenceException(nameof(input) + " is null");

        public ArcTextEffectProcessor(ArcTextEffect item)
        {
            this.item = item;
        }


        public DrawDescription Update(EffectDescription effectDescription)
        {
            var frame = effectDescription.ItemPosition.Frame;
            var length = effectDescription.ItemDuration.Frame;
            var fps = effectDescription.FPS;

            var textIndex = effectDescription.InputIndex;
            var textCount = effectDescription.InputCount;

            var height = item.Height.GetValue(frame, length, fps);
            var angleIntensity = item.Angle.GetValue(frame, length, fps);
            var interval = item.Interval.GetValue(frame, length, fps);
            var centerXPoint = item.CenterXPoint.GetValue(frame, length, fps);

            if (textCount < 2)
                return effectDescription.DrawDescription;

            var t = (double)textIndex / (textCount - 1);
            var x = -Math.Cos(Math.PI * t);
            var y = -height * Math.Sin(Math.PI * (t - centerXPoint / 200d));


            double angle;
            var dis = x - centerXPoint / 100d;
            var ang = angleIntensity * dis;
            if (dis >= 0)
            {
                angle = Math.Atan2(y + height, x + height) * 180 / Math.PI + ang;
            }
            else
            {
                angle = -Math.Atan2(y + height, x + height) * 180 / Math.PI + ang;
            }


            var drawDesc = effectDescription.DrawDescription;
            return drawDesc with
            {
                Draw = new(
                    drawDesc.Draw.X + (float)x + (float)(interval * dis),
                    drawDesc.Draw.Y + (float)y,
                    drawDesc.Draw.Z
                ),
                Rotation = new(
                    drawDesc.Rotation.X,
                    drawDesc.Rotation.Y,
                    drawDesc.Rotation.Z + (float)angle
                )
            };
        }




        public void ClearInput()
        {
            input = null;
        }

        public void SetInput(ID2D1Image? input)
        {
            this.input = input;
        }

        public void Dispose()
        {
        }
    }
}
