using '../templates/sqlServer_single_database.bicep'

// SQL Server の名前
param sqlServerName = 'sqlServerName'
// 管理者名 (Microsoft Entra ID のユーザー名)
param aadAdminName = 'admin@example.onmicrosoft.com'
// 管理者で指定したユーザーの SID
param aadAdminId = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
// 'sample' を指定した場合 AdventureWorkLT のサンプル データベースを作成
param dataSource = 'sample'
// リソースのデプロイ先
param location = 'westus2'
