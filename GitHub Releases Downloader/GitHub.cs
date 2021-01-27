using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GitHubReleasesDownloader
{
    public class Release
    {
        public string Tag;
    }

    public class GitHub
    {
        static string pathToGh;

        public async void Login()
        {
            var response = await Run("auth login --web");
            // TODO ワンタイムコードを切り出して、ブラウザに飛ばす。
            // TODO 標準出力を監視して、Authentication Complete を検知したら表示を変えたり。
        }

        /// <summary>
        /// Get information from latest release.
        /// </summary>
        /// <param name="repo"></param>
        /// <returns></returns>
        public async Task<Release> GetLatestRelease(string repo)
        {
            var tag = await Run($"release view --repo {repo}"); // TODO 1行目を取得
            return new Release { Tag = tag };
        }

        /// <summary>
        /// Download assets from latest release.
        /// </summary>
        /// <param name="repo">owner/repo</param>
        /// <param name="pattern">*.exe</param>
        public async void DownloadLatest(string repo, string pattern)
        {
            var response = await Run($"release download --pattern \"{pattern}\" --repo {repo}");
        }

        static async Task<string> Run(string arguments, Action<string, Process> action = null)
        {
            await EnsureGHExist();
            var startInfo = new ProcessStartInfo
            {
                FileName = pathToGh,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var output = "";

            using (var process = new Process())
            {
                process.EnableRaisingEvents = true;
                process.StartInfo = startInfo;
                process.OutputDataReceived += (sender, args) => OnDataReceived(args.Data, process, ref output, action);
                process.ErrorDataReceived += (sender, args) => OnDataReceived(args.Data, process, ref output, action);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                return output;
            }
        }

        static async Task EnsureGHExist()
        {
            pathToGh = await Resource.Extract("gh.exe");
        }

        static void OnDataReceived(string message, Process process, ref string output, Action<string, Process> onReceived)
        {
            if (string.IsNullOrEmpty(message)) return;
            if (string.IsNullOrWhiteSpace(message)) return;
            if (message.StartsWith("*")) return;
            output += $"{message}\n";

            onReceived?.Invoke(message, process);
        }
    }
}
