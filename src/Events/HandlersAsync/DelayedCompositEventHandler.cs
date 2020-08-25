using System.Collections.Generic;
using System.Threading.Tasks;
using Basil.Behaviors.Events.HandlerAbstract;
using Basil.Behaviors.Events.HandlerBase;
using Xamarin.Forms;

namespace Basil.Behaviors.Events.HandlersAsync
{
    [ContentProperty(nameof(Handler))]
    public class DelayedCompositEventHandler : BaseAsyncHandler, ICompositeHandler
    {
        #region Properties
        
        #region Handler property
        
        public static readonly BindableProperty HandlerProperty =
            BindableProperty.Create(
                propertyName: nameof(Handler),
                returnType: typeof(BaseHandler),
                declaringType: typeof(DelayedCompositEventHandler));

        public BaseHandler Handler
        {
            get => (BaseHandler)GetValue(HandlerProperty);
            set => SetValue(HandlerProperty, value);
        }
        
        #endregion
        
        #region DelayMilliseconds property
        
        public static readonly BindableProperty DelayMillisecondsProperty =
            BindableProperty.Create(
                propertyName: nameof(DelayMilliseconds),
                returnType: typeof(int),
                declaringType: typeof(DelayedCompositEventHandler),
                defaultValue: default(int));

        public int DelayMilliseconds
        {
            get => (int)GetValue(DelayMillisecondsProperty);
            set => SetValue(DelayMillisecondsProperty, value);
        }
        
        #endregion

        #endregion
        
        #region Overrides

        public override async void Rise(object sender, object eventArgs)
        {
            await Task.Delay(DelayMilliseconds);
            
            Handler.Rise(sender, eventArgs);
        }

        public override async Task RiseAsync(object sender, object eventArgs)
        {
            await Task.Delay(DelayMilliseconds);
           
            if (Handler is IAsyncRisible castedHandler)
            {
                if (castedHandler.WaitResult)
                    await castedHandler.RiseAsync(sender, eventArgs);
                else
                    castedHandler.RiseAsync(sender, eventArgs);
            }
            else
                Handler.Rise(sender, eventArgs);
        }

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            
            Handler?.AttachToBindableObject(bindable);
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
            
            Handler?.DetachFromBindableObject(bindable);
        }

        #endregion

        public IList<BaseHandler> GetInnerHandlers()
            => new List<BaseHandler> { Handler };
    }
}