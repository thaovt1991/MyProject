using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyCaching.Core;
using EasyCaching.Core.Configurations;
using EasyCaching.Core.DistributedLock;
using EasyCaching.CSRedis;
using EasyCaching.Redis;
using EasyCaching.InMemory;
using EasyCaching.Serialization.MessagePack; //1.9.2.0
using HomemadeCakes.Common.Config;
using Microsoft.EntityFrameworkCore.Internal;
using MessagePack.Resolvers; //'MessagePack, Version=2.5.0.0, 2.5.140
using Microsoft.Extensions.DependencyInjection;

namespace HomemadeCakes.Common
{
    public static class CacheManager
    {
        public static Func<Task> InitAction;

        private static IEasyCachingProvider _inProcess;

        private static IEasyCachingProvider _distributed;

        public static IEasyCachingProvider InProcess
        {
            get
            {
                if (_inProcess == null)
                {
                    InProcessCache();
                }

                return _inProcess;
            }
            private set
            {
                _inProcess = value;
            }
        }

        public static IEasyCachingProvider Distributed
        {
            get
            {
                if (_distributed == null)
                {
                    _distributed = GetDistributed();
                    if (_distributed == null)
                    {
                        _distributed = InProcess;
                    }
                }

                return _distributed;
            }
            set
            {
                _inProcess = value;
            }
        }

        public static string PublicPath => PJConfig.Settings.PublicPath;

        static CacheManager()
        {
        }

        public static void Configure(IEasyCachingProvider inProcess, IEasyCachingProvider distributed)
        {
            _distributed = inProcess;
            _distributed = distributed;
            if (InitAction != null)
            {
                Task.Run(async delegate
                {
                    await InitAction();
                });
            }
        }

        private static void InProcessCache()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddEasyCaching(delegate (EasyCachingOptions options)
            {
                options.UseInMemory("efinmemory").UseMemoryLock();
            });
            _inProcess = services.BuildServiceProvider().GetService<IEasyCachingProvider>();
        }

        public static IEasyCachingProvider GetDistributed(string tenant = "default", string cnnString = null)
        {
            if (_distributed != null)
            {
                return _distributed;
            }

            IEasyCachingProvider result = null;
            if (string.IsNullOrEmpty(cnnString) && PJConfig.Connections != null)
            {
                cnnString = PJConfig.Connections["Redis"];
            }

            if (string.IsNullOrEmpty(cnnString))
            {
                return result;
            }

            try
            {
                List<string> lstCnns = cnnString.Split('|').ToList();
                string text = PJConfig.Connections["redisSentinels"];
                List<string> lstSentinelsUrls = new List<string>();
                if (!string.IsNullOrEmpty(text))
                {
                    lstSentinelsUrls = text.Split('|').ToList();
                }

                ServiceCollection services = new ServiceCollection();
                services.AddEasyCaching(delegate (EasyCachingOptions options)
                {
                    if (PJConfig.Settings.CacheType != null && PJConfig.Settings.CacheType.Equals("redis", StringComparison.OrdinalIgnoreCase))
                    {
                        options.UseRedis(delegate (EasyCaching.Redis.RedisOptions config)
                        {
                            config.DBConfig.Configuration = cnnString;
                            config.SerializerName = "efredismsgpack";
                        }, "efdistributedcache").UseRedisLock().WithMessagePack(delegate (EasyCachingMsgPackSerializerOptions x)
                        {
                            x.EnableCustomResolver = true;
                            x.CustomResolvers = CompositeResolver.Create(TypelessContractlessStandardResolver.Instance);
                        }, "efredismsgpack");
                    }
                    else
                    {
                        options.UseCSRedis(delegate (EasyCaching.CSRedis.RedisOptions config)
                        {
                            config.DBConfig.ConnectionStrings = lstCnns;
                            config.DBConfig.Sentinels = lstSentinelsUrls;
                            config.DBConfig.ReadOnly = false;
                            config.SerializerName = "efredismsgpack";
                        }, "efdistributedcache").UseCSRedisLock().WithMessagePack(delegate (EasyCachingMsgPackSerializerOptions x)
                        {
                            x.EnableCustomResolver = true;
                            x.CustomResolvers = CompositeResolver.Create(TypelessContractlessStandardResolver.Instance);
                        }, "efredismsgpack");
                    }
                });
                result = services.BuildServiceProvider().GetService<IEasyCachingProvider>();
            }
            catch (Exception exceptxion)
            {
               // Log.Instance.Error(exception);
            }

            return result;
        }

        public static bool IsFolder(string path, bool assumeDneLookAlike = true)
        {
            if (Directory.Exists(path) || path.EndsWith(Path.DirectorySeparatorChar.ToString() ?? "") || path.EndsWith(Path.AltDirectorySeparatorChar.ToString() ?? ""))
            {
                return true;
            }

            if (File.Exists(path))
            {
                return false;
            }

            if (!Path.HasExtension(path) && assumeDneLookAlike)
            {
                return true;
            }

            return false;
        }

        public static string GetHostPath(params string[] subPaths)
        {
            string text = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            if (subPaths != null && subPaths.Length != 0)
            {
                text = Path.Combine(text, Path.Combine(subPaths));
            }

            string path = text;
            if (Path.HasExtension(path))
            {
                path = new FileInfo(text).DirectoryName;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return text;
        }

        public static string GetHostPathTemp(params string[] subPaths)
        {
            string text = "temp";
            if (subPaths != null && subPaths.Length != 0)
            {
                text = Path.Combine(text, Path.Combine(subPaths));
            }

            return GetHostPath(text);
        }

        public static string GetPublicPath(params string[] subPaths)
        {
            if (string.IsNullOrEmpty(PublicPath))
            {
                return "";
            }

            string text = PublicPath;
            if (subPaths != null && subPaths.Length != 0)
            {
                text = Path.Combine(text, Path.Combine(subPaths));
            }

            string path = text;
            if (Path.HasExtension(path))
            {
                path = new FileInfo(text).DirectoryName;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return text;
        }
    }
}
#if false // Decompilation log
'339' items in cache
------------------
Resolve: 'System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
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
Resolve: 'System.Threading, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Security.Cryptography.Csp, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Csp, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Csp.dll'
------------------
Resolve: 'System.Linq.Queryable, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Queryable, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Linq.Queryable.dll'
------------------
Resolve: 'System.Console, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Console, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Console.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration\7.0.0\lib\netstandard2.0\Microsoft.Extensions.Configuration.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.FileExtensions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.FileExtensions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration.fileextensions\7.0.0\lib\netstandard2.0\Microsoft.Extensions.Configuration.FileExtensions.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration.json\7.0.0\lib\netstandard2.1\Microsoft.Extensions.Configuration.Json.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.Binder, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.Binder, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration.binder\7.0.4\lib\netstandard2.0\Microsoft.Extensions.Configuration.Binder.dll'
------------------
Resolve: 'EasyCaching.InMemory, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.InMemory, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.inmemory\1.9.2\lib\netstandard2.0\EasyCaching.InMemory.dll'
------------------
Resolve: 'Serilog.Sinks.File, Version=5.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Sinks.File, Version=5.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.file\5.0.0\lib\net5.0\Serilog.Sinks.File.dll'
------------------
Resolve: 'System.ComponentModel.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.Primitives.dll'
------------------
Resolve: 'Polly.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Found single assembly: 'Polly.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\polly.core\8.4.0\lib\netstandard2.0\Polly.Core.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.Unsafe, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'System.Runtime.CompilerServices.Unsafe, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '6.0.0.0'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\system.runtime.compilerservices.unsafe\6.0.0\lib\netcoreapp3.1\System.Runtime.CompilerServices.Unsafe.dll'
#endif

/*
 * EasyCaching: Giải pháp đệm dữ liệu đơn giản và hiệu quả cho .NET
EasyCaching là một thư viện đệm dữ liệu (caching) linh hoạt và dễ sử dụng, được thiết kế đặc biệt cho các ứng dụng .NET. Nó cung cấp một lớp trừu tượng chung cho nhiều loại bộ nhớ đệm khác nhau, giúp các nhà phát triển dễ dàng tích hợp caching vào dự án của mình mà không cần phải lo lắng về các chi tiết triển khai phức tạp.

Tại sao nên sử dụng EasyCaching?
Đơn giản hóa việc sử dụng caching: EasyCaching cung cấp một API thống nhất để làm việc với nhiều loại bộ nhớ đệm khác nhau, như:
In-memory: Đệm dữ liệu trong bộ nhớ RAM của ứng dụng.
Distributed cache: Đệm dữ liệu trên nhiều máy chủ, ví dụ như Redis, Memcached.
Hybrid cache: Kết hợp nhiều loại bộ nhớ đệm để tối ưu hiệu suất.
Linh hoạt: Cho phép tùy chỉnh các cấu hình khác nhau, như thời gian sống của dữ liệu trong cache, chiến lược xóa dữ liệu, v.v.
Hiệu suất cao: Tối ưu hóa hiệu suất truy xuất dữ liệu bằng cách sử dụng các thuật toán caching thông minh.
Dễ tích hợp: Dễ dàng tích hợp vào các dự án .NET hiện có.
Các tính năng chính của EasyCaching:
Hỗ trợ nhiều loại bộ nhớ đệm: Redis, Memcached, MemoryCache, SQL Server, MongoDB, v.v.
Cung cấp các API đơn giản: Dễ dàng sử dụng để lưu, lấy và xóa dữ liệu trong cache.
Hỗ trợ đa luồng: An toàn khi sử dụng trong các môi trường đa luồng.
Hỗ trợ các tính năng nâng cao: Tự động làm mới dữ liệu, xóa dữ liệu theo thời gian, v.v.
Tích hợp với các framework khác: ASP.NET Core, Entity Framework Core.
Ví dụ sử dụng EasyCaching:
C#
using EasyCaching.Core;

// Cấu hình EasyCaching
var options = new EasyCachingOptions();
options.UseInMemory(); // Sử dụng bộ nhớ đệm trong RAM

// Tạo một IEasyCachingProvider
var factory = new EasyCachingServiceProviderFactory(options);
IEasyCachingProvider cache = factory.GetCachingProvider("default");

// Lưu dữ liệu vào cache
cache.Set("mykey", "myvalue", TimeSpan.FromSeconds(60));

// Lấy dữ liệu từ cache
string value = cache.Get<string>("mykey");
Hãy thận trọng khi sử dụng các đoạn mã.

Lợi ích của việc sử dụng caching:
Tăng tốc độ ứng dụng: Giảm thời gian truy cập dữ liệu bằng cách lấy dữ liệu từ cache thay vì truy cập cơ sở dữ liệu hoặc các dịch vụ khác.
Giảm tải cho hệ thống: Giảm tải cho cơ sở dữ liệu và các hệ thống backend khác.
Cải thiện trải nghiệm người dùng: Đảm bảo ứng dụng phản hồi nhanh hơn.
Tóm lại, EasyCaching là một công cụ hữu ích giúp các nhà phát triển .NET dễ dàng tích hợp caching vào ứng dụng của mình, từ đó cải thiện hiệu suất và khả năng mở rộng của ứng dụng.


MessagePack là một định dạng dữ liệu nhị phân nhẹ và hiệu quả được thiết kế để truyền dữ liệu giữa các hệ thống. Nó được tạo ra với mục tiêu thay thế JSON và XML trong các trường hợp cần truyền dữ liệu với kích thước nhỏ hơn và tốc độ nhanh hơn.

Tại sao chọn MessagePack?
Hiệu suất cao: MessagePack thường nhỏ gọn hơn JSON và XML từ 28% đến 50%, giúp giảm thời gian truyền dữ liệu và băng thông mạng.
Tốc độ nhanh: Quá trình mã hóa và giải mã MessagePack nhanh hơn so với các định dạng khác, nhờ vào cấu trúc dữ liệu được tối ưu hóa.
Ngôn ngữ trung lập: MessagePack có các thư viện hỗ trợ cho nhiều ngôn ngữ lập trình khác nhau, giúp dễ dàng tích hợp vào các dự án đa ngôn ngữ.
Sơ đồ dữ liệu: MessagePack có một sơ đồ dữ liệu đơn giản, dễ hiểu và linh hoạt, cho phép bạn định nghĩa các cấu trúc dữ liệu tùy chỉnh.
So sánh với JSON và XML
Tính năng	MessagePack	JSON	XML
Kích thước	Nhỏ gọn nhất	Trung bình	Lớn nhất
Tốc độ	Nhanh nhất	Trung bình	Chậm nhất
Sơ đồ dữ liệu	Đơn giản, linh hoạt	Đơn giản	Phức tạp
Sử dụng	Truyền dữ liệu hiệu quả, serialization/deserialization	Truyền dữ liệu, cấu hình	Cấu hình, dữ liệu có cấu trúc

Xuất sang Trang tính
Các trường hợp sử dụng phổ biến của MessagePack
Truyền dữ liệu giữa các microservices: MessagePack là một lựa chọn tuyệt vời để truyền dữ liệu giữa các microservices, giúp giảm thiểu độ trễ và tăng hiệu suất.
Serialization/deserialization: MessagePack có thể được sử dụng để serial hóa và deserialization các đối tượng trong các ngôn ngữ lập trình khác nhau.
Lưu trữ dữ liệu: MessagePack có thể được sử dụng để lưu trữ dữ liệu trong các cơ sở dữ liệu NoSQL hoặc các hệ thống phân tán.
Ví dụ về một đối tượng MessagePack
JSON
{
  "name": "John Doe",
  "age": 30,
  "is_active": true,
  "address": {
    "street": "123 Main St",
    "city": "Anytown"
  }
}
Hãy thận trọng khi sử dụng các đoạn mã.

Khi được mã hóa thành MessagePack, dữ liệu trên sẽ trở nên nhỏ gọn hơn và hiệu quả hơn.

Tổng kết
MessagePack là một công cụ mạnh mẽ và linh hoạt để truyền và lưu trữ dữ liệu. Nếu bạn đang tìm kiếm một định dạng dữ liệu hiệu quả, nhanh chóng và dễ sử dụng, MessagePack là một lựa chọn tuyệt vời.
 */

#if false // Decompilation log
'339' items in cache
------------------
Resolve: 'System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
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
Resolve: 'System.Threading, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Security.Cryptography.Csp, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Csp, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Csp.dll'
------------------
Resolve: 'System.Linq.Queryable, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Queryable, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Linq.Queryable.dll'
------------------
Resolve: 'System.Console, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Console, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Console.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration\7.0.0\lib\netstandard2.0\Microsoft.Extensions.Configuration.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.FileExtensions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.FileExtensions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration.fileextensions\7.0.0\lib\netstandard2.0\Microsoft.Extensions.Configuration.FileExtensions.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration.json\7.0.0\lib\netstandard2.1\Microsoft.Extensions.Configuration.Json.dll'
------------------
Resolve: 'Microsoft.Extensions.Configuration.Binder, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Found single assembly: 'Microsoft.Extensions.Configuration.Binder, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\microsoft.extensions.configuration.binder\7.0.4\lib\netstandard2.0\Microsoft.Extensions.Configuration.Binder.dll'
------------------
Resolve: 'EasyCaching.InMemory, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'EasyCaching.InMemory, Version=1.9.2.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\easycaching.inmemory\1.9.2\lib\netstandard2.0\EasyCaching.InMemory.dll'
------------------
Resolve: 'Serilog.Sinks.File, Version=5.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Found single assembly: 'Serilog.Sinks.File, Version=5.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\serilog.sinks.file\5.0.0\lib\net5.0\Serilog.Sinks.File.dll'
------------------
Resolve: 'System.ComponentModel.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Primitives, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.Primitives.dll'
------------------
Resolve: 'Polly.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Found single assembly: 'Polly.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\polly.core\8.4.0\lib\netstandard2.0\Polly.Core.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.Unsafe, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'System.Runtime.CompilerServices.Unsafe, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '5.0.0.0', Got: '6.0.0.0'
Load from: 'C:\Users\THAO_LAPTOP\.nuget\packages\system.runtime.compilerservices.unsafe\6.0.0\lib\netcoreapp3.1\System.Runtime.CompilerServices.Unsafe.dll'
#endif

