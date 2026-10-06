using System.ComponentModel.DataAnnotations;

namespace UserApi.DTOs;

public class OrderRequestDto
{
    [Range(1, int.MaxValue)]
    public int AssetId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}