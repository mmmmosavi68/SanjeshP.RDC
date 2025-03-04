namespace SanjeshP.RDC.Entities.Common
{
    public interface IEntity
    {
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }

    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // زمان ایجاد
        //public DateTime? UpdatedAt { get; set; } // زمان بروزرسانی
        //public bool IsActive { get; set; } = true; // وضعیت فعال بودن
        //public bool IsDeleted { get; set; } = false; // وضعیت حذف
    }

    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
