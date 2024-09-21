using static System.Net.Mime.MediaTypeNames;

namespace HomemadeCakes.Common.Config
{
    public class Mail : IMail
    {
        public bool Active
        {
            get
            {
                if (!SmtpActive)
                {
                    return ImapPopActive;
                }

                return true;
            }
        }

        public bool SmtpActive { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpAddress { get; set; }

        public string SmtpUser { get; set; }

        public string SmtpPassword { get; set; }

        public bool SSL { get; set; }

        public string ReceiveType { get; set; }

        public bool ImapPopActive { get; set; }

        public string ImapPopHost { get; set; }

        public int ImapPopPort { get; set; }

        public string ImapPopUser { get; set; }

        public string ImapPopPassword { get; set; }

        public string ApiHost { get; set; }

        public string ApiUser { get; set; }

        public string ApiPW { get; set; }

        public string ApiMailMethod { get; set; }

        public string ApiTokenMethod { get; set; }
    }
}
