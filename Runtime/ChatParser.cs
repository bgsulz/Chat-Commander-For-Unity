using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BGSulz.ChatCommander
{
    public static class ChatParser
    {
        private static readonly string[] DefaultPrefixes = { "!" };

        public static void Submit(in string input, in string user = default)
        {
            var usePrefixes = DefaultPrefixes;
            
            var noDoubleSpaces = input.ReplaceAll("  ", " ");
            var split = noDoubleSpaces.Split(' ');

            var indexedCommands = split
                .Select(x => x.RemovePrefixAny(usePrefixes))
                .Where(x => x != null)
                .Select((s, i) => (word: s, index: i))
                .ToArray();

            for (var (i, len) = (0, indexedCommands.Length); i < len; i++)
            {
                var (word, index) = indexedCommands[i];

                var startIndex = index + 1;
                var endIndex = i == len - 1 ? split.Length : indexedCommands[i + 1].index;

                var args = split[startIndex..endIndex];

                Debug.Log($"Sending {word}");
                ChatResponderManager.Process(new CommandMessage(word, args, user));
            }
        }

        private static string ReplaceAll(this string input, in string toReplace, in string toReplaceWith)
        {
            if (toReplaceWith.Contains(toReplace))
                throw new Exception($"Trying to {nameof(ReplaceAll)} {toReplace} with {toReplaceWith}, but {toReplaceWith} contains {toReplace}!");

            while (input.Contains(toReplace))
                input = input.Replace(toReplace, toReplaceWith);
            return input;
        }

        private static string RemovePrefixAny(this string input, IEnumerable<string> prefixes) =>
            prefixes
                .Where(prefix => input.StartsWith(prefix) && input.Length > prefix.Length)
                .Select(prefix => input[prefix.Length..])
                .FirstOrDefault();
    }
}