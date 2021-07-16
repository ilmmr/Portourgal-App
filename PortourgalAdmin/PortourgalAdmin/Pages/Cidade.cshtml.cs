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
    public class CidadeModel : PageModel
    {
        public void OnGet(string nome, string dascii)
        {
            Distrito = GetDistrito(dascii).Result;
            Cidade = Distrito.Cidades.FirstOrDefault(x => x.Nome == nome);
        }

        public IActionResult OnPostDelete(string asciiname, string nome)
        {
            Distrito d = GetDistrito(asciiname).Result;
            DeleteCidade(d,nome);
            return new RedirectToPageResult("/Distrito", new { ascii = asciiname });
        }

        public async void DeleteCidade(Distrito d, string nome)
        {
            d.Cidades.RemoveAll(c => c.Nome == nome);
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

        public Cidade Cidade { get; set; }
        public Distrito Distrito { get; set; }
    }
}
