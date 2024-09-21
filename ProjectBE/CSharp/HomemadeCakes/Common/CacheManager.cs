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
//using EasyCaching.Serialization.MessagePack;
using HomemadeCakes.Common.Config;
using Microsoft.EntityFrameworkCore.Internal;
//using MessagePack.Resolvers;
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
                        //options.UseRedis(delegate (EasyCaching.Redis.RedisOptions config)
                        //{
                        //    config.DBConfig.Configuration = cnnString;
                        //    config.SerializerName = "efredismsgpack";
                        //}, "efdistributedcache").UseRedisLock().WithMessagePack(delegate (EasyCachingMsgPackSerializerOptions x)
                        //{
                        //    x.EnableCustomResolver = true;
                        //    x.CustomResolvers = CompositeResolver.Create(TypelessContractlessStandardResolver.Instance);
                        //}, "efredismsgpack");
                    }
                    else
                    {
                        //options.UseCSRedis(delegate (EasyCaching.CSRedis.RedisOptions config)
                        //{
                        //    config.DBConfig.ConnectionStrings = lstCnns;
                        //    config.DBConfig.Sentinels = lstSentinelsUrls;
                        //    config.DBConfig.ReadOnly = false;
                        //    config.SerializerName = "efredismsgpack";
                        //}, "efdistributedcache").UseCSRedisLock().WithMessagePack(delegate (EasyCachingMsgPackSerializerOptions x)
                        //{
                        //    x.EnableCustomResolver = true;
                        //    x.CustomResolvers = CompositeResolver.Create(TypelessContractlessStandardResolver.Instance);
                        //}, "efredismsgpack");
                    }
                });
                result = services.BuildServiceProvider().GetService<IEasyCachingProvider>();
            }
            catch (Exception exception)
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

        //Hàm tự tạo
        public static bool SetCacheCustomerPJ(string key, string value, TimeSpan timeLive)
        {
            // Cấu hình EasyCaching
            var options = new EasyCachingOptions();
            options.UseInMemory(); // Sử dụng bộ nhớ đệm trong RAM

            // Tạo một IEasyCachingProvider
            var factory = new EasyCachingServiceProviderFactory(options);
            IEasyCachingProvider cache = factory.GetCachingProvider("default");

            // Lưu dữ liệu vào cache
            cache.Set(key, value, TimeSpan.FromSeconds(60));

            // Lấy dữ liệu từ cache
            //string value = cache.Get<string>("mykey");
            return true;
        }
    }
}

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
