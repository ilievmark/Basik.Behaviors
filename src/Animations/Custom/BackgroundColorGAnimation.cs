using Xamarin.Forms;

namespace Basil.Behaviors.Animations.Custom
{
    public class BackgroundColorGAnimation : CustomAnimationBase
    {
        protected override void Tick(VisualElement visualElement, double startValue, double currentValue, double endValue)
        {
            visualElement.BackgroundColor = new Color(
                visualElement.BackgroundColor.R,
                currentValue,
                visualElement.BackgroundColor.B);
        }
    }
}