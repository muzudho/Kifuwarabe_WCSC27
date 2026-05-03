# Convert traditional namespace to file-scoped namespace
# 従来の namespace { } 構文を namespace ; 構文に変換

$sourcePath = "D:\github.com\muzudho\Kifuwarabe_WCSC27\Sources"
$files = Get-ChildItem -Path $sourcePath -Filter "*.cs" -Recurse

$convertedCount = 0
$skippedCount = 0

foreach ($file in $files) {
	$content = Get-Content -Path $file.FullName -Raw -Encoding UTF8
	$originalContent = $content

	# Pattern 1: using statements before namespace
	# Pattern 2: using statements inside namespace
	# 単一の名前空間定義で、全体が囲まれているパターンを検出

	# まず、名前空間の外に using があるパターンを試す
	$pattern1 = '(?s)^((?:using[^;]+;[\r\n]*)*)([\r\n]*)namespace\s+([^\s{]+)\s*\{(.*)\}[\r\n]*$'
	# 名前空間の中に using があるパターン
	$pattern2 = '(?s)^([\r\n]*)namespace\s+([^\s{]+)\s*\{[\r\n]*((?:\s*using[^;]+;[\r\n]*)*)(.*)\}[\r\n]*$'

	$matched = $false
	$usingStatements = ""
	$namespaceName = ""
	$namespaceBody = ""

	if ($content -match $pattern1) {
		$usingStatements = $Matches[1]
		$namespaceName = $Matches[3]
		$namespaceBody = $Matches[4]
		$matched = $true
	} elseif ($content -match $pattern2) {
		$usingStatements = $Matches[3]
		$namespaceName = $Matches[2]
		$namespaceBody = $Matches[4]
		$matched = $true
	}

	if ($matched) {

		# ネストされた名前空間がないかチェック
		if ($namespaceBody -notmatch 'namespace\s+[^\s{]+\s*\{') {
			# 名前空間の内容のインデントを1段階解除
			$lines = $namespaceBody -split "`r?`n"
			$dedentedLines = @()

			foreach ($line in $lines) {
				if ($line -match '^    (.*)$') {
					# 4スペースのインデントを削除
					$dedentedLines += $Matches[1]
				} elseif ($line -match '^\t(.*)$') {
					# タブのインデントを削除
					$dedentedLines += $Matches[1]
				} else {
					$dedentedLines += $line
				}
			}

			$dedentedBody = $dedentedLines -join "`r`n"

			# 新しい形式で再構築
			$newContent = "namespace $namespaceName;`r`n"
			if ($usingStatements.Trim()) {
				$newContent += $usingStatements
			}
			$newContent += $dedentedBody

			# ファイルに書き込み
			Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8 -NoNewline

			Write-Host "✓ Converted: $($file.FullName)" -ForegroundColor Green
			$convertedCount++
		} else {
			Write-Host "⊘ Skipped (nested namespace): $($file.FullName)" -ForegroundColor Yellow
			$skippedCount++
		}
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
