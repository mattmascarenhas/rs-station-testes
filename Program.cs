using System;
using System.Net.Http;
using System.Threading.Tasks;

public class Program {
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task Main(string[] args) {
        string email = "teste@teste.com";  // Email inválido de propósito para simular a situação de e-mail não cadastrado
        string apiUrl = $"https://api.rd.services/platform/contacts/email:{email}";
        string token = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJodHRwczovL2FwaS5yZC5zZXJ2aWNlcyIsInN1YiI6Inp5dkhPY1QzLXJwVjNacjBwQjZnVzdmQjdWZkcyQVVLaTVyVnNzY2tmY2NAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vYXBwLnJkc3RhdGlvbi5jb20uYnIvYXBpL3YyLyIsImFwcF9uYW1lIjoiaW50ZWdyYWNhby1ib2xkLXJkc3RhdGlvbiIsImV4cCI6MTY5MjcxMjA2MywiaWF0IjoxNjkyNjI1NjYzLCJzY29wZSI6IiJ9.tW-wlqhIeH78G8FOK8YkRSJUktV-7H7ir50Me_ZsgFfHKLp-0X-eqCnR1jniW0FMe3Vi-V86HizktootkkCaR59S5zd6O40qeynf9r9mS7YwnX0bf0De611NArS5cGWH8tyBa-4y-VbavY2Sl3JT0DdxdVEh99WveCzbN4K_eYOwWLqQDSjwrjcSzQhuHIC_NJwErNyqHsB8LXgmMKy9Rmrw3IJehuIloziRbXVqJZPNYAJqDvVMHJFtG9cyua0Pz9HCB3E0Z5HsNAJxzypCrAiyVdQw9TO8G6cqyC9poOlsdYRltQPNObU0y5sjJ2Ns-l9mLnLK7KieozN6k4nyFg";

        try {
            // Definição do token Bearer
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);

            // solicitação à API
            HttpResponseMessage resposta = await _httpClient.GetAsync(apiUrl);

            if (resposta.StatusCode == System.Net.HttpStatusCode.NotFound) {
                Console.WriteLine("O email do cliente não está cadastrado.");
                return;
            }

            resposta.EnsureSuccessStatusCode();

            // Leia e analise a resposta
            string corpoResposta = await resposta.Content.ReadAsStringAsync();

            // Exiba a resposta no console
            Console.WriteLine(corpoResposta);
        } catch (HttpRequestException ex) {
            Console.WriteLine("Erro ao fazer a solicitação à API: " + ex.Message);
        } catch (Exception ex) {
            Console.WriteLine("Erro inesperado: " + ex.Message);
        }
    }
}
