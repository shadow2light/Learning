using System;
using System.Collections.Generic;
using Plugins.Tinject.LifeCircle;
using UnityEngine;

namespace Common.LifeCircle
{
    public class Main : MonoBehaviour
    {
        private List<IAwake> _awakes;
        private List<IStart> _starts;
        private List<IUpdate> _updates;
        private List<ILateUpdate> _lateUpdates;
        private List<IOnDestroy> _onDestroys;

        #region Unity Events
        protected virtual void Awake()
        {
            Prepare();

            foreach (var awake in _awakes)
            {
                awake.Awake();
            }
            
        }

        protected virtual void Start()
        {
            foreach (var start in _starts)
            {
                start.Start();
            }
        }

        protected virtual void Update()
        {
            foreach (var update in _updates)
            {
                update.Update();
            }
        }

        protected virtual void LateUpdate()
        {
            foreach (var lateUpdate in _lateUpdates)
            {
                lateUpdate.LateUpdate();
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var onDestroy in _onDestroys)
            {
                onDestroy.OnDestroy();
            }
        }

        #endregion

        private void Prepare()
        {
            _awakes = new List<IAwake>();
            _starts = new List<IStart>();
            _updates = new List<IUpdate>();
            _lateUpdates = new List<ILateUpdate>();
            _onDestroys = new List<IOnDestroy>();
        }

        public void AddAwake(IAwake awake)
        {
            _awakes.Add(awake);
            }

        public void AddStart(IStart start)
        {
            _starts.Add(start);
        }

        public void AddUpdate(IUpdate update)
        {
            _updates.Add(update);
        }

        public void AddLateUpdate(ILateUpdate lateUpdate)
        {
            _lateUpdates.Add(lateUpdate);
        }

        public void AddOnDestroy(IOnDestroy onDestroy)
        {
            _onDestroys.Add(onDestroy);
        }
    }
}