using System.Collections.Generic;
using System;

namespace HomemadeCakes.Common.Http
{
    public class PJStreamWriter : MessageBody
    {
        public void WriteObject(object obj)
        {
            if (base.MsgBodyData == null)
            {
                base.MsgBodyData = new List<object>();
            }

            base.MsgBodyData.Add(obj);
        }

        public void WriteObject<T>(T obj)
        {
            if (base.MsgBodyData == null)
            {
                base.MsgBodyData = new List<object>();
            }

            base.MsgBodyData.Add(obj);
        }

        public void Write(string value)
        {
            WriteObject(value);
        }

        public void Write(DateTime value)
        {
            WriteObject(value);
        }

        public void WriteNullable(DateTime? value)
        {
            WriteObject(value);
        }

        public void Write(Guid value)
        {
            WriteObject(value);
        }

        public void WriteNullable(Guid? value)
        {
            WriteObject(value);
        }

        public void Write(bool value)
        {
            WriteObject(value);
        }

        public void WriteNullable(bool? value)
        {
            WriteObject(value);
        }

        public void Write(short value)
        {
            WriteObject(value);
        }

        public void WriteNullable(short? value)
        {
            WriteObject(value);
        }

        public void Write(int value)
        {
            WriteObject(value);
        }

        public void WriteNullable(int? value)
        {
            WriteObject(value);
        }

        public void Write(long value)
        {
            WriteObject(value);
        }

        public void WriteNullable(long? value)
        {
            WriteObject(value);
        }

        public void Write(ushort value)
        {
            WriteObject(value);
        }

        public void WriteNullable(ushort? value)
        {
            WriteObject(value);
        }

        public void Write(uint value)
        {
            WriteObject(value);
        }

        public void WriteNullable(uint? value)
        {
            WriteObject(value);
        }

        public void Write(ulong value)
        {
            WriteObject(value);
        }

        public void WriteNullable(ulong? value)
        {
            WriteObject(value);
        }

        public void Write(byte value)
        {
            WriteObject(value);
        }

        public void WriteNullable(byte? value)
        {
            WriteObject(value);
        }

        public void Write(sbyte value)
        {
            WriteObject(value);
        }

        public void WriteNullable(sbyte? value)
        {
            WriteObject(value);
        }

        public void Write(char value)
        {
            WriteObject(value);
        }

        public void WriteNullable(char? value)
        {
            WriteObject(value);
        }

        public void Write(float value)
        {
            WriteObject(value);
        }

        public void WriteNullable(float? value)
        {
            WriteObject(value);
        }

        public void Write(double value)
        {
            WriteObject(value);
        }

        public void WriteNullable(double? value)
        {
            WriteObject(value);
        }

        public void Write(decimal value)
        {
            WriteObject(value);
        }

        public void WriteNullable(decimal? value)
        {
            WriteObject(value);
        }

        public void Write(byte[] value)
        {
            WriteObject(value);
        }

        public void Write(char[] value)
        {
            WriteObject(value);
        }

        public void Close()
        {
        }
    }

}
