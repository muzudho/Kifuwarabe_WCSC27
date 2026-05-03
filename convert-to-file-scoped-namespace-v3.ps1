# Convert traditional namespace to file-scoped namespace (最終版)
# 従来の namespace { } 構文を namespace ; 構文に変換
# プリプロセッサディレクティブ (#define, #if など) を正しく処理

$sourcePath = "D:\github.com\muzudho\Kifuwarabe_WCSC27\Sources"
$files = Get-ChildItem -Path $sourcePath -Filter "*.cs" -Recurse | Where-Object { 
	$_.FullName -notmatch '\\obj\\' -and 
	$_.FullName -notmatch 'AssemblyInfo\.cs$' -and
	$_.FullName -notmatch 'AssemblyAttributes\.cs$'
}

$convertedCount = 0
$skippedCount = 0

foreach ($file in $files) {
	$content = Get-Content -Path $file.FullName -Raw -Encoding UTF8

	# プリプロセッサディレクティブを先頭に移動する必要があるかチェック
	# namespace の中に #define があるパターンを検出
	if ($content -match 'namespace\s+[^\s{]+\s*\{[^}]*#define') {
		Write-Host "⚠ Skipped (has #define inside namespace): $($file.FullName)" -ForegroundColor Yellow
		$skippedCount++
		continue
	}

	# 単純な正規表現でファイル全体を処理
	# パターン: namespace Name { ... }
	$pattern = '(?s)^(.*?)namespace\s+([^\s{]+)\s*\{(.*)\}\s*$'

	if ($content -match $pattern) {
		$beforeNamespace = $Matches[1]
		$namespaceName = $Matches[2]
		$namespaceContent = $Matches[3]

		# namespace 内容の先頭から using を抽出
		$usings = ""
		$remainingContent = $namespaceContent

		# using ステートメントを抽出（インデント付き）
		while ($remainingContent -match '^\s*(using\s+[^;]+;)\s*[\r\n]+(.*)$') {
			$usings += $Matches[1] + "`r`n"
			$remainingContent = $Matches[2]
		}

		# インデントを削除（4スペースまたは1タブ）
		$dedented = $remainingContent -replace '(?m)^    ', '' -replace '(?m)^\t', ''

		# 新しい内容を構築
		$newContent = $beforeNamespace
		$newContent += "namespace $namespaceName;`r`n"
		$newContent += $usings
		$newContent += $dedented

		# 末尾の余分な空白を整理
		$newContent = $newContent -replace '\r\n\r\n\r\n+', "`r`n`r`n"

		# ファイルに書き込み
		[System.IO.File]::WriteAllText($file.FullName, $newContent, [System.Text.Encoding]::UTF8)

		Write-Host "✓ Converted: $($file.FullName)" -ForegroundColor Green
		$convertedCount++
	} else {
		Write-Host "⊘ Skipped (pattern not matched): $($file.FullName)" -ForegroundColor Gray
		$skippedCount++
	}
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Conversion Summary:" -ForegroundColor Cyan
Write-Host "  Converted: $convertedCount files" -ForegroundColor Green
Write-Host "  Skipped:   $skippedCount files" -ForegroundColor Yellow
Write-Host "========================================`n" -ForegroundColor Cyan
