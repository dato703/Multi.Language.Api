namespace App.Core
{
    public abstract class ValidatorBase<T> where T : class
    {
        protected T ObjectToValidate { get; }
        public abstract bool IsValid { get; }
        public abstract bool Validate();

        protected ValidatorBase(T obj)
        {
            ObjectToValidate = obj ?? throw new DomainException("მონაცემები ცარიელია", ExceptionLevel.Warning);
        }
    }
}
