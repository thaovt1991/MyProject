using System.Threading.Tasks;
using System;
using HomemadeCakes.Common.Interface;
using System.Data;//với dữ liệu trong các ứng dụng C# của mình. Nó cung cấp cho bạn một bộ công cụ mạnh mẽ để kết nối với các cơ sở dữ liệu, truy xuất, xử lý và cập nhật dữ liệu.

namespace HomemadeCakes.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        string UserID { get; set; }

        string BUID { get; set; }

        object Orm { get; }

        IServiceWorker ServiceWorker { get; set; }

        int SaveChanges(bool transaction = false, IsolationLevel level = IsolationLevel.ReadUncommitted);//lưu 2 đối tượng cùng lúc thì báo

        Task<int> SaveChangesAsync(bool transaction = false, IsolationLevel level = IsolationLevel.ReadUncommitted);

        new void Dispose();
    }
}
//Các mức IsolationLevel phổ biến:
//ReadUncommitted: Mức độ cô lập thấp nhất. Một giao dịch có thể đọc được dữ liệu chưa được xác nhận (uncommitted) của một giao dịch khác. Điều này có thể dẫn đến việc đọc được dữ liệu không chính xác hoặc không nhất quán.
//ReadCommitted: Một giao dịch chỉ có thể đọc được dữ liệu đã được xác nhận bởi các giao dịch khác. Điều này giúp tránh được việc đọc dữ liệu chưa hoàn chỉnh.
//RepeatableRead: Một giao dịch sẽ luôn đọc được cùng một giá trị cho một hàng cụ thể, ngay cả khi các giao dịch khác đã cập nhật hàng đó. Tuy nhiên, vẫn có thể xảy ra hiện tượng phantom read.
//Serializable: Mức độ cô lập cao nhất. Một giao dịch sẽ nhìn thấy cơ sở dữ liệu như thể chỉ có một giao dịch đang hoạt động. Điều này ngăn chặn tất cả các loại xung đột dữ liệu, nhưng cũng làm giảm hiệu suất.
//IsolationLevel là một khái niệm quan trọng trong lập trình cơ sở dữ liệu. Nó giúp đảm bảo tính nhất quán và toàn vẹn của dữ liệu trong các hệ thống đa người dùng.
//Việc hiểu rõ các mức IsolationLevel khác nhau và cách sử dụng chúng là rất cần thiết để thiết kế các ứng dụng cơ sở dữ liệu hiệu quả và đáng tin cậy.