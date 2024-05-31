using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorkPlus {

    public class RewindManager : MonoBehaviour {

        private static RewindManager instance;

        internal static Action Recorder;
        internal static Action<float, bool> Rewinder;

        internal static List<Rewinder> RewinderList { get; set; } = new List<Rewinder> ();
        internal static Dictionary<GameObject, Rewinder> RewinderDctn { get; set; } = new Dictionary<GameObject, Rewinder> ();

        [Tooltip ("period must be greater than 1")]
        public int recordPeriod = 1; //Record period based on frame number

        private int period = 0; //Temp variable for calculate current frame

        private void Awake () {
            instance = this;
            if (recordPeriod <= 0) recordPeriod = 1; //Set period to 1 if it's 0 or negative
        }

        //Execute record and rewind
        private void FixedUpdate () {
            if (!(recordPeriod <= 0) && period <= 0) {
                Recorder?.Invoke ();
                period = recordPeriod;
            }

            float lerpTime = (float)(recordPeriod - (period - 1)) / recordPeriod;
            Rewinder?.Invoke (lerpTime, period == 1);
            period -= 1;
        }

        //Rewind all of rewinder
        public static void RewindAll () {
            foreach (var rewinder in RewinderList) {
                rewinder.RewindEnable ();
            }
        }

        //Rewind the rewinder by GameObject
        public static void Rewind (GameObject gameObject) {
            RewinderDctn[gameObject].RewindEnable ();
        }

        //Rewind the rewinder
        public static void Rewind (Rewinder rewinder) {
            rewinder.RewindEnable ();
        }

        //Set All of rewinder's record limits
        public static void SetRecordLimitAll (int limit) {
            foreach (var rewinder in RewinderList) {
                rewinder.recordLimit = limit;
            }
        }

        //Get record period
        public static int GetRecordPeriod () {
            return instance.recordPeriod;
        }

        //Set record period
        public static void SetRecordPeriod (int period) {
            instance.recordPeriod = period;
        }

        //Get rewinder by GameObject
        public static Rewinder GetRewinder (GameObject gameObject) {
            return RewinderDctn[gameObject];
        }

        //Get all of rewinder
        public static Rewinder[] GetRewinderAll () {
            return RewinderList.ToArray ();
        }
    }
}