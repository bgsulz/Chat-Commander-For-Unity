namespace BGSulz.ChatCommander
{
    public class CommandMessage
    {
        public string Word { get; }
        private string[] Parameters { get; }
        public string User { get; }

        public CommandMessage(string word, string[] args, string user = default)
            => (Word, Parameters, User) = (word, args, user);

        public string GetParam(int index) => index >= 0 && index < Parameters.Length ? Parameters[index] : null;
        public string this[int index] => GetParam(index);
    }
}