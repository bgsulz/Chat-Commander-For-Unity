using System;
using System.Linq;

namespace BGSulz.ChatCommander
{
    public static class ChatParser
    {
        public static string Prefix { get; set; } = "!";

        public static void Submit(in string input, in string user = default, in string prefix = null)
        {
            var usePrefix = prefix ?? Prefix;
            
            var noDoubleSpaces = input.ReplaceAll("  ", " ");
            var split = noDoubleSpaces.Split(' ');

            var indexedCommands = split
                .Select((s, i) => (word: s, index: i))
                .Where(x => x.word.StartsWith(usePrefix) && x.word.Length > usePrefix.Length)
                .ToArray();

            for (var (i, len) = (0, indexedCommands.Length); i < len; i++)
            {
                var (rawWord, index) = indexedCommands[i];

                var startIndex = index + 1;
                var endIndex = i == len - 1 ? split.Length : indexedCommands[i + 1].index;

                var word = rawWord[usePrefix.Length..];
                var args = split[startIndex..endIndex];

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
    }
}