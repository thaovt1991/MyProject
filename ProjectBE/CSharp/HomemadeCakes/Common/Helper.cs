using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System;
using HomemadeCakes.Model.Common;
using HomemadeCakes.Model;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;

namespace HomemadeCakes.Common
{
    public sealed class Helper // hoi sealed
    {

        private const int PBKDF2_ITER_COUNT = 1000;

        private const int PBKDF2_SUBKEY_LENGTH = 32;

        private const int SALT_SIZE = 16;

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(32);
            }

            byte[] array = new byte[49];
            Buffer.BlockCopy(salt, 0, array, 1, 16);
            Buffer.BlockCopy(bytes, 0, array, 17, 32);
            return Convert.ToBase64String(array);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException("hashedPassword");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] array = Convert.FromBase64String(hashedPassword);
            if (array.Length != 49 || array[0] != 0)
            {
                return false;
            }

            byte[] array2 = new byte[16];
            Buffer.BlockCopy(array, 1, array2, 0, 16);
            byte[] array3 = new byte[32];
            Buffer.BlockCopy(array, 17, array3, 0, 32);
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array2, 1000))
            {
                bytes = rfc2898DeriveBytes.GetBytes(32);
            }

            return ByteArraysEqual(array3, bytes);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == b)
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool flag = true;
            for (int i = 0; i <= a.Length - 1; i++)
            {
                flag &= a[i] == b[i];
            }

            return flag;
        }

        //Invoker đến các asembly - Anh xa den business khasc
        public static async Task<object> InvokeMethodAsync(RequestBase request, User user = null)
        {
            try
            {
                var response = new ResponseBase<object>();
                var assemblyName = "HomemadeCakes";//request?.AssemblyName ?? "CakesManager"; //thuw vien
                var className = "HomemadeCakes.Business.UserLogBusiness";//request?.ClassName ?? "CakesManager.Business.CakesBusiness";    //;//Duog dan
                var assembly = Assembly.Load(new AssemblyName(assemblyName));

                var type = assembly.GetType(className);
                if (type == null)
                {
                    response.ErrorCode = "400";
                    response.Message = $"Class {className} not found in assembly {assemblyName}";
                    //Log.Instance.Error(response.Message);
                    return response;
                }

                request.MethodName = "LoginAsync";                //tesst 
                var method = type.GetMethod(request?.MethodName ?? "", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                if (method == null)
                {
                    response.ErrorCode = "400";
                    response.Message = $"Method {request?.MethodName ?? ""} not found in class {className}";
                    //  Log.Error(response.Message);
                    return response;
                }

                var parameters = request != null && request.Data != null ? new object[request.Data.Length] : new object[0];
                var parameterInfos = method.GetParameters(); //Lay kieru dữ liệu và param của methol
                //cover data to praram Type
                if (parameterInfos?.Length > 0 && request?.Data?.Length > 0)
                {
                    for (int i = 0; i < request?.Data?.Length; i++)
                    {
                        var parameterType = parameterInfos[i].ParameterType;

                        if (request.Data[i] is JsonElement jsonElement)
                        {
                            if (jsonElement.ValueKind == JsonValueKind.String)
                            {
                                parameters[i] = jsonElement.GetString() ?? string.Empty;
                            }
                            else if (jsonElement.ValueKind == JsonValueKind.Object)
                            {
                                parameters[i] = System.Text.Json.JsonSerializer.Deserialize(jsonElement.GetRawText(), parameterType) ?? string.Empty;
                            }
                        }
                        else
                        {
                            parameters[i] = request.Data[i];
                        }
                    }
                }


                var instance = method.IsStatic ? null : user != null ? Activator.CreateInstance(type, user) : Activator.CreateInstance(type);

                var result = method.Invoke(instance, parameters);
                response.Data = result ?? Array.Empty<object>();

                if (result is Task taskResult)
                {
                    await taskResult.ConfigureAwait(false);
                    return taskResult.GetType().GetProperty("Result")?.GetValue(taskResult) ?? response;
                }
                return response;
            }
            catch (InvalidOperationException ex)
            {
                var response = new ResponseBase<object>();
                response.ErrorCode = ex.HResult.ToString();
                response.Message = ex.Message;
                //Log.Instance.Info(ex);
                return response;
            }
            catch (Exception ex)
            {
                var response = new ResponseBase<string>();
                response.ErrorCode = ex.HResult.ToString();
                response.Message = ex.Message;
                // Log.Instance.Error(ex);
                return response;
            }

        }

        private string GetAutoNumberLogic(string refType)
        {
            var taskID = "TSK";
            if (string.IsNullOrEmpty(refType)) taskID += "TM";
            else
                taskID += refType.Substring(0, 2);
            taskID += DateTime.Now.ToString("yyMMddHHmmssfff");
            return taskID;
        }

        #region SetCookes  - Tham khao
        //public async Task Login()
        //{
        //var data = new { UserName = "admin", Password = "admin", DBName = "LVERP" };

        //ServicePointManager.Expect100Continue = true;
        //ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, error) => true;

        //using (var client = new HttpClient())
        //{
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        //    try
        //    {
        //        var response = await client.PostAsync("http://172.16.5.81:1112/MobileAPI/api/Auth/login", content);
        //        response.EnsureSuccessStatusCode();

        //        if (response == null || !response.IsSuccessStatusCode || response.Content == null) return; 
        ///    DOC COOKIE
        //        var cookies = response.Headers.GetValues("Set-Cookie");
        //        if (cookies != null && cookies.Count() > 0)
        //        {
        //            var tk = cookies.FirstOrDefault(x => x.Contains("lvtk"));
        //            if (tk != null)
        //            {
        //                var sIndx = tk.IndexOf(';');
        //                string s = tk.Substring(0, sIndx).Replace("lvtk=", "");
        //                token = Uri.UnescapeDataString(s);
        //            };
        //        }
        //        var responseJson = await response.Content.ReadAsStringAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance.Error(ex);
        //    }
        //}
        // }

        #endregion
    }

}
