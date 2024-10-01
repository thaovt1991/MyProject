using HomemadeCakes.Common;

namespace HomemadeCakes.Common.Entity
{
    public class EntityData
    {
        public string[] Keys { get; set; }

        public object Entity { get; set; }

        public object Original { get; set; }

        public string EntityName { get; set; }

        public DataState State { get; set; }
    }
}
#region Assembly ERM.Common, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\THAO_LAPTOP\.nuget\packages\erm.common\2.4.0\lib\net5.0\ERM.Common.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

#if false // Decompilation log
'409' items in cache
------------------
Resolve: 'System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Runtime.dll'
------------------
Resolve: 'System.Linq.Expressions, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Expressions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Linq.Expressions.dll'
------------------
Resolve: 'System.Reflection.Emit, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Reflection.Emit.dll'
------------------
Resolve: 'System.Collections.Concurrent, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.Concurrent, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Collections.Concurrent.dll'
------------------
Resolve: 'System.Collections, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Collections.dll'
------------------
Resolve: 'System.Security.Cryptography.Algorithms, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Algorithms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Security.Cryptography.Algorithms.dll'
------------------
Resolve: 'System.Security.Cryptography.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Security.Cryptography.Primitives.dll'
------------------
Resolve: 'EasyCaching.Core, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.Core, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.core\1.9.2\lib\net6.0\EasyCaching.Core.dll'
------------------
Resolve: 'EasyCaching.Serialization.MessagePack, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.Serialization.MessagePack, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.serialization.messagepack\1.9.2\lib\net6.0\EasyCaching.Serialization.MessagePack.dll'
------------------
Resolve: 'EasyCaching.Redis, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.Redis, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.redis\1.9.2\lib\net6.0\EasyCaching.Redis.dll'
------------------
Resolve: 'EasyCaching.CSRedis, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.CSRedis, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.csredis\1.9.2\lib\net6.0\EasyCaching.CSRedis.dll'
------------------
Resolve: 'System.Reflection.Emit.ILGeneration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit.ILGeneration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Reflection.Emit.ILGeneration.dll'
------------------
Resolve: 'System.Linq, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Linq.dll'
------------------
Resolve: 'System.Data.Common, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Data.Common, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Data.Common.dll'
------------------
Resolve: 'System.ComponentModel.Annotations, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Annotations, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.ComponentModel.Annotations.dll'
------------------
Resolve: 'DocumentFormat.OpenXml, Version=3.0.2.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17'
Found single assembly: 'DocumentFormat.OpenXml, Version=3.0.2.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\documentformat.openxml\3.0.2\lib\net8.0\DocumentFormat.OpenXml.dll'
------------------
Resolve: 'DocumentFormat.OpenXml.Framework, Version=3.0.2.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17'
Found single assembly: 'DocumentFormat.OpenXml.Framework, Version=3.0.2.0, Culture=neutral, PublicKeyToken=8fb06cb64d019a17'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\documentformat.openxml.framework\3.0.2\lib\net8.0\DocumentFormat.OpenXml.Framework.dll'
------------------
Resolve: 'Microsoft.Extensions.Caching.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Caching.Abstractions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.Caching.Abstractions.dll'
------------------
Resolve: 'System.Runtime.Serialization.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Runtime.Serialization.Primitives.dll'
------------------
Resolve: 'Serilog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog\3.1.1\lib\net7.0\Serilog.dll'
------------------
Resolve: 'System.Text.RegularExpressions, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Text.RegularExpressions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Text.RegularExpressions.dll'
------------------
Resolve: 'MessagePack, Version=2.5.0.0, Culture=neutral, PublicKeyToken=b4a0369545f0a1be'
Found single assembly: 'MessagePack, Version=2.5.0.0, Culture=neutral, PublicKeyToken=b4a0369545f0a1be'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\messagepack\2.5.140\lib\net6.0\MessagePack.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.Abstractions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.Configuration.Abstractions.dll'
------------------
Resolve: 'System.ComponentModel.TypeConverter, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.TypeConverter, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.ComponentModel.TypeConverter.dll'
------------------
Resolve: 'System.Threading.Tasks.Dataflow, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Tasks.Dataflow, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Threading.Tasks.Dataflow.dll'
------------------
Resolve: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Found single assembly: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\newtonsoft.json\13.0.3\lib\net6.0\Newtonsoft.Json.dll'
------------------
Resolve: 'MessagePack.Annotations, Version=2.5.0.0, Culture=neutral, PublicKeyToken=b4a0369545f0a1be'
Found single assembly: 'MessagePack.Annotations, Version=2.5.0.0, Culture=neutral, PublicKeyToken=b4a0369545f0a1be'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\messagepack.annotations\2.5.140\lib\netstandard2.0\MessagePack.Annotations.dll'
------------------
Resolve: 'System.ObjectModel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ObjectModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.ObjectModel.dll'
------------------
Resolve: 'MimeKit, Version=3.6.0.0, Culture=neutral, PublicKeyToken=bede1c8a46c66814'
Found single assembly: 'MimeKit, Version=4.7.0.0, Culture=neutral, PublicKeyToken=bede1c8a46c66814'
WARN: Version mismatch. Expected: '3.6.0.0', Got: '4.7.0.0'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\mimekit\4.7.1\lib\net8.0\MimeKit.dll'
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
Found single assembly: 'System.Net.Security, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Net.Security.dll'
------------------
Resolve: 'System.Security.Cryptography.X509Certificates, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.X509Certificates, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Security.Cryptography.X509Certificates.dll'
------------------
Resolve: 'System.Net.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Net.Primitives.dll'
------------------
Resolve: 'MailKit, Version=3.6.0.0, Culture=neutral, PublicKeyToken=4e064fe7c44a8f1b'
Found single assembly: 'MailKit, Version=4.7.0.0, Culture=neutral, PublicKeyToken=4e064fe7c44a8f1b'
WARN: Version mismatch. Expected: '3.6.0.0', Got: '4.7.0.0'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\mailkit\4.7.1.1\lib\net8.0\MailKit.dll'
------------------
Resolve: 'System.Diagnostics.TraceSource, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.TraceSource, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Diagnostics.TraceSource.dll'
------------------
Resolve: 'System.Text.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Text.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Text.Json.dll'
------------------
Resolve: 'RabbitMQ.Client, Version=6.0.0.0, Culture=neutral, PublicKeyToken=89e7d7c5feba84ce'
Found single assembly: 'RabbitMQ.Client, Version=6.0.0.0, Culture=neutral, PublicKeyToken=89e7d7c5feba84ce'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\rabbitmq.client\6.8.1\lib\netstandard2.0\RabbitMQ.Client.dll'
------------------
Resolve: 'Autofac, Version=8.0.0.0, Culture=neutral, PublicKeyToken=17863af14b0044da'
Found single assembly: 'Autofac, Version=8.0.0.0, Culture=neutral, PublicKeyToken=17863af14b0044da'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\autofac\8.0.0\lib\net8.0\Autofac.dll'
------------------
Resolve: 'Polly, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Found single assembly: 'Polly, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\polly\8.4.0\lib\net6.0\Polly.dll'
------------------
Resolve: 'Microsoft.Extensions.Logging.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Logging.Abstractions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.Logging.Abstractions.dll'
------------------
Resolve: 'Confluent.Kafka, Version=2.4.0.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e'
Found single assembly: 'Confluent.Kafka, Version=2.4.0.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\confluent.kafka\2.4.0\lib\net6.0\Confluent.Kafka.dll'
------------------
Resolve: 'Microsoft.Extensions.Http, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Http, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.Http.dll'
------------------
Resolve: 'System.Net.Http, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Http, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Net.Http.dll'
------------------
Resolve: 'Microsoft.Extensions.DependencyInjection.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.DependencyInjection.Abstractions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.DependencyInjection.Abstractions.dll'
------------------
Resolve: 'Microsoft.Extensions.DependencyInjection, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.DependencyInjection, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.DependencyInjection.dll'
------------------
Resolve: 'System.ComponentModel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.ComponentModel.dll'
------------------
Resolve: 'System.IO.FileSystem, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.FileSystem, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.IO.FileSystem.dll'
------------------
Resolve: 'System.Reflection.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Reflection.Primitives.dll'
------------------
Resolve: 'Fasterflect, Version=3.0.0.0, Culture=neutral, PublicKeyToken=38d18473284c1ca7'
Found single assembly: 'Fasterflect, Version=3.0.0.0, Culture=neutral, PublicKeyToken=38d18473284c1ca7'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\fasterflect\3.0.0\lib\netstandard20\Fasterflect.dll'
------------------
Resolve: 'Microsoft.CSharp, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'Microsoft.CSharp, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\Microsoft.CSharp.dll'
------------------
Resolve: 'System.Web.HttpUtility, Version=5.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Web.HttpUtility, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Web.HttpUtility.dll'
------------------
Resolve: 'System.Linq.Async, Version=6.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263'
Found single assembly: 'System.Linq.Async, Version=6.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\system.linq.async\6.0.1\ref\net6.0\System.Linq.Async.dll'
------------------
Resolve: 'Serilog.Enrichers.ClientInfo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Serilog.Enrichers.ClientInfo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.enrichers.clientinfo\2.0.3\lib\net7.0\Serilog.Enrichers.ClientInfo.dll'
------------------
Resolve: 'Serilog.Sinks.Async, Version=1.5.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Sinks.Async, Version=1.5.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.async\1.5.0\lib\netstandard2.0\Serilog.Sinks.Async.dll'
------------------
Resolve: 'Serilog.Sinks.Grafana.Loki, Version=8.3.0.0, Culture=neutral, PublicKeyToken=6a5ca2e48b0c9e92'
Found single assembly: 'Serilog.Sinks.Grafana.Loki, Version=8.3.0.0, Culture=neutral, PublicKeyToken=6a5ca2e48b0c9e92'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.grafana.loki\8.3.0\lib\net7.0\Serilog.Sinks.Grafana.Loki.dll'
------------------
Resolve: 'Serilog.Formatting.Compact, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Formatting.Compact, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.formatting.compact\2.0.0\lib\net7.0\Serilog.Formatting.Compact.dll'
------------------
Resolve: 'Serilog.Sinks.Console, Version=5.0.1.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Sinks.Console, Version=5.0.1.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.console\5.0.1\lib\net7.0\Serilog.Sinks.Console.dll'
------------------
Resolve: 'System.Runtime.InteropServices.RuntimeInformation, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices.RuntimeInformation, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Runtime.InteropServices.RuntimeInformation.dll'
------------------
Resolve: 'System.Threading.Tasks.Parallel, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Tasks.Parallel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Threading.Tasks.Parallel.dll'
------------------
Resolve: 'System.Threading, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Threading.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Security.Cryptography.Csp, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Csp, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Security.Cryptography.Csp.dll'
------------------
Resolve: 'System.Linq.Queryable, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Queryable, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Linq.Queryable.dll'
------------------
Resolve: 'System.Console, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Console, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Console.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.Configuration.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.FileExtensions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.FileExtensions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.Configuration.FileExtensions.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.Configuration.Json.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.Binder, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.Binder, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.Configuration.Binder.dll'
------------------
Resolve: 'EasyCaching.InMemory, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.InMemory, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.inmemory\1.9.2\lib\net6.0\EasyCaching.InMemory.dll'
------------------
Resolve: 'Serilog.Sinks.File, Version=5.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Sinks.File, Version=5.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.file\5.0.0\lib\net5.0\Serilog.Sinks.File.dll'
------------------
Resolve: 'System.Security.Cryptography, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Security.Cryptography.dll'
------------------
Resolve: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Runtime.dll'
------------------
Resolve: 'System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '7.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Runtime.dll'
------------------
Resolve: 'System.ComponentModel.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.ComponentModel.Primitives.dll'
------------------
Resolve: 'System.ObjectModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ObjectModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.ObjectModel.dll'
------------------
Resolve: 'Polly.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Found single assembly: 'Polly.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\polly.core\8.4.0\lib\net8.0\Polly.Core.dll'
------------------
Resolve: 'Microsoft.Extensions.DependencyInjection.Abstractions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.DependencyInjection.Abstractions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\8.0.5\ref\net8.0\Microsoft.Extensions.DependencyInjection.Abstractions.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.Unsafe, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'System.Runtime.CompilerServices.Unsafe, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.5\ref\net8.0\System.Runtime.CompilerServices.Unsafe.dll'
#endif

