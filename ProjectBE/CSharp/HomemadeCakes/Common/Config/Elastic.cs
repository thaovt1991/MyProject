namespace HomemadeCakes.Common.Config
{
    public class Elastic 
    {
        public bool IsActive => !string.IsNullOrEmpty(ESHost);

        public string ESHost { get; set; }

        public string ESUserName { get; set; }

        public string ESPass { get; set; }

        public string ApmHost { get; set; }

        public string ApmToken { get; set; }
    }
}
/*
 * Cung cấp thông tin cần thiết
 */
