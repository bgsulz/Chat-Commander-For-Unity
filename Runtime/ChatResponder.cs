using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BGSulz.ChatCommander
{
    public abstract class ChatResponder : MonoBehaviour
    {
        private class CommandInvocation
        {
            private readonly float _duration;
            private readonly Coroutine _coroutine;
            private readonly Action _cleanup;
            private readonly ChatResponder _enactor;

            public CommandInvocation(ChatResponder enactor, float duration, Action cleanup)
            {
                _duration = duration;
                _cleanup = cleanup;
                _enactor = enactor;
                _coroutine = _enactor.StartCoroutine(DisableAfterSeconds(this));
            }

            private static IEnumerator DisableAfterSeconds(CommandInvocation invocation)
            {
                yield return new WaitForSeconds(invocation._duration);
                invocation._cleanup.Invoke();
            }

            public void CutOffEarly()
            {
                if (!_enactor.Active) return;
                _enactor.StopCoroutine(_coroutine);
                _cleanup.Invoke();
            }
        }
        
        protected abstract IEnumerable<string> CommandKeywords { get; }

        public IEnumerable<string> CommandKeywordsLowercase => CommandKeywords.Select(x => x.ToLowerInvariant());

        public bool Active { get; private set; }

        protected abstract void OnReceive(CommandMessage msg);
        protected abstract void OnEnd(CommandMessage msg);

        public virtual void OnEnable() => ChatResponderManager.Register(this);
        public virtual void OnDisable() => ChatResponderManager.Unregister(this);

        private CommandInvocation _activeCommand;
        private CommandMessage _activeMsg;

        public void Activate(CommandMessage msg)
        {
            if (Active) 
                _activeCommand.CutOffEarly();
            
            Active = true;
            _activeMsg = msg;
            OnReceive(msg);
        }

        public void Deactivate(CommandMessage msg)
        {
            Active = false;
            OnEnd(msg);
        }

        protected void EndAfter(float seconds)
        {
            if (seconds <= 0)
            {
                Deactivate(_activeMsg);
                Active = false;
                return;
            }
            
            _activeCommand = new CommandInvocation(this, seconds, () =>
            {
                Deactivate(_activeMsg);
                Active = false;
            });
        }
    }
}