using System;

namespace ThinkSharp.ObjectStorage.Helper
{
    internal static class ArgumentValidations
    {
        public static ValidationContext<TArg> Ensure<TArg>(
            this TArg arg,
            string memberName)
        {
            return new ValidationContext<TArg>()
            {
                Argument = arg,
                MemberName = memberName
            };
        }

        public static void IsNotNull<TArg>(this ValidationContext<TArg> context)
        {
            if (context.Argument == null)
                throw new ArgumentNullException(context.MemberName);
        }

        public static void IsOfType<TRequiredType>(this ValidationContext context)
        {
            if (!(context.ArgumentObj is TRequiredType))
                throw new ArgumentException("'" + context.MemberName + "' must be of type '" + typeof(TRequiredType) + "'");
        }

        public static void IsNotNullOrEmpty(this ValidationContext<string> context)
        {
            if (string.IsNullOrEmpty(context.Argument))
                throw new ArgumentException("'" + context.MemberName + "' must not null or empty");
        }

        public static void IsNotNullOrWhiteSpace(this ValidationContext<string> context)
        {
            if (string.IsNullOrWhiteSpace(context.Argument))
                throw new ArgumentException("'" + context.MemberName + "' must not null or white space");
        }
    }

    internal abstract class ValidationContext
    {
        public abstract Object ArgumentObj { get; }
        public string MemberName { get; internal set; }
    }

    internal class ValidationContext<TArg> : ValidationContext
    {
        public override object ArgumentObj { get { return Argument; } }

        public TArg Argument { get; internal set; }
    }
}
