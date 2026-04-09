using Avalonia;
using System;
using System.Diagnostics;
using Velopack;

namespace QRCreator.Avalonia;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        VelopackApp.Build()
            .OnFirstRun(v =>
            {
                // 첫 설치 후 실행 시
            })
            .OnAfterInstallFastCallback(v =>
            {
                // install 훅 완료 — Velopack이 Environment.Exit() 호출
            })
            .OnAfterUpdateFastCallback(v =>
            {
                // update 훅 완료
            })
            .OnBeforeUninstallFastCallback(v =>
            {
                // uninstall 전 정리
            })
            .Run();

        // Velopack 훅 처리 중이면 Run()에서 Environment.Exit() 호출됨
        // 아래 코드는 일반 실행 시에만 도달
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
