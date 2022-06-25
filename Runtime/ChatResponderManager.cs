using System;
using System.Collections.Generic;
using UnityEngine;

namespace BGSulz.ChatCommander
{
    public static class ChatResponderManager
    {
        public static bool ThrowError { get; set; } = false;

        private static readonly Dictionary<string, List<ChatResponder>> CommandToResponderList = new();
        private static Action<string> Debugger => !ThrowError ? Debug.LogWarning : msg => throw new Exception(msg);

        public static void Register(ChatResponder responder)
        {
            foreach (var command in responder.CommandKeywordsLowercase)
            {
                if (!CommandToResponderList.ContainsKey(command))
                    CommandToResponderList[command] = new List<ChatResponder>();

                CommandToResponderList[command].Add(responder);
            }
        }

        public static void Unregister(ChatResponder responder)
        {
            foreach (var command in responder.CommandKeywordsLowercase)
            {
                if (!CommandToResponderList.TryGetValue(command, out var responders))
                {
                    Debugger.Invoke($"Trying to remove from list for command {command}, but responder list not found.");
                    return;
                }

                var index = responders.IndexOf(responder);

                if (index >= 0)
                    responders.Remove(responder);
                else
                    Debugger.Invoke($"Trying to remove from list for command {command}, but item not found in responder list.");
            }
        }

        public static void Process(CommandMessage msg)
        {
            if (!CommandToResponderList.TryGetValue(msg.Word, out var responders))
            {
                Debugger.Invoke($"Trying to process command {msg.Word}, but no responder list found.");
                return;
            }

            foreach (var responder in responders)
                responder.Activate(msg);
        }

        public static void ManualActivate(string word, params string[] args)
        {
            if (!CommandToResponderList.TryGetValue(word, out var responders))
            {
                Debugger.Invoke($"Trying to manually activate command {word}, but no responder list found.");
                return;
            }

            foreach (var responder in responders)
                responder.Activate(new CommandMessage(word, args));
        }
    }
}