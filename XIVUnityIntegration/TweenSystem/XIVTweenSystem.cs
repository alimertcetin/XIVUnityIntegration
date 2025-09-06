using System.Collections.Generic;
using UnityEngine;
using XIV.PoolSystem;

namespace XIV.Core.TweenSystem
{
    internal static class XIVTweenSystem
    {
        class TweenHelperMono : MonoBehaviour
        {
            
#if UNITY_EDITOR
            [System.Serializable]
            public struct TweenD
            {
                public GameObject go;
                public List<TweenData> tweenDatas;
            }

            [System.Serializable]
            public struct TweenData
            {
                public string name;
                public string time;
            }
            
            public List<TweenD> XIVTweenGameObjects = new List<TweenD>();
            public List<TweenD> history = new List<TweenD>();
#endif
            void Update()
            {
                int count = XIVTweenSystem.tweenTimelines.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    List<TweenTimeline> timelines = XIVTweenSystem.tweenTimelines[i];
#if UNITY_EDITOR
                    int GetIdx()
                    {
                        for (var idx = 0; idx < XIVTweenGameObjects.Count; idx++)
                        {
                            var tweenD = XIVTweenGameObjects[idx];
                            if (tweenD.go.GetInstanceID() == instanceIDs[i]) return idx;
                        }

                        return -1;
                    }

                    var xivTweenDebugIdx = GetIdx();
                    foreach (var tweenTimeline in timelines)
                    {
                        if (xivTweenDebugIdx != -1) continue;
                        TweenD tweenD = new TweenD()
                        {
                            go = (GameObject)UnityEditor.EditorUtility.InstanceIDToObject(instanceIDs[i]),
                            tweenDatas = new List<TweenData>(),
                        };
                        foreach (ITween t in tweenTimeline.Tweens)
                        {
                            tweenD.tweenDatas.Add(new TweenData()
                            {
                                name = t.GetType().ToString().Split('.')[^1],
                                time = System.DateTime.Now.ToString("hh:mm:ss:ffffff tt", System.Globalization.CultureInfo.InvariantCulture),
                            });
                        }
                        XIVTweenGameObjects.Add(tweenD);
                    }
#endif
                    TweenTimeline timeline = timelines[0];
                    timeline.Update();
                    if (timeline.IsDone())
                    {
                        timelines.RemoveAt(0);
                        timeline.Clear();
                        XIVPoolSystem.ReleaseItem(timeline);
                    }

                    // TODO : This doesn't look right
                    if (timelines.Count == 0)
                    {
#if UNITY_EDITOR
                        int idx = history.FindIndex((p) => p.go == XIVTweenGameObjects[xivTweenDebugIdx].go);
                        if (idx != -1)
                        {
                           history[idx].tweenDatas.AddRange(XIVTweenGameObjects[xivTweenDebugIdx].tweenDatas);
                        }
                        else
                        {
                            history.Add(XIVTweenGameObjects[xivTweenDebugIdx]);
                        }

                        if (history.Count > 10) history.RemoveAt(10);
                        XIVTweenGameObjects.RemoveAt(xivTweenDebugIdx);
#endif
                        XIVTweenSystem.Remove(instanceIDs[i]);
                        timelines.Clear();
                        XIVPoolSystem.ReleaseItem(timelines);
                    }
                }
            }

            void OnDestroy()
            {
                XIVTweenSystem.tweenHelperMono = null;
                XIVTweenSystem.Clear();
            }
        }

        static readonly List<List<TweenTimeline>> tweenTimelines = new(8);
        static readonly List<int> instanceIDs = new(8);
        static TweenHelperMono tweenHelperMono;

        internal static TweenTimeline GetTimeline()
        {
            var timeLine = XIVPoolSystem.GetItem<TweenTimeline>();
            timeLine.SetDeltaTimeFunc(TweenTimeline.defaulDeltaTimeFunc);
            return timeLine;
        }

        internal static TweenTimeline GetTimeline(ITween tween)
        {
            var timeline = GetTimeline();
            timeline.AddTween(tween);
            return timeline;
        }

        internal static TweenTimeline GetTimeline(ITween[] tweens)
        {
            var timeline = GetTimeline();
            int length = tweens.Length;
            for (int i = 0; i < length; i++)
            {
                timeline.AddTween(tweens[i]);
            }

            return timeline;
        }

        internal static void ReleaseTween(ITween tween)
        {
            XIVPoolSystem.ReleaseItem(tween);
        }

        internal static void AddTween(int instanceID, TweenTimeline timeline)
        {
            if (tweenHelperMono == false)
            {
                tweenHelperMono = new GameObject("XIV-TweenSystem-Helper").AddComponent<TweenHelperMono>();
                Object.DontDestroyOnLoad(tweenHelperMono);
            }

            GetTweenTimelines(instanceID).Add(timeline);
        }

        internal static void CancelTween(int instanceID, bool forceComplete = true)
        {
            var index = instanceIDs.IndexOf(instanceID);
            if (index == -1) return;

            var timelines = tweenTimelines[index];
            int timelineCount = timelines.Count;
            if (forceComplete)
            {
                for (int i = 0; i < timelineCount; i++)
                {
                    TweenTimeline timeline = timelines[i];
                    timeline.ForceComplete();
                    timeline.Clear();
                }
            }
            else
            {
                for (int i = 0; i < timelineCount; i++)
                {
                    TweenTimeline timeline = timelines[i];
                    timeline.Cancel();
                    timeline.Clear();
                }
            }
            
            timelines.Clear();
            XIVPoolSystem.ReleaseItem(timelines);
            Remove(instanceID);
        }

        internal static bool HasTween(int instanceID)
        {
            return instanceIDs.IndexOf(instanceID) != -1;
        }

        static List<TweenTimeline> GetTweenTimelines(int instanceID)
        {
            var index = instanceIDs.IndexOf(instanceID);
            if (index != -1)
            {
                return tweenTimelines[index];
            }

            var timelines = XIVPoolSystem.GetItem<List<TweenTimeline>>();
            Add(instanceID, timelines);
            return timelines;
        }

        static void Add(int instanceID, List<TweenTimeline> timelines)
        {
            instanceIDs.Add(instanceID);
            tweenTimelines.Add(timelines);
        }

        static void Remove(int instanceID)
        {
            var index = instanceIDs.IndexOf(instanceID);
            tweenTimelines.RemoveAt(index);
            instanceIDs.RemoveAt(index);
        }

        static void Clear()
        {
            instanceIDs.Clear();
            tweenTimelines.Clear();
        }
    }
}
