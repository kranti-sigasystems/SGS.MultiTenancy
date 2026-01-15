namespace SGS.MultiTenancy.Core.Application.DTOs
{
    public class GenericRequestDto<T>
    {
        /// <summary>
        /// Optional ID for update/delete/get operations
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Object representing the entity to create or update
        /// </summary>
        public T? Data { get; set; }
    }

}
