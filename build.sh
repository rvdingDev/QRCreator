#!/bin/bash
# QRCreator Build Script (macOS)
# Usage: ./build.sh [version]
# Builds both x64 (Intel) and arm64 (Apple Silicon)

set -e

VERSION="${1:-1.0.1}"
PROJECT_DIR="QRCreator.Avalonia"
PROJECT_FILE="$PROJECT_DIR/QRCreator.Avalonia.csproj"
PACK_ID="QRCreator"
ICON_PATH="$PROJECT_DIR/Assets/qrcreator_icon.ico"
OUTPUT_DIR="./releases/mac"

echo "=== QRCreator Build (macOS) v$VERSION ==="

# Clean
rm -rf ./publish/osx-x64 ./publish/osx-arm64 "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

# Publish x64 (Intel)
echo ""
echo "[1/4] Publishing osx-x64..."
dotnet publish "$PROJECT_FILE" \
    -c Release \
    -r osx-x64 \
    --self-contained \
    -o ./publish/osx-x64 \
    -p:PublishSingleFile=false

# Publish arm64 (Apple Silicon)
echo ""
echo "[2/4] Publishing osx-arm64..."
dotnet publish "$PROJECT_FILE" \
    -c Release \
    -r osx-arm64 \
    --self-contained \
    -o ./publish/osx-arm64 \
    -p:PublishSingleFile=false

# Pack x64
echo ""
echo "[3/4] Packing osx-x64..."
vpk pack \
    -u "$PACK_ID" \
    -v "$VERSION" \
    -p ./publish/osx-x64 \
    -o "$OUTPUT_DIR" \
    -r osx-x64 \
    -e "QRCreator.Avalonia" \
    -i "$ICON_PATH" \
    --packTitle "QR Creator" \
    --packAuthors "rv.ding"

# Pack arm64
echo ""
echo "[4/4] Packing osx-arm64..."
vpk pack \
    -u "$PACK_ID" \
    -v "$VERSION" \
    -p ./publish/osx-arm64 \
    -o "$OUTPUT_DIR" \
    -r osx-arm64 \
    -e "QRCreator.Avalonia" \
    -i "$ICON_PATH" \
    --packTitle "QR Creator" \
    --packAuthors "rv.ding"

# Summary
echo ""
echo "=== Done! ==="
echo "Output: $OUTPUT_DIR"
ls -lh "$OUTPUT_DIR"
