# Convert traditional namespace to file-scoped namespace (改訂版)
# 従来の namespace { } 構文を namespace ; 構文に変換

$sourcePath = "D:\github.com\muzudho\Kifuwarabe_WCSC27\Sources"
$files = Get-ChildItem -Path $sourcePath -Filter "*.cs" -Recurse | Where-Object { 
	$_.FullName -notmatch '\\obj\\' -and 
	$_.FullName -notmatch 'AssemblyInfo\.cs$' -and
	$_.FullName -notmatch 'AssemblyAttributes\.cs$'
}

$convertedCount = 0
$skippedCount = 0

foreach ($file in $files) {
	$lines = Get-Content -Path $file.FullName -Encoding UTF8
	$newLines = New-Object System.Collections.ArrayList

	$inNamespace = $false
	$namespaceFound = $false
	$namespaceIndent = 0
	$namespaceLine = ""
	$usingLines = New-Object System.Collections.ArrayList
	$beforeNamespace = $true

	for ($i = 0; $i -lt $lines.Count; $i++) {
		$line = $lines[$i]

		# 名前空間宣言を検出
		if ($line -match '^\s*namespace\s+([^\s{]+)\s*$' -or $line -match '^\s*namespace\s+([^\s{]+)\s*\{\s*$') {
			if (-not $namespaceFound) {
				$namespaceName = $Matches[1]
				$namespaceLine = "namespace $namespaceName;"
				$namespaceFound = $true
				$inNamespace = $true
				$beforeNamespace = $false

				# 開き括弧が同じ行にあるかチェック
				if ($line -notmatch '\{\s*$') {
					# 次の行が開き括弧のはず
					$i++
				}

				# インデントレベルを検出
				if ($i + 1 -lt $lines.Count -and $lines[$i + 1] -match '^(\s+)') {
					$namespaceIndent = $Matches[1].Length
				}
				continue
			}
		}

		# namespace の前の using を収集
		if ($beforeNamespace -and $line -match '^\s*using\s+') {
			[void]$usingLines.Add($line)
			continue
		}

		# namespace の中の using を収集
		if ($inNamespace -and $line -match '^\s*using\s+' -and $usingLines.Count -eq 0) {
			# インデントを削除
			$cleanLine = $line -replace '^\s{' + $namespaceIndent + '}', ''
			[void]$usingLines.Add($cleanLine)
			continue
		}

		# 最後の閉じ括弧をスキップ
		if ($i -eq $lines.Count - 1 -and $line -match '^\s*\}\s*$') {
			continue
		}

		# 通常の行を追加（インデント調整）
		if ($inNamespace -and $namespaceIndent -gt 0) {
			# インデントを1段階削除
			if ($line -match '^(\s{' + $namespaceIndent + '})(.*)$') {
				[void]$newLines.Add($Matches[2])
			} else {
				[void]$newLines.Add($line)
			}
		} else {
			[void]$newLines.Add($line)
		}
	}

	if ($namespaceFound) {
		# 新しいファイル内容を構築
		$output = New-Object System.Collections.ArrayList

		# コメントや空行（namespace宣言の前）
		foreach ($line in $newLines) {
			if ($line -match '^\s*(//.*)$' -or $line -match '^\s*$') {
				[void]$output.Add($line)
			} else {
				break
			}
		}

		# namespace宣言
		[void]$output.Add($namespaceLine)

		# using statements
		foreach ($line in $usingLines) {
			[void]$output.Add($line)
		}

		# 残りのコンテンツ
		$contentStarted = $false
		foreach ($line in $newLines) {
			if (-not ($line -match '^\s*(//.*)$' -or $line -match '^\s*$')) {
				$contentStarted = $true
			}
			if ($contentStarted) {
				[void]$output.Add($line)
			}
		}

		# ファイルに書き込み
		$output | Set-Content -Path $file.FullName -Encoding UTF8

		Write-Host "✓ Converted: $($file.FullName)" -ForegroundColor Green
		$convertedCount++
	} else {
		Write-Host "⊘ Skipped (no namespace): $($file.FullName)" -ForegroundColor Yellow
		$skippedCount++
	}
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Conversion Summary:" -ForegroundColor Cyan
Write-Host "  Converted: $convertedCount files" -ForegroundColor Green
Write-Host "  Skipped:   $skippedCount files" -ForegroundColor Yellow
Write-Host "========================================`n" -ForegroundColor Cyan
