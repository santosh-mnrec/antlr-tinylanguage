namespace TinyLanguage
{
    using System;
    public class Variable
    {

        public static Variable VOID = new Variable(new object());

        private object value;

        public Variable(object v)
        {
            value = v;
        }

        public bool ToBoolean()
        {
            return bool.TryParse(value.ToString(), out bool result);
        }

        public int ToInteger()
        {
            return Convert.ToInt32(value);

        }

        public String ToStringV()
        {
            return value?.ToString();
        }

        public bool IsNumber()
        {
            return int.TryParse(value.ToString(), out int i);
        }
        //ToStringV
        public override string ToString()
        {
            return value?.ToString();

        }

        public override int GetHashCode()
        {
            return value?.GetHashCode() ?? 0;

        }


        public override bool Equals(object obj)
        {
            if (value == obj)
            {
                return true;
            }

            if (value == null || obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            var that = (Variable)obj;

            return this.value.Equals(that.value);
        }

        public bool IsString()
        {
            return value is String;
        }
    }
}