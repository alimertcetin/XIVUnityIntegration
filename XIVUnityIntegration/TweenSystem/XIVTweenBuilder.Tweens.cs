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
using XIV.Core.Extensions;
using XIV.Core.XIVMath;
using XIV.UnityIntegration.Extensions;

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

        /// <summary>
        /// Provided action will be invoked whenever the tween completed/canceled
        /// </summary>
        public XIVTweenBuilder Finally(Action action)
        {
            return AddTween(Get<OnFinallyCallbackTween>().Set(action));
        }

        public XIVTweenBuilder OnComplete(Action<GameObject> action)
        {
            return AddTween(Get<OnCompleteCallbackTween<GameObject>>().Set(action, component.gameObject));
        }

        public XIVTweenBuilder OnCanceled(Action<GameObject> action)
        {
            return AddTween(Get<OnCanceledCallbackTween<GameObject>>().Set(action, component.gameObject));
        }

        /// <summary>
        /// <inheritdoc cref="Finally(Action)"/>
        /// </summary>
        public XIVTweenBuilder Finally(Action<GameObject> action)
        {
            return AddTween(Get<OnFinallyCallbackTween<GameObject>>().Set(action, component.gameObject));
        }

        public XIVTweenBuilder Wait(float duration)
        {
            return AddTween(Get<WaitTween>().Set(duration));
        }

        // Transform
        // Scale
        public XIVTweenBuilder Scale(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<ScaleTween>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ScaleCurve(XIVMemory<Vector3> scales, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<ScaleCurveTween>().Set(component.transform, scales, duration, easingFunc, isPingPong, loopCount));
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

        // Position
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

        public XIVTweenBuilder MoveFollowCurve(XIVMemory<Vector3> points, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<MoveFollowCurveTween>().Set(component.transform, points, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder MoveFollowCurve(XIVMemory<Vec3> points, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<MoveFollowCurveVec3Tween>().Set(component.transform, points, duration, easingFunc, isPingPong, loopCount));
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

        // Rotation
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
        
        public XIVTweenBuilder RotateFollowCurve(XIVMemory<Vec3> eulerRotations, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<RotationFollowCurveTween>().Set(component.transform, eulerRotations, eulerRotations.reversed, duration, easingFunc, isPingPong, loopCount));
        }

        // Image
        public XIVTweenBuilder ImageFill(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<Image, ImageFillTween>(out var image).Set(image, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ImageColor(Color from, Color to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<Image, ImageColorTween>(out var image).Set(image, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ImageAlpha(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<Image, ImageAlphaTween>(out var image).Set(image, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder ImageColorCurve(XIVMemory<Color> colors, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<Image, ImageColorCurveTween>(out var image).Set(image, colors, duration, easingFunc, isPingPong, loopCount));
        }

        // CanvasGroup
        public XIVTweenBuilder CanvasGroupAlpha(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<CanvasGroup, CanvasGroupAlphaTween>(out var canvasGroup).Set(canvasGroup, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        // RectTransform
        public XIVTweenBuilder RectTransformMove(Vector2 from, Vector2 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<RectTransform, RectTransformMoveTween>(out var rectTransform).Set(rectTransform, from, to, duration, easingFunc, isPingPong, loopCount));
        }
        
        public XIVTweenBuilder RectTransformLocalMove(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<RectTransform, RectTransformLocalMoveTween>(out var rectTransform).Set(rectTransform, from, to, duration, easingFunc, isPingPong, loopCount));
        }
        
        public XIVTweenBuilder RectTransformSizeDelta(Vector2 from, Vector2 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<RectTransform, RectTransformSizeDelta>(out var rectTransform).Set(rectTransform, from, to, duration, easingFunc, isPingPong, loopCount));
        }
        
        public XIVTweenBuilder RectTransformAnchoredPosition3D(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<RectTransform, RectTransformAnchoredPosition3D>(out var rectTransform).Set(rectTransform, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        // Renderer
        public XIVTweenBuilder RendererColor(Color from, Color to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<Renderer, RendererColorTween>(out var renderer).Set(renderer, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder RendererColorCurve(XIVMemory<Color> colors, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<Renderer, RendererColorCurveTween>(out var renderer).Set(renderer, colors, duration, easingFunc, isPingPong, loopCount));
        }

        // Sprite Renderer
        public XIVTweenBuilder SpriteRendererColor(Color from, Color to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<SpriteRenderer, SpriteRendererColorTween>(out var renderer).Set(renderer, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder SpriteRendererColorCurve(XIVMemory<Color> colors, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<SpriteRenderer, SpriteRendererColorCurveTween>(out var renderer).Set(renderer, colors, duration, easingFunc, isPingPong, loopCount));
        }

        // Material Property Block
        public XIVTweenBuilder Mpb(MaterialPropertyBlock mpb, int propId, Color from, Color to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<Renderer, MaterialPropertyBlockColorTween>(out var renderer).Set(mpb, propId, renderer, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        public XIVTweenBuilder Mpb(MaterialPropertyBlock mpb, int propId, float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(GetTween<Renderer, MaterialPropertyBlockFloatTween>(out var renderer).Set(mpb, propId, renderer, from, to, duration, easingFunc, isPingPong, loopCount));
        }

        // Combined
        public XIVTweenBuilder FollowCurveAndRotation(XIVMemory<Vec3> points, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween(Get<FollowCurveAndRotationVec3Tween>().Set(component.transform, points, duration, easingFunc, isPingPong, loopCount));
        }

        // Presets
        public XIVTweenBuilder ScaleBounceOnce(float intensity = 0.1f)
        {
            intensity = 1f - Math.Clamp(intensity, 0f, 1f);
            var currScale = component.transform.localScale;
            return Scale(currScale, currScale * intensity, 0.2f, EasingFunction.SmoothStop3, true);
        }
        
        /// <summary>
        /// Pre-defined values will give you a light shake
        /// </summary>
        public XIVTweenBuilder Shake(float rotationIntensity = 0.2f, float positionIntensity = 0.25f, float duration = 0.25f, int loopCount = 0)
        {
            var componentTransform = component.transform;
            var pos = componentTransform.position.ToVec3();
            var rot = componentTransform.rotation.eulerAngles.ToVec3();
            
            var arc = ArrayUtils.GetBuffer<Vec3>(4);
            var rotation =  ArrayUtils.GetBuffer<Vec3>(4);
            arc[0] = pos;
            arc[1] = pos + (Vec3.left + XIVRandom.insideUnitSphere) * positionIntensity;
            arc[2] = pos + (Vec3.right + XIVRandom.insideUnitSphere) * positionIntensity;
            arc[3] = pos;

            rotation[0] = rot;
            rotation[1] = rot + XIVRandom.insideUnitSphere * rotationIntensity;
            rotation[2] = rot + XIVRandom.insideUnitSphere * rotationIntensity;
            rotation[3] = rot;

            return MoveFollowCurve(arc.AsXIVMemory(4), duration, EasingFunction.SmoothStart3, true, loopCount)
                .And()
                .RotateFollowCurve(rotation.AsXIVMemory(4), duration, EasingFunction.SmoothStart3, loopCount: loopCount)
                .Finally((_) =>
                {
                    ((IDisposable)arc).Dispose();
                    ((IDisposable)rotation).Dispose();
                });
        }
    }
}