namespace Controllers.Builders
{
    public interface IBuildableController
    {
        public void Setup(params string[] args);
    }
}