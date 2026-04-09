namespace QRCreator.Avalonia.Rendering;

public static class FluentIconPaths
{
    // Square - 완전 각짐
    public const string Square =
        "M0 0H24V24H0Z";

    // Circle
    public const string Circle =
        "M12 2A10 10 0 1 0 12 22A10 10 0 1 0 12 2Z";

    // RoundedSquare - Square와 동일 크기, rx만 추가
    public const string RoundedSquare =
        "M6 2H18C20.2 2 22 3.8 22 6V18C22 20.2 20.2 22 18 22H6C3.8 22 2 20.2 2 18V6C2 3.8 3.8 2 6 2Z";

    // Diamond → diamond_24_filled
    public const string Diamond =
        "M2.95 9.7a3.25 3.25 0 0 0 0 4.6l6.75 6.75a3.25 3.25 0 0 0 4.6 0l6.75-6.75a3.25 3.25 0 0 0 0-4.6L14.3 2.95a3.25 3.25 0 0 0-4.6 0L2.95 9.7Z";

    // Heart → heart_24_filled
    public const string Heart =
        "M12.82 5.58 12 6.4l-.82-.82a5.37 5.37 0 1 0-7.6 7.6l7.89 7.9c.3.29.77.29 1.06 0l7.9-7.9a5.38 5.38 0 1 0-7.61-7.6Z";

    // Star → star_24_filled
    public const string Star =
        "M10.79 3.1c.5-1 1.92-1 2.42 0l2.36 4.78 5.27.77c1.1.16 1.55 1.52.75 2.3l-3.82 3.72.9 5.25a1.35 1.35 0 0 1-1.96 1.42L12 18.86l-4.72 2.48a1.35 1.35 0 0 1-1.96-1.42l.9-5.25-3.81-3.72c-.8-.78-.36-2.14.75-2.3l5.27-.77 2.36-4.78Z";

    // Hexagon - 위아래 꼭짓점 기준
    public const string Hexagon =
        "M12 2L21.6 7V17L12 22L2.4 17V7Z";

    // Raindrop - 위 뾰족, 아래 둥글게
    public const string Raindrop =
        "M12 2C12 2 4 10 4 15A8 8 0 0 0 20 15C20 10 12 2 12 2Z";

    // Clover → clover_24_filled
    public const string Clover =
        "M6.75 2a4.75 4.75 0 0 0 0 9.5h4c.41 0 .75-.34.75-.75v-4A4.75 4.75 0 0 0 6.75 2Zm0 20a4.75 4.75 0 1 1 0-9.5h4c.41 0 .75.34.75.75v4A4.75 4.75 0 0 1 6.75 22Zm10.5-20a4.75 4.75 0 1 1 0 9.5h-4a.75.75 0 0 1-.75-.75v-4A4.75 4.75 0 0 1 17.25 2Zm0 20a4.75 4.75 0 1 0 0-9.5h-4a.75.75 0 0 0-.75.75v4A4.75 4.75 0 0 0 17.25 22Z";

    // Octagon → null, DrawOctagon()으로 직접 그리기
    public const string? Octagon = null;
}
