namespace Repository.Interface
{
    public interface ICommonRepository
    {
        string GetConfigValue(int key);
        string GetCodeGenerate(string tableName, string columName);
    }
}
