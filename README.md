TeaRecall Manager
=================

業務向け WPF (.NET 8) アプリの初期実装です。

実装済み:
- プロジェクトファイル (.csproj)
- EF Core + SQLite の DbContext とシード
- エンティティ定義 (TeaLot / ProcessEvent / Shipment)
- ロット一覧画面 (検索、色付きUIの土台)

実行:
1. dotnet restore
2. dotnet build
3. dotnet run --project TeaRecallManager.csproj

