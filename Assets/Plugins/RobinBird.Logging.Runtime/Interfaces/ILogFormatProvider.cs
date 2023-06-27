namespace RobinBird.Logging.Runtime.Interfaces
{
    using JetBrains.Annotations;

    /// <summary>
    /// Log provider that receives logs with format information.
    /// </summary>
    public interface ILogFormatProvider : ILogProvider
    {
        [StringFormatMethod("format")]
        void InfoFormat([CanBeNull] object context, [NotNull] string format, [CanBeNull] object[] args, string category);


        [StringFormatMethod("format")]
        void WarnFormat([CanBeNull] object context, [NotNull] string format, [CanBeNull] object[] args, string category);


        [StringFormatMethod("format")]
        void ErrorFormat([CanBeNull] object context, [NotNull] string format, [CanBeNull] object[] args, string category);
    }
}