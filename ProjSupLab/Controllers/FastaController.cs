using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ProjSupLab.Controllers
{
    public class FastaController : Controller
    {
        [HttpGet]
        public IActionResult MesclarArquivos()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MesclarArquivos(IFormFileCollection files)
        {
            string resultadoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "multifasta.fasta");

            try
            {
                if (files != null && files.Count > 0)
                {
                    // Chama a função para mesclar os arquivos
                    MesclarArquivosFunc(files, resultadoPath);

                    // Define a mensagem de sucesso
                    ViewBag.Mensagem = "Arquivos mesclados com sucesso!";

                    // Passando a URL do arquivo para a View para gerar o link de download
                    ViewBag.FilePath = "/uploads/multifasta.fasta";
                }
                else
                {
                    ViewBag.Mensagem = "Nenhum arquivo selecionado!";
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = $"Erro: {ex.Message}";
                return View();
            }
        }

        private void MesclarArquivosFunc(IFormFileCollection files, string resultadoPath)
        {
            // Cria ou sobrescreve o arquivo de saída
            using (var writer = new StreamWriter(resultadoPath))
            {
                foreach (var file in files)
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        writer.WriteLine(reader.ReadToEnd());
                        writer.WriteLine(); // Adiciona uma linha em branco entre os arquivos
                    }
                }
            }
        }
        [HttpGet]
        public IActionResult DownloadArquivo()
        {
            string uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            string arquivoPath = Path.Combine(uploadsDir, "multifasta.fasta");

            // Verifica se o arquivo existe
            if (!System.IO.File.Exists(arquivoPath))
            {
                return NotFound("Arquivo não encontrado.");
            }

            // Lê o arquivo para envio
            var fileBytes = System.IO.File.ReadAllBytes(arquivoPath);

            // Exclui o arquivo mesclado e limpa os arquivos de upload
            try
            {
                // Remove o arquivo mesclado
                System.IO.File.Delete(arquivoPath);

                // Limpa outros arquivos do diretório
                foreach (var file in Directory.GetFiles(uploadsDir))
                {
                    System.IO.File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao limpar arquivos: {ex.Message}");
            }

            // Retorna o arquivo para o cliente e inicia o download
            return File(fileBytes, "application/octet-stream", "multifasta.fasta");
        }
    }
}
