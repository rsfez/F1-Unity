namespace Models.Builders
{
    public interface IBuildableFromCsv<out T>
    {
        public T Build(params string[] args);
    }
}