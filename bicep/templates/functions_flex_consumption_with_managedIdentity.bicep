param newOrExistingWorkspace string
param resourceGroup_workspace string
param workspaceName string
param appInsightsName string
param identityType string
param newOrExistingManagedIdentity string
param resourceGroup_id string
param managedIdentityName string
param storageAccountName string
param planName string
param functionName string
param location string

// newOrExsiting が 'new' の場合、Log Analytics と Application Insights を作成
module azureMonitor '../modules/monitor.bicep' = if (newOrExistingWorkspace == 'new') {
  name: 'azureMonitor'
  params: {
    workspaceName: workspaceName
    appInsightsName: appInsightsName
    location: location
  }
}

// identityType が 'userAssigned'、且つ newOrExistingManagedIdentity が 'new' の場合、マネージド ID を作成
module managedIdentity '../modules/managed_identity.bicep' = if (identityType == 'userAssigned' && newOrExistingManagedIdentity == 'new') {
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
    resourceGroup_workspace: resourceGroup_workspace
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
    resourceGroup_id: resourceGroup_id
    managedIdentityName: managedIdentityName
    storageAccountName: storageAccountName
    resourceGroup_workspace: resourceGroup_workspace
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
    resourceGroup_workspace: resourceGroup_workspace
    appInsightsName: appInsightsName
    planName: planName
    functionName: functionName
    location: location
  }
  dependsOn: [
    storageAccount
  ]
}
