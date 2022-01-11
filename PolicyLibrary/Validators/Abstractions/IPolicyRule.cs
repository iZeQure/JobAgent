namespace PolicyLibrary.Validators.Abstractions
{
    public interface IPolicyRule
    {
        void ValidateRule(object value, ref Validator validator);
    }
}
