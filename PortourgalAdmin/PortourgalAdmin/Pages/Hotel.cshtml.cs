using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PortourgalAdmin.Model;

namespace PortourgalAdmin.Pages
{
    public class HotelModel : PageModel
    {
        public void OnGet(string nome, string cid, string ascii)
        {
            Distrito d = GetDistrito(ascii).Result;
            Ascii = ascii;
            Cid = cid;
            Hotel = d.Cidades.FirstOrDefault(x => x.Nome == cid).Hoteis.FirstOrDefault(h => h.Nome == nome);
        }

        public IActionResult OnPostDelete(string asciiname, string cid, string nome)
        {
            Distrito d = GetDistrito(asciiname).Result;
            DeleteHotel(d, cid, nome);
            return new RedirectToPageResult("/Cidade", new { dascii = asciiname, nome = cid });
        }

        public async void DeleteHotel(Distrito d, string cid, string nome)
        {
            Cidade c = d.Cidades.FirstOrDefault(x => x.Nome == cid);
            c.Hoteis.RemoveAll(a => a.Nome == nome);
            d.Cidades.RemoveAll(x => x.Nome == cid);
            d.Cidades.Add(c);
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(d), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://portourgalapi2020.azurewebsites.net/api/distritos/nome/" + d.ASCIIName, content);
        }

        public async Task<Distrito> GetDistrito(string ascii)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://portourgalapi2020.azurewebsites.net/api/distritos/nome/" + ascii).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                String json = await response.Content.ReadAsStringAsync();
                Distrito d = JsonConvert.DeserializeObject<Distrito>(json);
                return d;
            }
            else return null;
        }

        public Hotel Hotel { get; set; }
        public string Ascii { get; set; }
        public string Cid { get; set; }
    }
}
