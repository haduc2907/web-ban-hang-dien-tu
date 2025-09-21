using Entities;
using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        public string Name { get; set; } = string.Empty;

        // Cho phép để trống mô tả
        public string? Description { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống!")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0!")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Ảnh sản phẩm không được để trống!")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá sản phẩm không được để trống!")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0!")]
        public decimal Price { get; set; }

        public EStatusProduct Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống!")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Thương hiệu không được để trống!")]
        public string Brand { get; set; } = string.Empty;
    }
}
