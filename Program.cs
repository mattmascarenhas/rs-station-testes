using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class Program {
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string token = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJodHRwczovL2FwaS5yZC5zZXJ2aWNlcyIsInN1YiI6Inp5dkhPY1QzLXJwVjNacjBwQjZnVzdmQjdWZkcyQVVLaTVyVnNzY2tmY2NAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vYXBwLnJkc3RhdGlvbi5jb20uYnIvYXBpL3YyLyIsImFwcF9uYW1lIjoiaW50ZWdyYWNhby1ib2xkLXJkc3RhdGlvbiIsImV4cCI6MTY5MjcxMjA2MywiaWF0IjoxNjkyNjI1NjYzLCJzY29wZSI6IiJ9.tW-wlqhIeH78G8FOK8YkRSJUktV-7H7ir50Me_ZsgFfHKLp-0X-eqCnR1jniW0FMe3Vi-V86HizktootkkCaR59S5zd6O40qeynf9r9mS7YwnX0bf0De611NArS5cGWH8tyBa-4y-VbavY2Sl3JT0DdxdVEh99WveCzbN4K_eYOwWLqQDSjwrjcSzQhuHIC_NJwErNyqHsB8LXgmMKy9Rmrw3IJehuIloziRbXVqJZPNYAJqDvVMHJFtG9cyua0Pz9HCB3E0Z5HsNAJxzypCrAiyVdQw9TO8G6cqyC9poOlsdYRltQPNObU0y5sjJ2Ns-l9mLnLK7KieozN6k4nyFg";

    public static async Task Main(string[] args) {
        await Program.CriarContato(
            "Novo Contato teste 7",
            "testenovocontato7@teste.com",
            "Desenvolvedor",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            "www.example.com",
            "+55 11 9876-5432",
            "+55 11 1234-5678",
            "Cidade",
            "Estado",
            "País",
            new string[] { "tag1", "tag2" }
        );
    }

    public async static Task CriarContato(string nome, string email, string cargo, string bio, string website, string telefonePessoal, string telefoneCelular, string cidade, string estado, string pais, string[] tags) {
        string apiUrl = "https://api.rd.services/platform/contacts";
        try {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization")) {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            string verificarContatoUrl = "https://us-central1-stable-glass-391813.cloudfunctions.net/rd-station-verfiricar-contato";

            var verificaContatoContent = new StringContent(email, Encoding.UTF8, "text/plain");

            HttpResponseMessage respostaVerificacao = await _httpClient.PostAsync(verificarContatoUrl, verificaContatoContent);

            if (respostaVerificacao.IsSuccessStatusCode) {
                string responseBody = await respostaVerificacao.Content.ReadAsStringAsync();
                if (responseBody == "Contato existente") {
                    Console.WriteLine("O email do contato já existe. Contato não foi criado.");
                    return;
                }
            } else if (respostaVerificacao.StatusCode != System.Net.HttpStatusCode.NotFound) {
                Console.WriteLine("Erro ao verificar o contato. Status: " + respostaVerificacao.StatusCode);
                Console.WriteLine("Conteúdo da Resposta: " + await respostaVerificacao.Content.ReadAsStringAsync());
                return;
            }

            var novoContato = new {
                name = nome,
                email = email,
                job_title = cargo,
                bio = bio,
                website = website,
                personal_phone = telefonePessoal,
                mobile_phone = telefoneCelular,
                city = cidade,
                state = estado,
                country = pais,
                tags = tags
            };

            var jsonContato = JsonConvert.SerializeObject(novoContato);
            var content = new StringContent(jsonContato, Encoding.UTF8, "application/json");

            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpRequestMessage request = new HttpRequestMessage {
                Content = content,
                Method = HttpMethod.Post,
                RequestUri = new Uri(apiUrl)
            };

            HttpResponseMessage resposta = await _httpClient.SendAsync(request);

            if (resposta.IsSuccessStatusCode) {
                Console.WriteLine("Contato criado com sucesso.");
            } else {
                Console.WriteLine("O email do contato já existe. Contato não foi criado.");

            }
        } catch (HttpRequestException ex) {
            Console.WriteLine("Erro ao fazer a solicitação à API: " + ex.Message);
        } catch (Exception ex) {
            Console.WriteLine("Erro inesperado: " + ex.Message);
        }
    }
}
