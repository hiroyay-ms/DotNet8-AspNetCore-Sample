using '../templates/functions_flex_consumption_with_managedIdentity.bicep'

// 新しく Log Analytics ワークスペースと Application Insights を作成する場合は 'new' を指定、既定のものを使用する場合は 'existing' を指定
param newOrExisting = 'new'
// Log Analytics ワークスペースの名前
param workspaceName = 'log-fy25q3-test-5a'
// Application Insights の名前
param appInsightsName ='appi-fy25q3-test-5a'
// 関数アプリへ割り当てるマネージド ID の種類 (userAssigned, systemAssigned) を指定
param identityType = 'systemAssigned'
// マネージド ID の名前
param managedIdentityName = 'uami-fy25q3-test-3a'
// ストレージ アカウントの名前
param storageAccountName = 'stfy25q3test3a'
// 関数アプリのプラン名
param planName = 'plan-fy25q3-test-3a'
// 関数アプリの名前
param functionName = 'func-fy25q3-test-3a'
// リソースのデプロイ先
param location = 'westus2'
