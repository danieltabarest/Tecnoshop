namespace Nop.Core.Domain.Pedidos
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum Pedidostatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 10,
        /// <summary>
        /// Processing
        /// </summary>
        Processing = 20,
        /// <summary>
        /// Complete
        /// </summary>
        Complete = 30,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 40
    }
}
