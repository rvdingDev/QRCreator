<div align="center">
  <img src="QRCreator.Avalonia/Assets/qrcreator_icon.ico" width="96" alt="QR Creator" />
  <h1>QR Creator</h1>
  <p>모양, 색상, 로고를 자유롭게 바꿀 수 있는 QR 코드 디자이너</p>

  <br>

  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt=".NET 10" />
  <img src="https://img.shields.io/badge/Avalonia-12-8B44AC?style=flat-square" alt="Avalonia 12" />
  <img src="https://img.shields.io/badge/SkiaSharp-3.x-0B6623?style=flat-square" alt="SkiaSharp" />
  <img src="https://img.shields.io/badge/license-MIT-green?style=flat-square" alt="MIT" />
</div>

<br>

## 뭘 할 수 있나요?

- **셀 모양 10가지** — 정사각형, 원, 둥근사각형, 다이아몬드, 하트, 별, 육각형, 물방울, 클로버, 팔각형
- **파인더 모양 독립 선택** — 셀과 다른 모양으로 조합 가능 (10 x 10 = 100가지)
- **전경/배경 색상** — 12색 프리셋 + HEX 입력
- **로고 삽입** — 중앙 배치, 투명 마스킹, 크기 조절
- **PNG 내보내기** — 1x / 2x / 4x 배율 선택
- **다크/라이트 테마** — 원클릭 전환

<br>

## 실행

```bash
dotnet run --project QRCreator.Avalonia/QRCreator.Avalonia.csproj
```

> 필요: [.NET 10 SDK](https://dotnet.microsoft.com/download)

<br>

## 다운로드

[GitHub Releases](../../releases/latest)에서 플랫폼별 설치 파일을 받을 수 있습니다.

| 플랫폼 | 아키텍처 |
|:---|:---|
| Windows | x64 |
| macOS | x64 (Intel) / ARM64 (Apple Silicon) |

설치 후 Velopack 자동 업데이트가 적용됩니다.

## 로컬 빌드

```powershell
# Windows
.\build.ps1 -Version "1.0.0"      # → releases/win/

# macOS (Intel + Apple Silicon)
./build.sh 1.0.0                   # → releases/mac/
```

> 필요: [Velopack CLI](https://velopack.io) (`dotnet tool install -g vpk`)

<br>

## 구조

```
QRCreator.Avalonia/
├── Models/           셀/파인더 모양 enum, 렌더링 옵션
├── Rendering/        QR 렌더러, SVG 패스 데이터, SkiaSharp 변환
├── ViewModels/       MVVM ViewModel (디바운스, 커맨드)
├── Converters/       값 변환기 (Color↔HEX, Enum↔bool)
├── MainWindow.axaml  2열 레이아웃 (옵션 + 미리보기)
└── AboutWindow.axaml 앱 정보 + 오픈소스 라이선스
```

<br>

## 오픈소스 라이브러리

| 이름 | 용도 | 라이선스 |
|:---|:---|:---|
| [Avalonia](https://github.com/AvaloniaUI/Avalonia) | 크로스플랫폼 UI | MIT |
| [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) | MVVM 소스 생성기 | MIT |
| [QRCoder](https://github.com/codebude/QRCoder) | QR 행렬 생성 | MIT |
| [SkiaSharp](https://github.com/mono/SkiaSharp) | 2D 렌더링 | MIT |
| [Velopack](https://github.com/velopack/velopack) | 배포/업데이트 | MIT |

전체 라이선스 원문은 [`licenses/`](licenses/) 폴더에 있습니다.

<br>

## 라이선스

MIT
