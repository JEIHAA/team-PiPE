using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorkPlus {

    public class Rewinder : MonoBehaviour {

        [SerializeField] private new Transform transform;
        [SerializeField] private new Rigidbody rigidbody;

        /* The elements of record (Add what you want to rewind) */
        public struct Recorder {
            public Vector3 position;
            public Quaternion rotation;

            public Recorder (Vector3 position, Quaternion rotation) {
                this.position = position;
                this.rotation = rotation;
            }
        } //End of define Recorder struct

        private new bool enabled = true;//Record and Rewind enabled
        public bool Enabled {
            get { return enabled; }
            set {
                if (value) OnEnable ();
                else OnDisable ();
                enabled = value;
            }
        }

        private List<Recorder> recordList = new List<Recorder> (); //Transform record list

        public int recordLimit = 50; //Limitation of record list count

        public bool IsRewinding { get; internal set; } = false; //Is Rewinding now

        private bool isKinematic = false; //Rigidbody's isKinematic

        //Get max rewinding time
        public float RewindTime {
            get {
                int period = RewindManager.GetRecordPeriod ();
                return recordLimit * (Time.fixedDeltaTime * period);
            }
        }

        //Initialize
        private void Awake () {
            if (transform == null) transform = GetComponent<Transform> ();
            if (rigidbody == null) rigidbody = GetComponent<Rigidbody> ();
        }

        //Add this to list and enable recording when it's enabled
        private void OnEnable () {
            if (Enabled) {
                RewindManager.RewinderList.Add (this);
                RewindManager.RewinderDctn.Add (gameObject, this);
                RewindManager.Recorder += Record;
            }
        }

        //Remove this form list and disable recording when it's disabled
        private void OnDisable () {
            RewindManager.RewinderList.Remove (this);
            RewindManager.RewinderDctn.Remove (gameObject);
            RewindManager.Recorder -= Record;
        }

        internal void Rewind (float lerpTime, bool endOfFrame) {
            if (enabled && recordList.Count > 0) {

                /* Apply last elements of recorder (Using lerp with lerpTime) */
                transform.position = Vector3.Lerp (transform.position, recordList[0].position, lerpTime);           
                transform.rotation = Quaternion.Slerp (transform.rotation, recordList[0].rotation, lerpTime);   
                //End of apply elements

                if (endOfFrame) recordList.RemoveAt (0); //Remove first index

            } else {
                //End of rewinding
                RewindManager.Rewinder -= Rewind;
                RewindManager.Recorder += Record;
                if (rigidbody != null && !isKinematic) rigidbody.isKinematic = false; //Don't disabled if origin's false
                IsRewinding = false;
            }
        }

        //Enable rewind
        internal void RewindEnable () {
            if (enabled) {
                IsRewinding = true;
                if (rigidbody != null) {
                    isKinematic = rigidbody.isKinematic; //Get kinematic enabled
                    if (!isKinematic) rigidbody.isKinematic = true;
                }

                RewindManager.Recorder -= Record;
                RewindManager.Rewinder += Rewind;
            }
        }

        //record or clear the list
        internal void Record () {

            //Record to list if it's enabled
            if (enabled) {

                //Rid of last indext if count exceed the limitation
                if (recordList.Count >= recordLimit) {
                    recordList.RemoveAt (recordList.Count - 1);
                }

                /* Record new recorder at first index (Add elements of current state) */
                recordList.Insert (0, new Recorder (transform.position, transform.rotation));
                //End of insert recorder

            //Clear the list if it's not enabled
            } else if (!enabled) {
                if (recordList.Count > 0) 
                    recordList.Clear ();
            }
        }
    }
}
