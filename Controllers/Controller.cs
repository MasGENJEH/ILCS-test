using Microsoft.AspNetCore.Mvc;

namespace ILCS_Test.Controllers;

[ApiController]
[Route("[controller]")]
public class BeaCukaiController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> HitungBeaMasuk(BeaCukaiModel model)
    {
        if (!ModelState.IsValid)
        {
            
            return View(model);
        }

        
        var hsCode = model.HsCode;
        var quantity = model.Quantity;
        var harga = model.Harga;
        var negara = model.Negara;

       
        using var client = new HttpClient();
        var url = $"https://insw-dev.ilcs.co.id/n/tarif?hs_code={hsCode}";
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var tarif = JsonConvert.DeserializeObject<decimal>(data);

            
            var beaMasuk = tarif * quantity * harga;

            
            ViewData["BeaMasuk"] = beaMasuk;

            return View("Hasil");
        }
        else
        {
            
            return View("Error");
        }
    }
}


public class BeaCukaiModel
{
    public string HsCode { get; set; }
    public int Quantity { get; set; }
    public decimal Harga { get; set; }
    public string Negara { get; set; }
}


