using eShop.ClientApp.Animations.Base;

namespace eShop.ClientApp.Helpers;

public static class EasingHelper
{
    /// <summary>
    /// Gets the easing function based on the specified easing type.
    /// </summary>
    /// <param name="type">The type of easing function.</param>
    /// <returns>The corresponding easing function.</returns>
    public static Easing GetEasing(EasingType type)
    {
        return type switch
        {
            EasingType.BounceIn => Easing.BounceIn,
            EasingType.BounceOut => Easing.BounceOut,
            EasingType.CubicIn => Easing.CubicIn,
            EasingType.CubicInOut => Easing.CubicInOut,
            EasingType.CubicOut => Easing.CubicOut,
            EasingType.Linear => Easing.Linear,
            EasingType.SinIn => Easing.SinIn,
            EasingType.SinInOut => Easing.SinInOut,
            EasingType.SinOut => Easing.SinOut,
            EasingType.SpringIn => Easing.SpringIn,
            EasingType.SpringOut => Easing.SpringOut,
            _ => null
        };
    }
}
