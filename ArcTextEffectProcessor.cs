using Vortice.Direct2D1;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;

namespace ArcText
{
    internal class ArcTextEffectProcessor : IVideoEffectProcessor
    {
        readonly ArcTextEffect item;
        ID2D1Image? input;
        public ID2D1Image Output => input ?? throw new NullReferenceException(nameof(input) + " is null");

        public ArcTextEffectProcessor(IGraphicsDevicesAndContext devices, ArcTextEffect item)
        {
            this.item = item;
        }


        public DrawDescription Update(EffectDescription effectDescription)
        {
            var frame = effectDescription.ItemPosition.Frame;
            var length = effectDescription.ItemDuration.Frame;
            var fps = effectDescription.FPS;

            var X = effectDescription.DrawDescription.Draw.X;
            var Y = effectDescription.DrawDescription.CenterPoint.Y;
            var textIndex = effectDescription.InputIndex;
            var totalTextCount = effectDescription.InputCount;

            var height = item.Height.GetValue(frame, length, fps);
            var angleIntensity = item.Angle.GetValue(frame, length, fps);
            var interval = item.Interval.GetValue(frame, length, fps);
            var centerXPoint = item.CenterXPoint.GetValue(frame, length, fps);


            float t = 0.0f;
            if (totalTextCount > 2)
            {
                t = (textIndex - (totalTextCount - 1) / 2.0f) / ((totalTextCount - 1) / 2.0f);
            }
            

            float distanceFactor = MathF.Cos(MathF.PI * (t - (float)centerXPoint / 400.0f) / 2.0f );
            float y = (float)height * distanceFactor;


            float dx = MathF.Abs(X) + MathF.Abs((float)centerXPoint);
            float dy = Y + y  + (float)height;
            
            float angle = (MathF.Atan2(dy, dx) * 180.0f / MathF.PI + (float)angleIntensity * MathF.Abs(t)) * -(t - (float)centerXPoint / 400.0f);


            var drawDesc = effectDescription.DrawDescription;
            return drawDesc with
            {
                Draw = new(
                    drawDesc.Draw.X + (float)interval * t,
                    drawDesc.Draw.Y + y,
                    drawDesc.Draw.Z
                ),
                Rotation = new(
                    drawDesc.Rotation.X,
                    drawDesc.Rotation.Y,
                    drawDesc.Rotation.Z + angle
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
