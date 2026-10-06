using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetsController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var assets = await _assetService.GetAllAsync();

        return Ok(assets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var asset = await _assetService.GetByIdAsync(id);

        if (asset == null)
            return NotFound(new
            {
                message = "Asset not found"
            });

        return Ok(asset);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] Asset asset)
    {
        var createdAsset = await _assetService.CreateAsync(asset);

        return Ok(createdAsset);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        await _assetService.DeleteAsync(id);

        return Ok(new
        {
            message = "Asset deleted successfully"
        });
    }
}