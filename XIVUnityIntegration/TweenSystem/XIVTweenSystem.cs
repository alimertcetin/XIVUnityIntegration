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
            public struct TweenDebugRelationData
            {
                public GameObject go;
                public List<TweenDebugData> tweenDebugDatas;
            }

            [System.Serializable]
            public struct TweenDebugData
            {
                public string name;
                public string time;
            }
            
            public List<TweenDebugRelationData> tweenDebugRelations = new List<TweenDebugRelationData>();
            public List<TweenDebugRelationData> tweenDebugRelationHistory = new List<TweenDebugRelationData>();
#endif
            void Update()
            {
                int tweenTimelineListsCount = XIVTweenSystem.tweenTimelineLists.Count;
                for (int i = tweenTimelineListsCount - 1; i >= 0; i--)
                {
                    List<TweenTimeline> timelines = XIVTweenSystem.tweenTimelineLists[i];
#if UNITY_EDITOR
                    // Get the index of TweenRelationData by looking up the instance id of GameObjects
                    int GetIdx()
                    {
                        var count = tweenDebugRelations.Count;
                        for (var idx = 0; idx < count; idx++)
                        {
                            if (tweenDebugRelations[idx].go.GetInstanceID() == instanceIDs[i]) return idx;
                        }

                        return -1;
                    }

                    var tweenRelationIdx = GetIdx();
                    if (tweenRelationIdx == -1)
                    {
                        // loop through the timelines and add tweens if they are not present in the relation list
                        foreach (var tweenTimeline in timelines)
                        {
                            // Create a relation
                            TweenDebugRelationData tweenDebugRelationData = new TweenDebugRelationData()
                            {
                                go = (GameObject)UnityEditor.EditorUtility.InstanceIDToObject(instanceIDs[i]),
                                tweenDebugDatas = new List<TweenDebugData>(),
                            };
                            // add tweens to relation list which are present in the timeline
                            foreach (ITween t in tweenTimeline.Tweens)
                            {
                                tweenDebugRelationData.tweenDebugDatas.Add(new TweenDebugData()
                                {
                                    name = t.GetType().ToString().Split('.')[^1],
                                    time = System.DateTime.Now.ToString("hh:mm:ss:ffffff tt", System.Globalization.CultureInfo.InvariantCulture),
                                });
                            }
                            tweenDebugRelations.Add(tweenDebugRelationData);
                        }
                    }
#endif
                    TweenTimeline timeline = timelines[0];
                    timeline.Update();
                    if (timeline.IsDone())
                    {
                        // MAYBE : Reverse the Timeline list when the timeline is added to it so that removing timelines from the list would be much more performant
                        timelines.RemoveAt(0);
                        timeline.Clear();
                        XIVPoolSystem.ReleaseItem(timeline);
                    }

                    // There is no timeline left in the list
                    if (timelines.Count == 0)
                    {
#if UNITY_EDITOR
                        int historyListIdx = tweenDebugRelationHistory.FindIndex((p) => p.go == tweenDebugRelations[tweenRelationIdx].go);
                        if (historyListIdx != -1)
                        {
                           tweenDebugRelationHistory[historyListIdx].tweenDebugDatas.AddRange(tweenDebugRelations[tweenRelationIdx].tweenDebugDatas);
                        }
                        else
                        {
                            tweenDebugRelationHistory.Add(tweenDebugRelations[tweenRelationIdx]);
                        }

                        if (tweenDebugRelationHistory.Count > 10) tweenDebugRelationHistory.RemoveAt(10);
                        tweenDebugRelations.RemoveAt(tweenRelationIdx);
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

        static readonly List<List<TweenTimeline>> tweenTimelineLists = new(8);
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

            var timelines = tweenTimelineLists[index];
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
                return tweenTimelineLists[index];
            }

            var timelines = XIVPoolSystem.GetItem<List<TweenTimeline>>();
            Add(instanceID, timelines);
            return timelines;
        }

        static void Add(int instanceID, List<TweenTimeline> timelines)
        {
            instanceIDs.Add(instanceID);
            tweenTimelineLists.Add(timelines);
        }

        static void Remove(int instanceID)
        {
            var index = instanceIDs.IndexOf(instanceID);
            tweenTimelineLists.RemoveAt(index);
            instanceIDs.RemoveAt(index);
        }

        static void Clear()
        {
            instanceIDs.Clear();
            tweenTimelineLists.Clear();
        }
    }
}
