namespace SGRH.Common.Common
{
    public interface IAppLogger<T>
    {
        void Info(string message, params object[] args);
        void ErrorNoEx(string message, params object[] args);
        void ErrorEx(Exception ex, string message, params object[] args);
    }
}
