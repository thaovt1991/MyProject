using HomemadeCakes.DAL.Interface;
using System.Collections.Generic;
using System;
using HomemadeCakes.Common.Interface;
using HomemadeCakes.DAL.Interface;
using Fasterflect;

namespace HomemadeCakes.DAL.Base
{
    public class DALContainerManager
    {
        public static bool IsDateTimeUTC = true;

        public static Type ContainerType = null;

        public static IDALContainer GetCurrentDALContainer()
        {
            PJRequestContext current = PJRequestContext.Current;
            IDALContainer result = null;
            if (current != null && current.RequestSession != null)
            {
                result = GetDALContainerForKey(current.RequestConnection);
            }

            return result;
        }

        public static IDALContainer GetDALContainerForKey(IConnection connetion)
        {
            string key = connetion.DBSource + "|" + connetion.DBName;
            PJRequestContext current = PJRequestContext.Current;
            IDALContainer iDALContainer = null;
            if (current != null)
            {
                if (current.DALContainerStorage != null)
                {
                    iDALContainer = current.DALContainerStorage.GetDALContainerForKey(key);
                }

                if (iDALContainer == null)
                {
                    iDALContainer = CreateDALContainer(connetion);
                    AddDALContainerToStore(key, iDALContainer);
                }
            }

            return iDALContainer;
        }

        public static void AddDALContainerToStore(string key, IDALContainer dalContainer)
        {
            PJRequestContext current = PJRequestContext.Current;
            if (current != null)
            {
                if (current.DALContainerStorage == null)
                {
                    current.DALContainerStorage = new DALContainerStorage();
                }

                current.DALContainerStorage.SetDALContainerForKey(key, dalContainer);
            }
        }

        public static IDALContainer CreateDALContainer(IConnection connetion, bool addToStore = true)
        {
            return ContractorDAL(connetion, addToStore);
        }

        public static IDALContainer CreateDALContainer<DALContainer>(IConnection connetion, bool addToStore = true) where DALContainer : IDALContainer
        {
            return ContractorDAL(connetion, addToStore, typeof(DALContainer));
        }

        private static IDALContainer ContractorDAL(IConnection connetion, bool addToStore, Type type = null, Type sysType = null)
        {
            if (type == null)
            {
                type = ContainerType;
            }

            if (type == null || connetion == null)
            {
                return null;
            }

            return (IDALContainer)type.CreateInstance(connetion, addToStore);
        }

        public static IEnumerable<IDALContainer> GetAllEFDALContainer()
        {
            PJRequestContext current = PJRequestContext.Current;
            IEnumerable<IDALContainer> result = null;
            if (current != null && current.DALContainerStorage != null)
            {
                result = current.DALContainerStorage.GetAllDALContainer();
            }

            return result;
        }
    }
}
/*
 * Fasterflect: Tăng tốc và đơn giản hóa Reflection trong .NET
Fasterflect là một thư viện .NET mạnh mẽ, được thiết kế để cung cấp một cách tiếp cận nhanh hơn và trực quan hơn đối với Reflection. Reflection là một tính năng quan trọng của .NET cho phép bạn kiểm tra và thao tác các thành phần của một đối tượng tại thời điểm chạy, nhưng nó thường chậm và phức tạp. Fasterflect giải quyết những hạn chế này bằng cách cung cấp các phương thức mở rộng trực quan và hiệu quả, giúp bạn làm việc với Reflection một cách dễ dàng hơn.

Tại sao nên sử dụng Fasterflect?
Hiệu suất cao: Fasterflect được tối ưu hóa để cung cấp hiệu suất vượt trội so với Reflection thông thường, giúp bạn thực hiện các thao tác Reflection một cách nhanh chóng.
Dễ sử dụng: Các phương thức mở rộng của Fasterflect rất trực quan và dễ hiểu, giúp bạn giảm thiểu lượng mã cần viết và tránh các lỗi phổ biến.
Linh hoạt: Fasterflect cung cấp một loạt các phương thức mở rộng cho phép bạn thực hiện nhiều loại thao tác khác nhau, từ việc truy cập các trường và thuộc tính đến việc gọi các phương thức và tạo các đối tượng mới.
Hỗ trợ đa dạng: Fasterflect hỗ trợ các phiên bản .NET khác nhau và có thể được sử dụng trong nhiều loại ứng dụng.
Các tính năng chính của Fasterflect
Truy cập các trường và thuộc tính: Đọc và ghi giá trị của các trường và thuộc tính một cách dễ dàng.
Gọi các phương thức: Gọi các phương thức với các tham số khác nhau.
Tạo các đối tượng mới: Tạo các đối tượng mới và khởi tạo các trường và thuộc tính của chúng.
Hỗ trợ các kiểu dữ liệu: Làm việc với các kiểu dữ liệu khác nhau, bao gồm cả các kiểu generic.
Dynamic code generation: Tạo mã động để tăng tốc độ thực thi.
Ví dụ
C#
using Fasterflect;

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Sử dụng Fasterflect để truy cập các trường và thuộc tính
var person = new Person { Name = "John Doe", Age = 30 };
var name = person.Get("Name"); // Lấy giá trị của thuộc tính Name
person.Set("Age", 35); // Thiết lập giá trị của thuộc tính Age

// Sử dụng Fasterflect để gọi một phương thức
var result = person.Call("ToString");
Hãy thận trọng khi sử dụng các đoạn mã.

So sánh với Reflection thông thường
Tính năng	Reflection thông thường	Fasterflect
Hiệu suất	Chậm	Nhanh
Dễ sử dụng	Khó	Dễ
Linh hoạt	Cao	Cao
 */
