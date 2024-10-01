using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System;
using Fasterflect;

namespace HomemadeCakes.Common.Http
{
    public class PJStreamReader : MessageBody
    {
        private int readIndex;

        public PJStreamReader(MessageBody msgBody)
            : base(msgBody)
        {
        }

        public PJStreamReader(MessageBase msgBase)
            : base(msgBase)
        {
        }

        public T ReadObject<T>()
        {
            object obj = null;
            if (base.MsgBodyData?.Count > readIndex)
            {
                Type type = typeof(T);
                if (base.MsgBodyData[readIndex] != null)
                {
                    object obj2 = base.MsgBodyData[readIndex];
                    Type type2 = obj2.GetType();
                    if (type2.ToString() == "System.Collections.Generic.Dictionary`2[System.String,System.Object]" && type.ToString() == "System.Object")
                    {
                        return (T)obj2;
                    }

                    if (type2 == type)
                    {
                        if (type == typeof(DateTime))
                        {
                            DateTime dateTime = (DateTime)obj2;
                            obj = ((dateTime.Kind != DateTimeKind.Utc) ? ((object)dateTime) : ((object)dateTime.ToLocalTime()));
                        }
                        else
                        {
                            obj = obj2;
                        }
                    }
                    else
                    {
                        string text = obj2.ToString();
                        if (!type.IsArray && type.Namespace == "System")
                        {
                            if (type.IsGenericType)
                            {
                                type = Nullable.GetUnderlyingType(type);
                            }

                            obj = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(text);
                            if (type == typeof(DateTime))
                            {
                                DateTime dateTime2 = (DateTime)obj;
                                if (dateTime2.Kind == DateTimeKind.Utc)
                                {
                                    obj = dateTime2.ToLocalTime();
                                }
                            }
                        }
                        else
                        {
                            obj = PJJsonHelper.Deserializer<T>(text);
                        }
                    }
                }

                readIndex++;
            }

            if (obj != null)
            {
                return (T)obj;
            }

            return default(T);
        }

        public object ReadObject(Type type)
        {
            return this.CallMethod(new Type[1] { type }, "ReadObject");
        }

        public string ReadString()
        {
            return ReadObject<string>();
        }

        public DateTime ReadDateTime()
        {
            return ReadObject<DateTime>();
        }

        public DateTime? ReadDateTimeNullable()
        {
            return ReadObject<DateTime?>();
        }

        public Guid ReadGuid()
        {
            return ReadObject<Guid>();
        }

        public Guid? ReadGuidNullable()
        {
            return ReadObject<Guid?>();
        }

        public bool ReadBoolean()
        {
            return ReadObject<bool>();
        }

        public bool? ReadBooleanNullable()
        {
            return ReadObject<bool?>();
        }

        public short ReadInt16()
        {
            return ReadObject<short>();
        }

        public short? ReadInt16Nullable()
        {
            return ReadObject<short?>();
        }

        public int ReadInt32()
        {
            return ReadObject<int>();
        }

        public int? ReadInt32Nullable()
        {
            return ReadObject<int?>();
        }

        public long ReadInt64()
        {
            return ReadObject<long>();
        }

        public long? ReadInt64Nullable()
        {
            return ReadObject<long?>();
        }

        public ushort ReadUInt16()
        {
            return ReadObject<ushort>();
        }

        public ushort? ReadUInt16Nullable()
        {
            return ReadObject<ushort?>();
        }

        public uint ReadUInt32()
        {
            return ReadObject<uint>();
        }

        public uint? ReadUInt32Nullable()
        {
            return ReadObject<uint?>();
        }

        public ulong ReadUInt64()
        {
            return ReadObject<ulong>();
        }

        public ulong? ReadUInt64Nullable()
        {
            return ReadObject<ulong?>();
        }

        public byte ReadByte()
        {
            return ReadObject<byte>();
        }

        public byte? ReadByteNullable()
        {
            return ReadObject<byte?>();
        }

        public sbyte ReadSByte()
        {
            return ReadObject<sbyte>();
        }

        public sbyte? ReadSByteNullable()
        {
            return ReadObject<sbyte?>();
        }

        public char ReadChar()
        {
            return ReadObject<char>();
        }

        public char? ReadCharNullable()
        {
            return ReadObject<char?>();
        }

        public float ReadSingle()
        {
            return ReadObject<float>();
        }

        public float? ReadSingleNullable()
        {
            return ReadObject<float?>();
        }

        public double ReadDouble()
        {
            return ReadObject<double>();
        }

        public double? ReadDoubleNullable()
        {
            return ReadObject<double?>();
        }

        public decimal ReadDecimal()
        {
            return ReadObject<decimal>();
        }

        public decimal? ReadDecimalNullable()
        {
            return ReadObject<decimal?>();
        }

        public byte[] ReadBytes()
        {
            return ReadObject<byte[]>();
        }

        public char[] ReadChars()
        {
            return ReadObject<char[]>();
        }

        public void Close()
        {
        }
    }
}
