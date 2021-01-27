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

        static async Task EnsureGHExist()
        {
            pathToGh = await Resource.Extract("gh.exe");
        }

        public void Login()
        {
            var response = Run("auth login --web");
            // TODO ワンタイムコードを切り出して、ブラウザに飛ばす。
            // TODO 標準出力を監視して、Authentication Complete を検知したら表示を変えたり。
        }

        /// <summary>
        /// Get information from latest release.
        /// </summary>
        /// <param name="repo"></param>
        /// <returns></returns>
        public Release GetLatestRelease(string repo)
        {
            var tag = Run($"release view --repo {repo}"); // TODO 1行目を取得
            return new Release { Tag = tag };
        }

        /// <summary>
        /// Download assets from latest release.
        /// </summary>
        /// <param name="repo">owner/repo</param>
        /// <param name="pattern">*.exe</param>
        public void DownloadLatest(string repo, string pattern)
        {
            Run($"release download --pattern \"{pattern}\" --repo {repo}");
        }

        string Run(string arguments)
        {
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
            await EnsureDaemonRunning();

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
    }
}
