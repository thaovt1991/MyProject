namespace HomemadeCakes.Common.Config
{
    public interface IMail
    {
        bool Active { get; }

        bool SmtpActive { get; set; }

        string SmtpHost { get; set; }

        int SmtpPort { get; set; }

        string SmtpAddress { get; set; }

        string SmtpUser { get; set; }

        string SmtpPassword { get; set; }

        bool SSL { get; set; }

        string ReceiveType { get; set; }

        bool ImapPopActive { get; set; }

        string ImapPopHost { get; set; }

        int ImapPopPort { get; set; }

        string ImapPopUser { get; set; }

        string ImapPopPassword { get; set; }

        string ApiHost { get; set; }

        string ApiUser { get; set; }

        string ApiPW { get; set; }

        string ApiMailMethod { get; set; }

        string ApiTokenMethod { get; set; }
    }
}
