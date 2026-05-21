using FactoryScheduler.Api.DTOs;
using FactoryScheduler.Api.Entities;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace FactoryScheduler.Api.Services
{
    public class OllamaSchedulerService
    {
        private readonly HttpClient _httpClient;

        public OllamaSchedulerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AiSchedulingResultDto?> SelectMachineAsync(List<Machine> machines, AssignJobDto dto)
        {
            var payload = new
            {
                Job = new
                {
                    dto.JobName,
                    dto.Load,
                    dto.WorkMinutes,
                    Priority = dto.Priority.ToString()
                },
                Machines = machines.Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.MaxLoad,
                    m.CurrentLoad,
                    m.WorkMinutes,
                    m.IsRunning,
                    m.Ratio
                })
            };
            var prompt =
                $$$"""
                You are an AI factory scheduler.

                Choose the BEST machine.

                Return JSON only:
                {"machineId":1,"message":"reason"}
                
                Input:
                {JsonSerializer.Serialize(payload)}
                """;
            var request = new
            {
                model = "phi4-mini",
                prompt,
                stream = false,
                temperature = 0
            };
            var response = await _httpClient.PostAsJsonAsync("http://localhost:11434/api/generate", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
            if (result == null)
                return null;
            var content = ClearJson(result.Response);
            if (content == null) 
            {
                Console.WriteLine("Failed to extract JSON from response: " + result.Response);
                return null;
            }
            return JsonSerializer.Deserialize<AiSchedulingResultDto>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        private class OllamaResponse
        {
            public string Response { get; set; } = "";
        }
        private static string? ClearJson(string text)
        {
            var start = text.IndexOf('{');
            var end = text.LastIndexOf('}');
            if (start<0 || end<0 || end <= start)
            {
                return "null";
            }
            return text.Substring(start, end - start + 1);
        }
    }
}
