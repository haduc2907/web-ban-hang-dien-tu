using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public enum EOrderStatus
    {
        PendingConfirmation, // Chờ xác nhận
        ToBeShipped,         // Chờ giao hàng
        Delivered,           // Đã giao hàng
        Reviewed,            // Đã đánh giá
        Cancelled            // Đã hủy
    }
}
