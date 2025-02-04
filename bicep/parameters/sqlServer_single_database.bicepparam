using '../templates/sqlServer_single_database.bicep'

// 新しく SQL Server を作成する場合は 'new' を指定、既存のものを使用する場合は 'existing' を指定
// 既存の SQL Server を使用する場合は、展開先のリソース グループが SQL Server と同じになるように注意が必要
param newOrExistingServer = 'new'
// SQL Server の名前
param sqlServerName = 'sqlServerName'
// 管理者名 (Microsoft Entra ID のユーザー名)
param aadAdminName = 'admin@example.onmicrosoft.com'
// 管理者で指定したユーザーの SID
param aadAdminId = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
// 'sample' を指定した場合 Serverless レベルで AdventureWorkLT のサンプル データベースを作成、'new' の場合は空のデータベースを作成
// 'sample' の場合、Serverless レベルで最大 1GB 容量のデータベースを作成のため SKU の指定は不要
param dataSource = 'new'
// dataSouce が 'new' の場合、データベースの名前を指定
param databaseName = 'Pubs'
// SKU パラメーターに指定する値は az sql db list-editions -l <リージョン> -o table で確認可
// GeneralPurpose, BusinessCritical, Hyperscale などを指定
param tier = 'GeneralPurpose'
// SKU のサイズ、GP_S_Gen5, GP_Gen5, BC_Gen5, HS_S_Gen5 などを指定
param skuName = 'GP_S_Gen5'
// SKU に対するハードウェアの世代
param family = 'Gen5'
// SKU の容量
param capacity = 1
// データベースの最大サイズ (バイト)
param maxSizeBytes = 1073741824
// 照合順序の指定
// https://learn.microsoft.com/ja-jp/sql/relational-databases/collations/collation-and-unicode-support?view=sql-server-ver16
param collation = 'SQL_Latin1_General_CP1_CI_AS'
// バックアップを格納するストレージの冗長オプションを指定
param requestedBackupStorageRedundancy = 'Local'
// リソースのデプロイ先
param location = 'westus2'
