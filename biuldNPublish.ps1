# Script para build e criação do pacote NuGet do BlazMapper

param(
    [string]$Configuration = "Release",
    [string]$Version = "",
    [switch]$Push = $false,
    [string]$ApiKey = "",
    [string]$Source = "https://api.nuget.org/v3/index.json"
)

# ⚠️ SEGURANÇA: Nunca commite API Keys no repositório público!

Write-Host "🚀 Iniciando build do BlazMapper..." -ForegroundColor Green

# Limpar builds anteriores
Write-Host "🧹 Limpando builds anteriores..." -ForegroundColor Yellow
dotnet clean --configuration $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Error "Falha ao limpar o projeto"
    exit 1
}

# Restaurar dependências
Write-Host "📦 Restaurando dependências..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "Falha ao restaurar dependências"
    exit 1
}

# Build do projeto
Write-Host "🔨 Fazendo build do projeto..." -ForegroundColor Yellow
dotnet build --configuration $Configuration --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "Falha no build do projeto"
    exit 1
}

# Executar testes (se existirem)
if (Test-Path "*.Tests") {
    Write-Host "🧪 Executando testes..." -ForegroundColor Yellow
    dotnet test --configuration $Configuration --no-build --verbosity normal
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Testes falharam"
        exit 1
    }
}

# Criar pacote NuGet
Write-Host "📦 Criando pacote NuGet..." -ForegroundColor Yellow

$packCommand = "dotnet pack --configuration $Configuration --no-build --output ./nupkg"

if ($Version -ne "") {
    $packCommand += " --version-suffix $Version"
    Write-Host "📌 Usando versão: $Version" -ForegroundColor Cyan
}

Invoke-Expression $packCommand
if ($LASTEXITCODE -ne 0) {
    Write-Error "Falha ao criar o pacote NuGet"
    exit 1
}

# Listar pacotes criados
Write-Host "📋 Pacotes criados:" -ForegroundColor Green
Get-ChildItem -Path "./nupkg" -Filter "*.nupkg" | ForEach-Object {
    Write-Host "  - $($_.Name)" -ForegroundColor Cyan
}

# Push para NuGet (opcional)
if ($Push) {
    if ($ApiKey -eq "") {
        # Tenta pegar da variável de ambiente primeiro
        $ApiKey = $env:NUGET_API_KEY
        
        if ($ApiKey -eq "") {
            Write-Error "API Key é necessária para push. Use uma das opções:"
            Write-Host "  1. .\build-and-pack.ps1 -Push -ApiKey 'sua-api-key'" -ForegroundColor Yellow
            Write-Host "  2. Set \$env:NUGET_API_KEY='sua-api-key'" -ForegroundColor Yellow
            exit 1
        }
    }
    
    Write-Host "🚀 Fazendo push para NuGet..." -ForegroundColor Yellow
    
    Get-ChildItem -Path "./nupkg" -Filter "*.nupkg" | ForEach-Object {
        Write-Host "📤 Enviando $($_.Name)..." -ForegroundColor Cyan
        dotnet nuget push $_.FullName --api-key $ApiKey --source $Source
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Falha ao enviar $($_.Name)"
            exit 1
        }
    }
    
    Write-Host "✅ Pacote enviado com sucesso!" -ForegroundColor Green
} else {
    Write-Host "💡 Para enviar para NuGet, use: .\build-and-pack.ps1 -Push -ApiKey 'sua-api-key'" -ForegroundColor Yellow
}

Write-Host "✅ Build concluído com sucesso!" -ForegroundColor Green
