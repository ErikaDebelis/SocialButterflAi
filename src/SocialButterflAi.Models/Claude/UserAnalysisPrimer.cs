namespace ButterflAi.Models.Claude
{
    public UserAnalysisPrimer
    {
        /// <summary>
        /// claude requires a conversation to be started with a user message before the assistant. so in order to provide the assistant with the necessary context, we need to provide a user message first.
        /// </summary>
        public Message Message => new Message
        {
            Role = Role.User,
            Content = UserClaudePrimer
        };

        private string const UserClaudePrimer = @"please consider the following scenario:"; // todo: maybe something more meaningful can go here
    }
}