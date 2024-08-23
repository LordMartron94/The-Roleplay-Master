namespace MD.Common.EconomySystem
{
    public interface IItem
    {
        public string Name { get; }
        public string UuidV4 { get; }

        void ChangeName(string newName);

        void SubscribeChangeName(Action<string, string, string?> methodToSubscribe);

        void UnsubscribeChangeName(Action<string, string, string?> methodToUnsubscribe);
    }
}