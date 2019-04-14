using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;

namespace RESTAPIDemo
{
    // Source: https://dev.to/cwetanow/wiring-up-ninject-with-aspnet-core-20-3hp
    public sealed class DelegatingControllerActivator : IControllerActivator
    {
        private readonly Func<ControllerContext, object> controllerCreator;
        private readonly Action<ControllerContext, object> controllerReleaser;

        public DelegatingControllerActivator(
            Func<ControllerContext, object> controllerCreator,
            Action<ControllerContext, object> controllerReleaser = null)
        {
            this.controllerCreator = controllerCreator ?? throw new ArgumentNullException(nameof(controllerCreator));
            this.controllerReleaser = controllerReleaser ?? ((_, __) => { });
        }

        public object Create(ControllerContext context) => controllerCreator(context);

        public void Release(ControllerContext context, object controller)
            => controllerReleaser(context, controller);
    }

    // Source: https://dev.to/cwetanow/wiring-up-ninject-with-aspnet-core-20-3hp
    public sealed class DelegatingViewComponentActivator : IViewComponentActivator
    {
        private readonly Func<Type, object> viewComponentCreator;
        private readonly Action<object> viewComponentReleaser;

        public DelegatingViewComponentActivator(Func<Type, object> viewComponentCreator,
            Action<object> viewComponentReleaser = null)
        {
            this.viewComponentCreator = viewComponentCreator ?? throw new ArgumentNullException(nameof(viewComponentCreator));
            this.viewComponentReleaser = viewComponentReleaser ?? (_ => { });
        }

        public object Create(ViewComponentContext context)
            => viewComponentCreator(context.ViewComponentDescriptor.TypeInfo.AsType());

        public void Release(ViewComponentContext context, object viewComponent)
            => viewComponentReleaser(viewComponent);
    }
}
