namespace BGSulz.ChatCommander
{
    public class CommandMessage
    {
        public string Word { get; }
        public string[] Parameters { get; }
        public string User { get; }

        public CommandMessage(string word, string[] args, string user = default)
            => (Word, Parameters, User) = (word, args, user);
    }
}