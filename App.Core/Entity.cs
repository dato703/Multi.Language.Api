namespace App.Core
{
    public class Entity<T> where T : struct
    {
        public T Id { get; set; }

        public Entity()
        {

        }
    }
}
