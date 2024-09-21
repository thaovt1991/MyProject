using Amazon.Runtime.Internal.Util;
using System.Diagnostics;
using System;

namespace HomemadeCakes.Common.Config
{
    public class Kafka
    {
        public bool IsActive => !string.IsNullOrEmpty(BootstrapServers);

        public string BootstrapServers { get; set; }

        public string SslCaLocation { get; set; }

        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
    }
}

/*Apache Kafka là một nền tảng xử lý dữ liệu luồng (stream processing) phân tán, mã nguồn mở, được thiết kế để xử lý lượng lớn dữ liệu trong thời gian thực. Nó được sử dụng rộng rãi trong các hệ thống phân tán để xây dựng các ứng dụng xử lý dữ liệu luồng, các hệ thống truyền thông thời gian thực và các ứng dụng big data.

Kafka hoạt động như thế nào?
Kafka hoạt động dựa trên khái niệm topics và partitions.

Topic: Là một chủ đề mà các tin nhắn được gửi đến. Ví dụ: "user_logins", "product_orders".
Partition: Là một phân vùng của topic, giúp phân tán dữ liệu và tăng khả năng mở rộng.
Các producer gửi tin nhắn đến các topic cụ thể. Các consumer (người tiêu dùng) thì đăng ký vào các topic để nhận các tin nhắn mới. Kafka đảm bảo rằng các tin nhắn được phân phối đến các consumer một cách đáng tin cậy và theo thứ tự.

Tại sao sử dụng Kafka?
Xử lý dữ liệu thời gian thực: Kafka rất hiệu quả trong việc xử lý các luồng dữ liệu lớn đến liên tục.
Khả năng mở rộng cao: Kafka có thể dễ dàng mở rộng để xử lý lượng dữ liệu ngày càng tăng.
Đảm bảo tính tin cậy: Kafka đảm bảo rằng các tin nhắn được lưu trữ và phân phối một cách đáng tin cậy.
Cung cấp khả năng tái sử dụng dữ liệu: Dữ liệu được lưu trữ trong Kafka có thể được tái sử dụng nhiều lần bởi các ứng dụng khác nhau.
Cộng đồng lớn: Kafka có một cộng đồng người dùng lớn và được hỗ trợ bởi Apache Software Foundation.
Ứng dụng của Kafka
Xây dựng các hệ thống truyền thông thời gian thực: Chat, notification, IoT.
Xử lý log: Thu thập và phân tích log từ nhiều hệ thống.
Xây dựng các hệ thống recommendation: Phân tích hành vi người dùng để đưa ra các gợi ý.
Xử lý dữ liệu streaming: Xử lý dữ liệu từ các cảm biến, thiết bị IoT.
Xây dựng các hệ thống ETL (Extract, Transform, Load): Trích xuất, chuyển đổi và tải dữ liệu từ các nguồn khác nhau.
So sánh với RabbitMQ
Cả Kafka và RabbitMQ đều là các message broker, nhưng chúng có những điểm khác biệt:

Mục tiêu: Kafka được thiết kế chủ yếu cho việc xử lý dữ liệu luồng trong thời gian thực, trong khi RabbitMQ tập trung vào việc trao đổi tin nhắn giữa các ứng dụng.
Hiệu suất: Kafka thường có hiệu suất cao hơn RabbitMQ khi xử lý lượng lớn dữ liệu.
Mẫu thiết kế: Kafka sử dụng mô hình publish-subscribe, trong khi RabbitMQ hỗ trợ cả mô hình publish-subscribe và point-to-point.
Tóm lại
Kafka là một công cụ mạnh mẽ và linh hoạt để xây dựng các hệ thống xử lý dữ liệu luồng. Nếu bạn đang tìm kiếm một giải pháp để xử lý lượng lớn dữ liệu trong thời gian thực, Kafka là một lựa chọn tuyệt vời.
*/