using System;
using System.IO;
using System.Reflection;
using HomemadeCakes.Common.Config;
using Serilog;
using Serilog.Configuration;//Serilog.Configuration là một namespace trong thư viện Serilog của .NET, cung cấp các lớp và phương thức giúp bạn cấu hình chi tiết cách thức các sự kiện log được ghi lại.
//Nó cho phép bạn tùy chỉnh các khía cạnh như:
//Mức độ log: Quyết định những sự kiện nào sẽ được ghi lại (Debug, Information, Warning, Error, Fatal).
//Đích đến: Nơi lưu trữ log, có thể là console, file, database, hoặc các hệ thống giám sát như Elasticsearch, Seq.
//Định dạng: Cách thức các sự kiện log được hiển thị, bao gồm việc thêm các thông tin bổ sung như timestamp, thread ID, properties.
//Các bộ lọc: Chỉ ghi lại những sự kiện đáp ứng các điều kiện nhất định.
using Serilog.Events;
using Serilog.Formatting.Compact;////2.0,là một thư viện trong hệ sinh thái Serilog, một thư viện ghi log mạnh mẽ và linh hoạt cho các ứng dụng .NET. Thư viện này được thiết kế đặc biệt để định dạng các sự kiện log (log events) thành một định dạng JSON gọn gàng và dễ đọc, thường được gọi là "compact JSON".
using Serilog.Sinks.Grafana.Loki;//8.3
                                 //Serilog.Sinks.File - #region Assembly Serilog.Sinks.File, Version=5.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10 - tai Serilog.Sinks.File


namespace HomemadeCakes.Common
{
    public static class Log
    {
        public static ILogger Instance { get; set; }

        static Log()
        {
            if (Instance == null)
            {
                Instance = GetConfiguration().CreateLogger();
            }
        }

        public static LoggerConfiguration GetConfiguration()
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration().MinimumLevel.Debug().MinimumLevel.Override("Microsoft", LogEventLevel.Warning).MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning).Enrich.FromLogContext().Enrich.WithClientIp().Enrich.With(new RemovePropertiesEnricher());
            if (PJConfig.Settings.IsLogFile)
            {
                loggerConfiguration.WriteTo.Async(delegate (LoggerSinkConfiguration x)
                {
                    RenderedCompactJsonFormatter formatter = new RenderedCompactJsonFormatter();
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "debug.log");
                    long? fileSizeLimitBytes = 10485760L;
                    int? retainedFileCountLimit = 15;
                    x.File(formatter, path, LogEventLevel.Verbose, fileSizeLimitBytes, null, buffered: false, shared: true, null, RollingInterval.Hour, rollOnFileSizeLimit: true, retainedFileCountLimit);
                });
            }
            else if (!string.IsNullOrEmpty(PJConfig.Settings.Grafana.Loki))
            {
                loggerConfiguration.WriteTo.GrafanaLoki(PJConfig.Settings.Grafana.Loki);
            }
            else
            {
                loggerConfiguration.WriteTo.Console(new RenderedCompactJsonFormatter());
            }

            return loggerConfiguration;
        }
    }

}
//Serilog.Formatting.Compact là gì?
//Serilog.Formatting.Compact là một thư viện trong hệ sinh thái Serilog, một thư viện ghi log mạnh mẽ và linh hoạt cho các ứng dụng .NET. Thư viện này được thiết kế đặc biệt để định dạng các sự kiện log (log events) thành một định dạng JSON gọn gàng và dễ đọc, thường được gọi là "compact JSON".

//Tại sao sử dụng Serilog.Formatting.Compact?
//Định dạng JSON gọn gàng: Định dạng JSON compact giúp cho việc phân tích và tìm kiếm log trở nên dễ dàng hơn, đặc biệt khi sử dụng các công cụ như Elasticsearch, Kibana.
//Hiệu suất cao: Thư viện này được tối ưu hóa để tạo ra các chuỗi JSON một cách hiệu quả, giúp giảm thiểu tác động đến hiệu năng của ứng dụng.
//Tùy chỉnh cao: Bạn có thể tùy chỉnh cách thức các sự kiện log được định dạng thành JSON bằng cách cấu hình các mẫu (templates).
//Phổ biến: Được sử dụng rộng rãi trong cộng đồng .NET, đặc biệt là trong các ứng dụng microservices và các hệ thống phân tán.

//Ứng dụng thực tế
//Serilog.Formatting.Compact thường được sử dụng trong các trường hợp sau:

//Ghi log vào file: Định dạng JSON compact giúp cho việc phân tích log bằng các công cụ như Elasticsearch, Kibana trở nên dễ dàng hơn.
//Ghi log vào hệ thống tập trung: Nhiều hệ thống tập trung như ELK Stack (Elasticsearch, Logstash, Kibana) hỗ trợ định dạng JSON, vì vậy việc sử dụng Serilog.Formatting.Compact sẽ giúp tích hợp log vào các hệ thống này một cách trơn tru.
//Xây dựng các hệ thống giám sát: Định dạng JSON compact giúp dễ dàng trích xuất các thông tin cần thiết để xây dựng các dashboard và báo cáo.
/*
 * Serilog.Sinks.Async là một thư viện .NET mạnh mẽ, được thiết kế để cải thiện hiệu suất và độ tin cậy của việc ghi nhật ký Serilog bằng cách giới thiệu việc ghi vào sink một cách không đồng bộ. Điều này có nghĩa là các sự kiện nhật ký không được xử lý ngay lập tức bởi các sink cơ bản, mà thay vào đó được xếp hàng để xử lý không đồng bộ, cho phép ứng dụng tiếp tục công việc của mình mà không bị chặn.

Lợi ích chính:

Hiệu suất được cải thiện: Viết không đồng bộ ngăn chặn ứng dụng bị chậm lại do các hoạt động ghi nhật ký, đặc biệt khi xử lý khối lượng dữ liệu nhật ký lớn hoặc các sink chậm.
Độ tin cậy tăng: Bằng cách tách rời việc ghi nhật ký khỏi luồng chính của ứng dụng, nguy cơ chết tiệt hoặc ứng dụng bị sập do các vấn đề ghi nhật ký được giảm đáng kể.
Tính linh hoạt: Serilog.Sinks.Async có thể được sử dụng với nhiều loại sink Serilog khác nhau, bao gồm tệp, bảng điều khiển, cơ sở dữ liệu và các sink tùy chỉnh.
Cấu hình: Thư viện cung cấp các tùy chọn cấu hình khác nhau để điều khiển kích thước hàng đợi không đồng bộ, hành vi ghép nối và các tham số khác.
Cách sử dụng:

Cài đặt gói NuGet:

Bash
Install-Package Serilog.Sinks.Async
Hãy thận trọng khi sử dụng các đoạn mã.

Tạo một trình ghi nhật ký Serilog:

C#
var logger = new LoggerConfiguration()
    .WriteTo.Async(a => a
        .Queue(1000) // Đặt kích thước hàng đợi thành 1000
        .Batch(1000, TimeSpan.FromSeconds(1)) // Ghép 1000 sự kiện hoặc đợi trong 1 giây
        .WriteTo.Console() // Ghi vào bảng điều khiển
    )
    .CreateLogger();
Hãy thận trọng khi sử dụng các đoạn mã.

Sử dụng trình ghi nhật ký:

C#
logger.Information("Hello, world!");
Hãy thận trọng khi sử dụng các đoạn mã.

Tùy chọn cấu hình:

Kích thước hàng đợi: Thiết lập số lượng tối đa các sự kiện nhật ký có thể được xếp hàng trước khi ứng dụng bắt đầu chặn.
Ghép nối: Cấu hình kích thước lô và khoảng thời gian để ghi các sự kiện nhật ký vào sink cơ bản.
Hành vi tràn: Xác định những gì xảy ra khi hàng đợi đầy và các sự kiện nhật ký mới được thêm vào.
Xử lý lỗi: Điều khiển cách xử lý lỗi trong quá trình ghi không đồng bộ.
Xét thêm:

Đối với các trường hợp phức tạp hoặc yêu cầu cụ thể, bạn có thể cần tùy chỉnh hàng đợi không đồng bộ hoặc xử lý lỗi.
Cân nhắc sử dụng Serilog.Sinks.Async với các kỹ thuật tối ưu hóa hiệu suất khác, chẳng hạn như E/A không đồng bộ hoặc nhóm luồng.
 * 
 * 
 * 
 * 
 * 
 
 * 
 * 
 * 
 * 
 * 
 * 
 * ADD THu vien
 * 
 * Resolve: 'System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.dll'
------------------
Resolve: 'System.Linq.Expressions, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Expressions, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Linq.Expressions.dll'
------------------
Resolve: 'System.Reflection.Emit, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Emit.dll'
------------------
Resolve: 'System.Collections.Concurrent, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.Concurrent, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Collections.Concurrent.dll'
------------------
Resolve: 'System.Collections, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Collections.dll'
------------------
Resolve: 'System.Security.Cryptography.Algorithms, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Algorithms, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Algorithms.dll'
------------------
Resolve: 'System.Security.Cryptography.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Primitives.dll'
------------------
Resolve: 'EasyCaching.Core, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.Core, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.core\1.9.2\lib\netstandard2.0\EasyCaching.Core.dll'
------------------
Resolve: 'EasyCaching.Serialization.MessagePack, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.Serialization.MessagePack, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.serialization.messagepack\1.9.2\lib\netstandard2.0\EasyCaching.Serialization.MessagePack.dll'
------------------
Resolve: 'EasyCaching.Redis, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.Redis, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.redis\1.9.2\lib\netstandard2.0\EasyCaching.Redis.dll'
------------------
Resolve: 'EasyCaching.CSRedis, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.CSRedis, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.csredis\1.9.2\lib\netstandard2.0\EasyCaching.CSRedis.dll'
------------------
Resolve: 'System.Reflection.Emit.ILGeneration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit.ILGeneration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Emit.ILGeneration.dll'
------------------
Resolve: 'System.Linq, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Linq.dll'
------------------
Resolve: 'System.Data.Common, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Data.Common, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Data.Common.dll'
------------------
Resolve: 'System.ComponentModel.Annotations, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Annotations, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.Annotations.dll'
------------------
Resolve: 'DocumentFormat.OpenXml, Version=3.0.2.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17'
Found single assembly: 'DocumentFormat.OpenXml, Version=3.0.2.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\documentformat.openxml\3.0.2\lib\netstandard2.0\DocumentFormat.OpenXml.dll'
------------------
Resolve: 'DocumentFormat.OpenXml.Framework, Version=3.0.2.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17'
Found single assembly: 'DocumentFormat.OpenXml.Framework, Version=3.0.2.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\documentformat.openxml.framework\3.0.2\lib\netstandard2.0\DocumentFormat.OpenXml.Framework.dll'
------------------
Resolve: 'Microsoft.Extensions.Caching.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Caching.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.caching.abstractions\7.0.0\lib\netstandard2.0\Microsoft.Extensions.Caching.Abstractions.dll'
------------------
Resolve: 'System.Runtime.Serialization.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Serialization.Primitives.dll'
------------------
Resolve: 'Serilog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog\3.1.1\lib\net5.0\Serilog.dll'
------------------
Resolve: 'System.Text.RegularExpressions, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Text.RegularExpressions, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Text.RegularExpressions.dll'
------------------
Resolve: 'MessagePack, Version=2.5.0.0, Culture=neutral, PublicKeyToken=b4a0369545f0a1be'
Found single assembly: 'MessagePack, Version=2.5.0.0, Culture=neutral, PublicKeyToken=b4a0369545f0a1be'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\messagepack\2.5.140\lib\netstandard2.0\MessagePack.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration.abstractions\7.0.0\lib\netstandard2.0\Microsoft.Extensions.Configuration.Abstractions.dll'
------------------
Resolve: 'System.ComponentModel.TypeConverter, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.TypeConverter, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.TypeConverter.dll'
------------------
Resolve: 'System.Threading.Tasks.Dataflow, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Tasks.Dataflow, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Tasks.Dataflow.dll'
------------------
Resolve: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Found single assembly: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\newtonsoft.json\13.0.3\lib\netstandard2.0\Newtonsoft.Json.dll'
------------------
Resolve: 'MessagePack.Annotations, Version=2.5.0.0, Culture=neutral, PublicKeyToken=b4a0369545f0a1be'
Found single assembly: 'MessagePack.Annotations, Version=2.5.0.0, Culture=neutral, PublicKeyToken=b4a0369545f0a1be'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\messagepack.annotations\2.5.140\lib\netstandard2.0\MessagePack.Annotations.dll'
------------------
Resolve: 'System.ObjectModel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ObjectModel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ObjectModel.dll'
------------------
Resolve: 'MimeKit, Version=3.6.0.0, Culture=neutral, PublicKeyToken=bede1c8a46c66814'
Found single assembly: 'MimeKit, Version=3.6.0.0, Culture=neutral, PublicKeyToken=bede1c8a46c66814'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\mimekit\3.6.0\lib\netstandard2.1\MimeKit.dll'
------------------
Resolve: 'Ical.Net, Version=1.0.0.0, Culture=neutral, PublicKeyToken=65c0446cd019ea53'
Found single assembly: 'Ical.Net, Version=1.0.0.0, Culture=neutral, PublicKeyToken=65c0446cd019ea53'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\ical.net\4.2.0\lib\net5.0\Ical.Net.dll'
------------------
Resolve: 'HtmlAgilityPack, Version=1.11.61.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a'
Found single assembly: 'HtmlAgilityPack, Version=1.11.61.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\htmlagilitypack\1.11.61\lib\netstandard2.0\HtmlAgilityPack.dll'
------------------
Resolve: 'System.Net.Security, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Security, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Security.dll'
------------------
Resolve: 'System.Security.Cryptography.X509Certificates, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.X509Certificates, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.X509Certificates.dll'
------------------
Resolve: 'System.Net.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Primitives.dll'
------------------
Resolve: 'MailKit, Version=3.6.0.0, Culture=neutral, PublicKeyToken=4e064fe7c44a8f1b'
Found single assembly: 'MailKit, Version=3.6.0.0, Culture=neutral, PublicKeyToken=4e064fe7c44a8f1b'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\mailkit\3.6.0\lib\netstandard2.1\MailKit.dll'
------------------
Resolve: 'System.Diagnostics.TraceSource, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.TraceSource, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.TraceSource.dll'
------------------
Resolve: 'System.Text.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Text.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\system.text.json\7.0.0\lib\netstandard2.0\System.Text.Json.dll'
------------------
Resolve: 'RabbitMQ.Client, Version=6.0.0.0, Culture=neutral, PublicKeyToken=89e7d7c5feba84ce'
Found single assembly: 'RabbitMQ.Client, Version=6.0.0.0, Culture=neutral, PublicKeyToken=89e7d7c5feba84ce'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\rabbitmq.client\6.8.1\lib\netstandard2.0\RabbitMQ.Client.dll'
------------------
Resolve: 'Autofac, Version=8.0.0.0, Culture=neutral, PublicKeyToken=17863af14b0044da'
Found single assembly: 'Autofac, Version=8.0.0.0, Culture=neutral, PublicKeyToken=17863af14b0044da'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\autofac\8.0.0\lib\netstandard2.1\Autofac.dll'
------------------
Resolve: 'Polly, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Found single assembly: 'Polly, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\polly\8.4.0\lib\netstandard2.0\Polly.dll'
------------------
Resolve: 'Microsoft.Extensions.Logging.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Logging.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.logging.abstractions\7.0.0\lib\netstandard2.0\Microsoft.Extensions.Logging.Abstractions.dll'
------------------
Resolve: 'Confluent.Kafka, Version=2.4.0.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e'
Found single assembly: 'Confluent.Kafka, Version=2.4.0.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\confluent.kafka\2.4.0\lib\netstandard2.0\Confluent.Kafka.dll'
------------------
Resolve: 'Microsoft.Extensions.Http, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Http, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.http\7.0.0\lib\netstandard2.0\Microsoft.Extensions.Http.dll'
------------------
Resolve: 'System.Net.Http, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Http, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Http.dll'
------------------
Resolve: 'Microsoft.Extensions.DependencyInjection.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.DependencyInjection.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.dependencyinjection.abstractions\7.0.0\lib\netstandard2.1\Microsoft.Extensions.DependencyInjection.Abstractions.dll'
------------------
Resolve: 'Microsoft.Extensions.DependencyInjection, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.DependencyInjection, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.dependencyinjection\7.0.0\lib\netstandard2.1\Microsoft.Extensions.DependencyInjection.dll'
------------------
Resolve: 'System.ComponentModel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.dll'
------------------
Resolve: 'System.IO.FileSystem, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.FileSystem, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.FileSystem.dll'
------------------
Resolve: 'System.Reflection.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Primitives.dll'
------------------
Resolve: 'Fasterflect, Version=3.0.0.0, Culture=neutral, PublicKeyToken=38d18473284c1ca7'
Found single assembly: 'Fasterflect, Version=3.0.0.0, Culture=neutral, PublicKeyToken=38d18473284c1ca7'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\fasterflect\3.0.0\lib\netstandard20\Fasterflect.dll'
------------------
Resolve: 'Microsoft.CSharp, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'Microsoft.CSharp, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\Microsoft.CSharp.dll'
------------------
Resolve: 'System.Web.HttpUtility, Version=5.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Web.HttpUtility, Version=5.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Web.HttpUtility.dll'
------------------
Resolve: 'System.Linq.Async, Version=6.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263'
Found single assembly: 'System.Linq.Async, Version=6.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\system.linq.async\6.0.1\ref\netstandard2.1\System.Linq.Async.dll'
------------------
Resolve: 'Serilog.Enrichers.ClientInfo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Serilog.Enrichers.ClientInfo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.enrichers.clientinfo\2.0.3\lib\netstandard2.1\Serilog.Enrichers.ClientInfo.dll'
------------------
Resolve: 'Serilog.Sinks.Async, Version=1.5.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Sinks.Async, Version=1.5.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.async\1.5.0\lib\netstandard2.0\Serilog.Sinks.Async.dll'
------------------
Resolve: 'Serilog.Sinks.Grafana.Loki, Version=8.3.0.0, Culture=neutral, PublicKeyToken=6a5ca2e48b0c9e92'
Found single assembly: 'Serilog.Sinks.Grafana.Loki, Version=8.3.0.0, Culture=neutral, PublicKeyToken=6a5ca2e48b0c9e92'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.grafana.loki\8.3.0\lib\net5.0\Serilog.Sinks.Grafana.Loki.dll'
------------------
Resolve: 'Serilog.Formatting.Compact, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Formatting.Compact, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.formatting.compact\2.0.0\lib\netstandard2.1\Serilog.Formatting.Compact.dll'
------------------
Resolve: 'Serilog.Sinks.Console, Version=5.0.1.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Sinks.Console, Version=5.0.1.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.console\5.0.1\lib\net5.0\Serilog.Sinks.Console.dll'
------------------
Resolve: 'System.Runtime.InteropServices.RuntimeInformation, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices.RuntimeInformation, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.InteropServices.RuntimeInformation.dll'
------------------
Resolve: 'System.Threading.Tasks.Parallel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Tasks.Parallel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Tasks.Parallel.dll'
------------------
 */