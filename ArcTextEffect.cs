using System.ComponentModel.DataAnnotations;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Plugin.Effects;

namespace ArcText
{
    [VideoEffect("アーチ配置", ["配置"], ["arch","a-tihaiti","ArTex"], isAviUtlSupported:false)]
    internal class ArcTextEffect: VideoEffectBase
    {
        public override string Label => "アーチ配置";

        [Display(GroupName = "配置", Name = "高さ", Description = "高さ")]
        [AnimationSlider("F1", "px", -100, 100)]
        public Animation Height { get; } = new Animation(-100, -99999, 99999);

        [Display(GroupName = "配置", Name = "中心位置", Description = "X座標の中心位置")]
        [AnimationSlider("F1", "px", -100, 100)]
        public Animation CenterXPoint { get; } = new Animation(0, -99999, 99999);

        [Display(GroupName = "配置", Name = "角度", Description = "角度")]
        [AnimationSlider("F1", "°", -90, 90)]
        public Animation Angle { get; } = new Animation(0, -3600, 3600);

        [Display(GroupName = "配置", Name = "間隔", Description = "間隔")]
        [AnimationSlider("F1", "px", -100, 100)]
        public Animation Interval { get; } = new Animation(0, -99999, 99999);


        public override IEnumerable<string> CreateExoVideoFilters(int keyFrameIndex, ExoOutputDescription exoOutputDescription)
        {
            return[];
        }

        public override IVideoEffectProcessor CreateVideoEffect(IGraphicsDevicesAndContext devices)
        {
            return new ArcTextEffectProcessor(devices, this);
        }

        protected override IEnumerable<IAnimatable> GetAnimatables() => [Height, Interval, Angle, CenterXPoint];
    }
}
