param newOrExisting string
param workspaceName string
param appInsightsName string
param identityType string
param managedIdentityName string
param storageAccountName string
param planName string
param functionName string
param location string

// newOrExsiting が 'new' の場合、Log Analytics と Application Insights を作成
module azureMonitor '../modules/monitor.bicep' = if (newOrExisting == 'new') {
  name: 'azureMonitor'
  params: {
    workspaceName: workspaceName
    appInsightsName: appInsightsName
    location: location
  }
}

// identityType が userAssigned の場合、マネージド ID を作成
module managedIdentity '../modules/managed_identity.bicep' = if (identityType == 'userAssigned') {
  name: 'managedIdentity'
  params: {
    managedIdentityName: managedIdentityName
    location: location
  }
}

// ストレージ アカウントの作成
module storageAccount '../modules/storage_account.bicep' = {
  name: 'storageAccount'
  params: {
    storageAccountName: storageAccountName
    workspaceName: workspaceName
    functionName: functionName
    location: location
  }
  dependsOn: [
    azureMonitor
  ]
}

// 関数アプリの作成 (ユーザー割り当てマネージド ID)
module functionFlexConsumption_with_UserAssigned_Id '../modules/functions_flex_consumption_userassigned_identity.bicep'= if (identityType == 'userAssigned') {
  name: 'functionFlexConsumption_with_UserAssigned_Id'
  params: {
    managedIdentityName: managedIdentityName
    storageAccountName: storageAccountName
    appInsightsName: appInsightsName
    planName: planName
    functionName: functionName
    location: location
  }
  dependsOn: [
    storageAccount
  ]
}

// 関数アプリの作成 (システム割り当てマネージド ID)
module functionFlexConsumption_with_SystemAssigned_Id '../modules/functions_flex_consumption_systemassigned_identity.bicep'= if (identityType == 'systemAssigned') {
  name: 'functionFlexConsumption_with_SystemAssigned_Id'
  params: {
    storageAccountName: storageAccountName
    appInsightsName: appInsightsName
    planName: planName
    functionName: functionName
    location: location
  }
  dependsOn: [
    storageAccount
  ]
}
