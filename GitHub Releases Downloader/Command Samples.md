# 環境変数
GH_TOKEN, GITHUB_TOKEN (in order of precedence): an authentication token for github.com
API requests. Setting this avoids being prompted to authenticate and takes precedence over
previously stored credentials.


# ログイン手順
λ gh auth login --web

! First copy your one-time code: AABB-CC12
- Press Enter to open github.com in your browser... 
✓ Authentication complete. Press Enter to continue...

✓ Logged in as yutokun


# リスト
λ gh release list --repo yutokun/APK-Installer
安定性の改善 ☔                  Latest  (v1.4.1)  about 2 months ago
完了サウンド �                          (v1.4.0)  about 2 months ago
まっさらな環境でも動作 ✨                (v1.3.1)  about 2 months ago
ダブルクリックでインストール �          (v1.3.0)  about 2 months ago
安定化・使いやすさの改善 �              (v1.2.0)  about 2 months ago
バグ修正 �                              (v1.1.1)  about 3 months ago
複数デバイス対応 ��                    (v1.1.0)  about 3 months ago
初版リリース �                          (v1.0.0)  about 3 months ago


# exe を DL
λ gh release download --pattern "*.exe" --repo yutokun/APK-Installer


# 最新のリリース情報を取得
λ gh release view --repo yutokun/APK-Installer
v1.4.1
github-actions[bot] released this about 2 months ago


  • 別バージョンの ADB が動いていると落ちる問題を修正
  • 一時ファイルのクリーンアップを堅牢に
  • 複数起動時に起こる問題を修正


Assets
APKInstaller.exe  5.63 MiB

View on GitHub: https://github.com/yutokun/APK-Installer/releases/tag/v1.4.1


# リリースがない場合
λ gh release view --repo yutokun/GitHub-Actions-Unity-Build-Sample
HTTP 404: Not Found (https://api.github.com/repos/yutokun/GitHub-Actions-Unity-Build-Sample/releases/latest)