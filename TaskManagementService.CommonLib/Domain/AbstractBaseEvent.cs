namespace TaskManagementService.CommonLib.Domain
{
    public class AbstractBaseEvent<T> : EventBase where T : class
    {
        public AbstractBaseEvent(T item)
        {
            Item = item;
        }
        public T Item { get; }
    }
}
