namespace Logic.Common
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            var item = obj as T;

            if (item == null || GetType() != obj.GetType())
                return false;

            return EqualsCore(item);
        }

        protected abstract bool EqualsCore(T obj);

        public static bool operator ==(ValueObject<T> item1, ValueObject<T> item2)
        {
            if (item1 is null && item2 is null)
                return true;

            if (item1 is null || item2 is null)
                return false;

            return item1.Equals(item2);
        }

        public static bool operator !=(ValueObject<T> item1, ValueObject<T> item2)
        {
            return !(item1 == item2);
        }
    }
}
