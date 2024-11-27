using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

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
        public IActionResult MesclarArquivos(string folderPath)
        {
            string resultadoPath = Path.Combine(folderPath, "multifasta.fasta");

            try
            {
                MesclarArquivosFunc(folderPath);

                // Retorna o arquivo mesclado para download
                var fileBytes = System.IO.File.ReadAllBytes(resultadoPath);
                return File(fileBytes, "application/octet-stream", "multifasta.fasta");
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = $"Erro: {ex.Message}";
                return View();
            }
        }

        private void MesclarArquivosFunc(string pasta)
        {
            if (!Directory.Exists(pasta))
            {
                throw new DirectoryNotFoundException($"O caminho da pasta não existe: {pasta}");
            }

            // Obter arquivos .fasta, .fas e .ab1
            var arquivosFasta = Directory.GetFiles(pasta, "*.fasta");
            var arquivosFas = Directory.GetFiles(pasta, "*.fas");
            var arquivosAB1 = Directory.GetFiles(pasta, "*.ab1");

            // Combinar todos os arrays de arquivos
            var arquivos = arquivosFasta.Concat(arquivosFas).Concat(arquivosAB1).ToArray();

            if (arquivos.Length == 0)
            {
                throw new FileNotFoundException($"Nenhum arquivo FASTA (.fasta, .fas ou .ab1) encontrado na pasta especificada: {pasta}");
            }

            using (var writer = new StreamWriter(Path.Combine(pasta, "multifasta.fasta")))
            {
                foreach (var arquivo in arquivos)
                {
                    // Log para verificar os arquivos sendo processados
                    Console.WriteLine($"Processando arquivo: {arquivo}");
                    var conteudo = System.IO.File.ReadAllText(arquivo);
                    writer.WriteLine(conteudo);
                    writer.WriteLine(); // Adiciona uma linha em branco entre os arquivos
                }
            }
        }
    }
}