using System;
using UnityEngine;
using UnityEngine.UI;
using XIV.Core.TweenSystem.ImageTweens;
using XIV.Core.TweenSystem.OtherTweens;
using XIV.Core.TweenSystem.RectTransformTweens;
using XIV.Core.TweenSystem.RendererTweens;
using XIV.Core.TweenSystem.TransformTweens;
using XIV.Core.Utils;
using XIV.Core.DataStructures;

namespace XIV.Core.TweenSystem
{
    public sealed partial class XIVTweenBuilder
    {
        public XIVTweenBuilder OnComplete(Action action)
        {
            return AddTween(Get<OnCompleteCallbackTween>().Set(action));
        }

        public XIVTweenBuilder OnCanceled(Action action)
        {
            return AddTween(Get<OnCanceledCallbackTween>().Set(action));
        }

        public XIVTweenBuilder OnComplete(Action<GameObject> action)
        {
            return AddTween(Get<OnCompleteCallbackTween<GameObject>>().Set(action, component.gameObject));
        }

        public XIVTweenBuilder OnCanceled(Action<GameObject> action)
        {
            return AddTween(Get<OnCanceledCallbackTween<GameObject>>().Set(action, component.gameObject));
        }

        public XIVTweenBuilder Wait(float duration)
        {
            return AddTween(Get<WaitTween>().Set(duration));
        }

        // Transform
        public XIVTweenBuilder Scale(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<ScaleTween>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ScaleX(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<ScaleTweenX>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ScaleY(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<ScaleTweenY>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ScaleZ(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<ScaleTweenZ>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder Move(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<MoveTween>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder MoveX(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<MoveTweenX>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder MoveY(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<MoveTweenY>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder MoveZ(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<MoveTweenZ>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        /// <summary>
        /// Moves GameObject in world space but calculates the position on screen space
        /// </summary>
        public XIVTweenBuilder WorldToUIMove(XIVMemory<Vector3> screenSpacePoints, Camera cam, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            var followCurve = Get<FollowCurveScreenSpaceToWorldSpaceTween>();
            followCurve.cam = cam;
            return AddTween(followCurve.Set(component.transform, screenSpacePoints, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder Rotate(Quaternion from, Quaternion to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<RotationTween>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder RotateX(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<RotationTweenX>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder RotateY(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<RotationTweenY>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder RotateZ(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<RotationTweenZ>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder FollowCurve(XIVMemory<Vector3> points, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<FollowCurveTween>().Set(component.transform, points, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder FollowCurve(XIVMemory<Vec3> points, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<FollowCurveVec3Tween>().Set(component.transform, points, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder FollowCurveAndRotation(XIVMemory<Vec3> points, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<FollowCurveAndRotationVec3Tween>().Set(component.transform, points, duration, easingFunc, isPingPong, loopCount));
        }

        // Image
        public XIVTweenBuilder ImageFill(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            if (TryGetComponent<Image>(out var image) == false) ThrowCastError(typeof(Image));
            return AddTween(Get<ImageFillTween>().Set(image, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ImageColor(Color from, Color to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            if (TryGetComponent<Image>(out var image) == false) ThrowCastError(typeof(Image));
            return AddTween(Get<ImageColorTween>().Set(image, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ImageAlpha(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            if (TryGetComponent<Image>(out var image) == false) ThrowCastError(typeof(Image));
            return AddTween(Get<ImageAlphaTween>().Set(image, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        // RectTransform
        public XIVTweenBuilder RectTransformMove(Vector2 from, Vector2 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            if (TryGetComponent<RectTransform>(out var rectTransform) == false) ThrowCastError(typeof(RectTransform));
            return AddTween(Get<RectTransformMoveTween>().Set(rectTransform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ImageColorCurve(XIVMemory<Color> colors, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            if (TryGetComponent<Image>(out var image) == false) ThrowCastError(typeof(Image));
            return AddTween(Get<ImageColorCurveTween>().Set(image, colors, duration, easingFunc, isPingPong, loopCount));
        }

        // Renderer
        public XIVTweenBuilder RendererColor(Color from, Color to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            if (TryGetComponent<Renderer>(out var renderer) == false) ThrowCastError(typeof(Renderer));
            return AddTween(Get<RendererColorTween>().Set(renderer, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder RendererColorCurve(XIVMemory<Color> colors, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            if (TryGetComponent<Renderer>(out var renderer) == false) ThrowCastError(typeof(Renderer));
            return AddTween(Get<RendererColorCurveTween>().Set(renderer, colors, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ScaleBounceOnce()
        {
            var currScale = component.transform.localScale;
            return Scale(currScale, currScale * 0.9f, 0.2f, EasingFunction.SmoothStop3, true);
        }
    }
}