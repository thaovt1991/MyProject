namespace HomemadeCakes.Model.Common
{
    public class RequestBase
    {
        public object[] Data { get; set; }
        public string MethodName { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public string TenantID { get; set; }

    }
}
